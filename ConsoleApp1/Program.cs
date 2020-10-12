using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var newb = Decompress(File.ReadAllBytes("tutorial_whisper_b.txt"));
            File.WriteAllBytes("new.txt", newb);
        }
        public static Byte[] Decompress(Byte[] buffer)
        {
            MemoryStream resultStream = new MemoryStream();

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    ds.CopyTo(resultStream);
                    ds.Close();
                }
            }
            Byte[] decompressedByte = resultStream.ToArray();
            resultStream.Dispose();
            return decompressedByte;
        }
    }
}
