using System;

namespace SickDev.BinaryCompressor
{
	class Program
	{
		static int maxNumber = ushort.MaxValue;
		static int[] numbers = new int[maxNumber + 1];
		static byte[] compressedData;
		static BinaryCompressor compressor;

		static void Main(string[] args)
		{
			Random random = new Random(0);
			for (int i = 0; i < numbers.Length; i++)
				numbers[i] = i;// random.Next(maxNumber);

			//numbers = new int[] {
			//	1
			//};

			//for (int i = 0; i < numbers.Length; i++)
			//	Console.WriteLine(i + ": " + numbers[i]);

			Compress();
			Decompress();
		}

		static void Compress()
		{
			compressor = new BinaryCompressor();
			for (int i = 0; i < numbers.Length; i++)
			{
				compressor.Write(numbers[i], 32);
			}
			compressedData = compressor.GetBytes();

			Console.WriteLine(string.Format("---Initial state---\nNumbers: {0}\nBytes: {1}\n\n---Final state---\nBytes: {2}",
				numbers.Length, numbers.Length * sizeof(int), compressedData.Length));
			Console.WriteLine();
		}

		static void Decompress()
		{
			int[] result = new int[numbers.Length];
			BinaryDecompressor decompressor = new BinaryDecompressor(compressedData);
			for (int i = 0; i < result.Length; i++)
			{
				BinaryNumber value = decompressor.Read(32);
				result[i] = (int)value;
			}


			if (result.Length != numbers.Length)
			{
				Console.WriteLine("Wrong!");
				return;
			}

			bool wrong = false;
			for (int i = 0; i < result.Length; i++)
			{
				if (result[i] != numbers[i])
				{
					wrong = true;
					break;
				}
			}

			if (wrong)
				Console.WriteLine("Wrong!");
		}
	}
}