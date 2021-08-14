using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace DXTF_SaveDecryptor
{
	class Program
	{
		static void Main(string[] args)
		{
			if(args.Length > 0 || File.Exists("dxmsave.sav"))
			{
				string file = "";
				if (args.Length > 0)
					file = string.Join(" ", args);
				else
					file = "dxmsave.sav";

				if (File.Exists(file))
				{
					string text = File.ReadAllText(file);
					try
					{
						text = DecryptData(text);
						var xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(text);
						xmlDoc.Save("decrypted.xml");
						Console.WriteLine("Seems like save file was decrypted successfully!");
						Console.ReadKey();
					}
					catch(Exception e)
					{
						Console.WriteLine("Error decrypting file: " + e);
						Console.ReadKey();
					}
				}
				else
				{
					Console.WriteLine("No save file exists?!");
					Console.ReadKey();
				}
			}
			else
			{
				Console.WriteLine("Please drag and drop a save file onto the exe or drop dxmsave.sav in the same folder as the program.");
				Console.ReadKey();
			}
		}

		internal static string DecryptData(string toDecrypt)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(" <stringid:1,24><stringid:1,25> ");
			byte[] array = Convert.FromBase64String(toDecrypt);
			ICryptoTransform cryptoTransform = new RijndaelManaged
			{
				Key = bytes,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			}.CreateDecryptor();
			byte[] bytes2 = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
			return Encoding.UTF8.GetString(bytes2);
		}
	}
}
