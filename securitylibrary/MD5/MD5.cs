using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.MD5
{
    public class MD5
    {
        static int[] s = new int[64] {
        7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
        5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
        4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
        6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
        };
        static uint[] K = new uint[64] {
        0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
        0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
        0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
        0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
        0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
        0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
        0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
        0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
        0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
        0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
        0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
        0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
        0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
        0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
        0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
        0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
        };
        public string GetHash(string text)
        {
            byte[] input = Encoding.ASCII.GetBytes(text);
            uint a0 = 0x67452301;   // A
            uint b0 = 0xefcdab89;   // B
            uint c0 = 0x98badcfe;   // C
            uint d0 = 0x10325476;   // D

            var addLength = (56 - ((input.Length + 1) % 64)) % 64; // calculate the new length with padding
            var processedInput = new byte[input.Length + 1 + addLength + 8];
            Array.Copy(input, processedInput, input.Length);
            processedInput[input.Length] = 0x80; // add 1

            byte[] length = BitConverter.GetBytes(input.Length * 8); // bit converter returns little-endian
            Array.Copy(length, 0, processedInput, processedInput.Length - 8, 4); // add length in bits

            for (int i = 0; i < processedInput.Length / 64; ++i)
            {
                // copy the input to M
                uint[] M = new uint[16];
                for (int j = 0; j < 16; ++j)
                {
                    M[j] = BitConverter.ToUInt32(processedInput, i * 64 + j * 4);
                }

                //initialize
                uint A = a0;
                uint B = b0;
                uint C = c0;
                uint D = d0;

                // main loop
                for (int j = 0; j < 64; ++j)
                {
                    uint F, g;
                    if (j < 16)
                    {
                        F = (B & C) | (~B & D);
                        g = (uint)j;
                    }
                    else if (j < 32)
                    {
                        F = (D & B) | (~D & C);
                        g = (uint)((5 * j + 1) % 16);
                    }
                    else if (j < 48)
                    {
                        F = B ^ C ^ D;
                        g = (uint)((3 * j + 5) % 16);
                    }
                    else
                    {
                        F = C ^ (B | ~D);
                        g = (uint)((7 * j) % 16);
                    }

                    uint tempD = D;
                    D = C;
                    C = B;
                    B = B + leftRotate((A + F + K[j] + M[g]), s[j]);
                    A = tempD;
                }

                // Add this chunk's hash to result so far:
                a0 += A;
                b0 += B;
                c0 += C;
                d0 += D;
            }

            //Produce the final hash value as a 32 character length string
            byte[] md5Bytes = new byte[16];
            BitConverter.GetBytes(a0).CopyTo(md5Bytes, 0);
            BitConverter.GetBytes(b0).CopyTo(md5Bytes, 4);
            BitConverter.GetBytes(c0).CopyTo(md5Bytes, 8);
            BitConverter.GetBytes(d0).CopyTo(md5Bytes, 12);

            return string.Concat(md5Bytes.Select(x => x.ToString("x2")));
        }
        static uint leftRotate(uint x, int n)
        {
            return (x << n) | (x >> (32 - n));
        }
    }
}
