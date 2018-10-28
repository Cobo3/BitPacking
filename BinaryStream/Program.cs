using System;
using System.Linq;

namespace SickDev.BinaryCompressor {
    class Program {
        static int maxNumber = ushort.MaxValue;
        static int[] numbers = new int[1];
        static byte[] compressedData;

        static void Main(string[] args) {
            Random random = new Random();
            for (int i = 0; i < numbers.Length; i++)
                numbers[i] = 3765;// random.Next(maxNumber);

            Compress();
            Decompress();
        }

        static void Compress() {
            BinaryCompressor compressor = new BinaryCompressor(numbers.Max());
            for (int i = 0; i < numbers.Length; i++)
                compressor.Write(numbers[i]);
            compressedData = compressor.GetBytes();

            Console.WriteLine(string.Format("---Initial state---\nNumbers: {0}\nBytes: {1}\n\n---Final state---\nBytes: {2}",
                numbers.Length, numbers.Length * sizeof(int), compressedData.Length));
            Console.WriteLine();
        }

        static void Decompress() {
            BinaryDecompressor decompressor = new BinaryDecompressor(compressedData, maxNumber);
            BinaryNumber[] decompressedNumbers = decompressor.ReadAll();
        }
    }
}
