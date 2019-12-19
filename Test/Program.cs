using System;
using System.Diagnostics;

namespace SickDev.BitPacking
{
	class Program
	{
		static int maxNumber = int.MaxValue;
		static int[] numbers = new int[10000000];
		static int[] significantBits = new int[numbers.Length];
		static byte[] compressedData;
		static Stopwatch stopwatch = new Stopwatch();

		static void Main(string[] args)
		{
			Random random = new Random();
			for (int i = 0; i < numbers.Length; i++)
				numbers[i] = random.Next(maxNumber);

			Compress();
			Decompress();
		}

		static void Compress()
		{
			BitWriter writter = new BitWriter();
			stopwatch.Start();

			for (int i = 0; i < numbers.Length; i++)
			{
				BinaryNumber binaryNumber = (BinaryNumber)numbers[i];
				significantBits[i] = binaryNumber.significantBits;
				writter.Write(binaryNumber);
			}

			Console.WriteLine(stopwatch.Elapsed.ToString());
			compressedData = writter.GetBytes();

			Console.WriteLine(string.Format("---Initial state---\nNumbers: {0}\nBytes: {1}\n\n---Final state---\nBytes: {2}",
				numbers.Length, numbers.Length * sizeof(int), compressedData.Length));
			Console.WriteLine();
		}

		static void Decompress()
		{
			int[] result = new int[numbers.Length];
			GC.Collect();
			BitReader reader = new BitReader(compressedData);
			stopwatch.Restart();
			for (int i = 0; i < result.Length; i++)
				result[i] = (int)reader.Read(significantBits[i]);

			bool good = true;
			for (int i = 0; i < result.Length; i++)
			{
				if (result[i] != numbers[i])
				{
					good = false;
					break;
				}
			}

			Console.WriteLine($"{(good?"Good":"Wrong")}! {stopwatch.Elapsed.ToString()}");
		}
	}
}