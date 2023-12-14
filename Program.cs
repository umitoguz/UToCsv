using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UToCsv
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            string _dataPath = "\\EMNISTCSV\\";
            string imagesPath = "\\EMNISTCSV\\t10k-images-idx3-ubyte";
            string labelsPath =  "\\EMNISTCSV\\t10k-labels-idx1-ubyte";
            string outputPath = _dataPath + "emnist.csv";

            var images = ReadEMNISTImages(imagesPath);
            var labels = ReadEMNISTLabels(labelsPath);

            WriteToCSV(images, labels, outputPath);

            Console.WriteLine("EMNIST veri seti CSV'ye dönüþtürüldü.");
        }
        static byte[][] ReadEMNISTImages(string path)
        {
            using (var file = new FileStream(path, FileMode.Open))
            using (var reader = new BinaryReader(file))
            {
                byte[] intBytes = reader.ReadBytes(4); // 4 byte oku
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intBytes); // Küçük-endian ise, diziyi ters çevir
                int magicNumber = BitConverter.ToInt32(intBytes, 0);
                byte[] intBytes1 = reader.ReadBytes(4); // 4 byte oku
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intBytes1); // Küçük-endian ise, diziyi ters çevir
                int numberOfImages = BitConverter.ToInt32(intBytes1, 0);
                byte[] intBytes2 = reader.ReadBytes(4); // 4 byte oku
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intBytes2); // Küçük-endian ise, diziyi ters çevir
                int numberOfRows = BitConverter.ToInt32(intBytes2, 0);
                byte[] intBytes3 = reader.ReadBytes(4); // 4 byte oku
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intBytes3); // Küçük-endian ise, diziyi ters çevir
                int numberOfColumns = BitConverter.ToInt32(intBytes3, 0);
                //int magicNumber = reader.ReadInt32BigEndian();
                //int numberOfImages = reader.ReadInt32BigEndian();
                //int numberOfRows = reader.ReadInt32BigEndian();
                //int numberOfColumns = reader.ReadInt32BigEndian();

                var images = new byte[numberOfImages][];

                for (int i = 0; i < numberOfImages; i++)
                {
                    byte[] pixels = reader.ReadBytes(numberOfRows * numberOfColumns);
                    images[i] = pixels;
                }

                return images;
            }
        }

       static byte[] ReadEMNISTLabels(string path)
        {
            using (var file = new FileStream(path, FileMode.Open))
            using (var reader = new BinaryReader(file))
            {


                //int magicNumber = reader.ReadInt32BigEndian();
                byte[] intBytes = reader.ReadBytes(4); // 4 byte oku
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intBytes); // Küçük-endian ise, diziyi ters çevir
                int magicNumber = BitConverter.ToInt32(intBytes, 0);
                //int numberOfItems = reader.ReadInt32BigEndian();
                byte[] intBytes1 = reader.ReadBytes(4); // 4 byte oku
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intBytes1); // Küçük-endian ise, diziyi ters çevir
                int numberOfItems = BitConverter.ToInt32(intBytes1, 0);

                var labels = new byte[numberOfItems];

                for (int i = 0; i < numberOfItems; i++)
                {
                    labels[i] = reader.ReadByte();
                }

                return labels;
            }
        }

        static void WriteToCSV(byte[][] images, byte[] labels, string outputPath)
        {
            using (var writer = new StreamWriter(outputPath))
            {
                writer.WriteLine("Label,PixelValues");

                for (int i = 0; i < images.Length; i++)
                {
                    string pixelValues = string.Join(",", images[i]);
                    writer.WriteLine($"{labels[i]},{pixelValues}");
                }
            }
        }

    }
}
