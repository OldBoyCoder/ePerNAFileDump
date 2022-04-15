using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePerNAFileDump
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"C:\Program Files (x86)\Fiat\ePER\data\SP.NA.00900.FCTLR", "*.na");
            foreach (var file in files)
            {
                Console.WriteLine(file);
                ProcessNaFile(file);
            }
        }
        static void ProcessNaFile(string fileName)
        {
            var folder = $"C:\\temp\\eperimages\\{Path.GetFileNameWithoutExtension(fileName)}";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            var reader = new BinaryReader(File.OpenRead(fileName));
            reader.ReadInt16();
            Int32 numberOfEntries = reader.ReadInt16();
            for (int i = 0; i < numberOfEntries; i++)
            {
                Int16 imageIndex = reader.ReadInt16();
                byte[] imageNameBytes = reader.ReadBytes(10);
                string imageName = System.Text.Encoding.ASCII.GetString(imageNameBytes);
                Int32 mainImageStart = reader.ReadInt32();
                Int32 mainImageLength = reader.ReadInt32();
                Int32 thumbImageStart = reader.ReadInt32();
                Int32 thumbImageLength = reader.ReadInt32();
                Console.WriteLine($"{imageIndex} {imageName} {mainImageStart} {mainImageLength} {thumbImageStart} {thumbImageLength}");
                var pos = reader.BaseStream.Position;
                reader.BaseStream.Seek(mainImageStart, SeekOrigin.Begin);
                var imageBytes = reader.ReadBytes(mainImageLength);
                if (imageBytes[0] != 137 || imageBytes[1] != 80 || imageBytes[2] != 78 || imageBytes[3] != 71)
                    Console.WriteLine("Not a PNG");
                using (var output = new BinaryWriter(File.Open(Path.Combine(folder, $"{imageName}.PNG"), FileMode.Create)))
                {
                    output.Write(imageBytes);
                }
                reader.BaseStream.Seek(thumbImageStart, SeekOrigin.Begin);
                imageBytes = reader.ReadBytes(thumbImageLength);
                using (var output = new BinaryWriter(File.Open(Path.Combine(folder, $"{imageName}Thumb.PNG"), FileMode.Create)))
                {
                    output.Write(imageBytes);
                }
                reader.BaseStream.Seek(pos, SeekOrigin.Begin);

            }

        }
    }
}
