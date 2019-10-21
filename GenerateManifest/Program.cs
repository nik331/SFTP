using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GenerateManifest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the path of a folder which has template : ");
            var path = Console.ReadLine();
            StringBuilder stringBuilder = new StringBuilder();
            var list = Directory.EnumerateFiles(path);

            foreach (var file in list)
            {
                var hashAlgorithm = SHA1.Create();             
                var fileData = File.ReadAllBytes(file);
                var value = BitConverter.ToString(hashAlgorithm.ComputeHash(fileData)).Replace("-", string.Empty);

                var data = string.Format(@""".\{0},""SHA1"",""{1}""", Path.GetFileName(file), value);
                stringBuilder.Append(data);
                stringBuilder.AppendLine();
            }
            File.WriteAllText(path + @"\manifest.txt", stringBuilder.ToString());

            Console.WriteLine("Generated manifest file is available at : " + path);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
