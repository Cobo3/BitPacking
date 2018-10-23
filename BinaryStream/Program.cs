using System;

namespace SickDev.BinaryCompressor {
    class Program {
        static void Main(string[] args) {
            BinaryCompressor compressor = new BinaryCompressor();
            Random random = new Random();
            int amount = 100;
            for (int i = 0; i < amount; i++)
                compressor.Write(random.Next(0, 1));
            hay un bug cuando fguardo solamente 1s
            Console.WriteLine(string.Format("---Initial state---\nNumbers: {0}\nBytes: {1}\n\n---Final state---\nBytes: {2}",
                amount, amount*sizeof(int), compressor.GetBytes().Length*sizeof(byte)));
        }
    }
}
