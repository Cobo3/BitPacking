﻿using System;
using System.Linq;

namespace SickDev.BinaryCompressor {
    class Program {
        static int maxNumber = ushort.MaxValue;
        static int[] numbers = new int[100];
        static byte[] compressedData;
        static BinaryCompressor compressor;

        static void Main(string[] args) {
            Random random = new Random();
			for (int i = 0; i < numbers.Length; i++)
				numbers[i] = random.Next(maxNumber);

            Compress();
            Decompress();
        }

        static void Compress() {
            compressor = new BinaryCompressor(numbers.Max());
            for (int i = 0; i < numbers.Length; i++)
                compressor.Write(numbers[i]);
            compressedData = compressor.GetBytes();

            Console.WriteLine(string.Format("---Initial state---\nNumbers: {0}\nBytes: {1}\n\n---Final state---\nBytes: {2}",
                numbers[0], numbers.Length * sizeof(int), compressedData.Length));
            Console.WriteLine();
        }

        static void Decompress() {
            BinaryDecompressor decompressor = new BinaryDecompressor(compressedData, numbers.Max());
            BinaryNumber[] decompressedNumbers = decompressor.ReadAll();

            int[] result = decompressedNumbers.Select(x => (int)x).ToArray();

            if (result.Length != numbers.Length) {
                Console.WriteLine("Wrong!");
                return;
            }

            bool wrong = false;
            for (int i = 0; i < result.Length; i++) {
                if (result[i] != numbers[i]) {
                    wrong = true;
                    break;
                }
            }

            if (wrong)
                Console.WriteLine("Wrong!");
        }
    }
}