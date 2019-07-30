using System;
using System.Linq;

namespace SickDev.BinaryCompressor {
    class Program {
        static int maxNumber = byte.MaxValue;
		static byte[] numbers = new byte[10000];
        static byte[] compressedData;
        static BinaryCompressor compressor;

        static void Main(string[] args) {
			Random random = new Random(0);
			for (int i = 0; i < numbers.Length; i++)
				numbers[i] = (byte)random.Next(maxNumber);

			//numbers = new int[] {
			//	255
			//};

			//for (int i = 0; i < numbers.Length; i++)
			//	Console.WriteLine(i + ": " + numbers[i]);

			Compress();
            Decompress();
        }

        static void Compress() {
            compressor = new BinaryCompressor(numbers.Max());
            for (int i = 0; i < numbers.Length; i++)
                compressor.Write(numbers[i]);
            compressedData = compressor.GetBytes();

            Console.WriteLine(string.Format("---Initial state---\nNumbers: {0}\nBytes: {1}\n\n---Final state---\nBytes: {2}",
                numbers.Length, numbers.Length * sizeof(byte), compressedData.Length));
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