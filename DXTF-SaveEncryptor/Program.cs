using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DXTF_SaveEncryptor
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length > 0 || File.Exists("decrypted.xml"))
			{
				string file = "";
				if (args.Length > 0)
					file = string.Join(" ", args);
				else
					file = "decrypted.xml";

				if (File.Exists(file))
				{
					string text = File.ReadAllText(file);
					try
					{
						var xmlDoc = new XmlDocument();
						xmlDoc.LoadXml(text);
						text = EncryptData(text);
						File.WriteAllText("dxmsave.sav", text);
						Console.WriteLine("Seems like save file was encrypted successfully!");
						Console.ReadKey();
					}
					catch (Exception e)
					{
						Console.WriteLine("Error encrypting file: " + e);
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
				Console.WriteLine("Please drag and drop a file to encrypt or drop decrypted.xml in the same folder as the program.");
				Console.ReadKey();
			}
		}

		private static string EncryptData(string toEncrypt)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(" <stringid:1,24><stringid:1,25> ");
			byte[] bytes2 = Encoding.UTF8.GetBytes(toEncrypt);
			ICryptoTransform cryptoTransform = new RijndaelManaged
			{
				Key = bytes,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			}.CreateEncryptor();
			byte[] array = cryptoTransform.TransformFinalBlock(bytes2, 0, bytes2.Length);
			return Convert.ToBase64String(array, 0, array.Length);
		}
	}
}
