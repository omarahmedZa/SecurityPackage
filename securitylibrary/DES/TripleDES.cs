using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {
        public string Decrypt(string cipherText, List<string> key)
        {
            string plaintext = "";

            plaintext = DesDecrypt(cipherText, key[1]);
            plaintext = DesEncrypt(plaintext, key[0]);
            plaintext = DesDecrypt(plaintext, key[1]);

            return plaintext;
        }

        public string Encrypt(string plainText, List<string> key)
        {
            string cipherText = "";

            cipherText = DesEncrypt(plainText, key[0]);
            cipherText = DesDecrypt(cipherText, key[1]);
            cipherText = DesEncrypt(cipherText, key[0]);

            return cipherText;
        }

        public List<string> Analyse(string plainText,string cipherText)
        {
            throw new NotSupportedException();
        }
        public string DesDecrypt(string cipherText, string key)
        {
            key = Convert.ToString(Convert.ToInt64(key, 16), 2).PadLeft(64, '0');

            int[] Pc1 = { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59,
                51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62,
                54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };

            key = permutation(key, Pc1);


            int[] shift_table = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

            int[] Pc2 = { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26,
                8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49,
                39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };


            // Split
            string C = key.Substring(0, 28);
            string D = key.Substring(28);

            List<string> Keys = new List<string>();
            List<string> KeysEnverse = new List<string>();

            for (int i = 0; i < 16; i++)
            {
                C = Shift_Left(C, shift_table[i]);
                D = Shift_Left(D, shift_table[i]);
                string K = C + D;
                string Round_Key = permutation(K, Pc2);
                Keys.Add(Round_Key);
            }

            for (int j = 15; j >= 0; j--)
            {
                KeysEnverse.Add(Keys[j]);
            }

            string plainText = encrypt_decrypt(cipherText, KeysEnverse);

            return plainText;
        }
        public string DesEncrypt(string plainText, string key)
        {


            key = Convert.ToString(Convert.ToInt64(key, 16), 2).PadLeft(64, '0');

            int[] Pc1 = { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59,
                51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62,
                54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };

            key = permutation(key, Pc1);


            int[] shift_table = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

            int[] Pc2 = { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26,
                8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49,
                39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };


            // Split
            string C = key.Substring(0, 28);
            string D = key.Substring(28);

            List<string> Keys = new List<string>();

            for (int i = 0; i < 16; i++)
            {
                C = Shift_Left(C, shift_table[i]);
                D = Shift_Left(D, shift_table[i]);
                string K = C + D;
                string Round_Key = permutation(K, Pc2);
                Keys.Add(Round_Key);


            }
            string cipher = encrypt_decrypt(plainText, Keys);
            return cipher;
        }

        public string permutation(string s, int[] arr)
        {
            string perm = "";
            for (int i = 0; i < arr.Length; i++)
            {
                perm += s[arr[i] - 1];
            }
            return perm;
        }
        public string Shift_Left(string s, int Shift)
        {
            string k = "";
            for (int i = 0; i < Shift; i++)
            {
                for (int j = 1; j < 28; j++)
                {
                    k += s[j];
                }
                k += s[0];
                s = k;
                k = "";
            }
            return s;
        }

        public string XOR(string x, string y)
        {
            string Result = "";
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] == y[i])
                {
                    Result += "0";
                }
                else
                {
                    Result += "1";
                }
            }
            return Result;
        }
        public string encrypt_decrypt(string Text, List<string> keys)
        {
            // hexadecimal to binary
            Text = Convert.ToString(Convert.ToInt64(Text, 16), 2).PadLeft(64, '0');

            // initial Permutation table
            int[] init_perm = { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46,
                38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35,
                27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };

            Text = permutation(Text, init_perm);

            string L = Text.Substring(0, 32);
            string R = Text.Substring(32);

            // expansion table
            int[] ExpansionTable = { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15,
                16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };

            // S-box table
            int[][][] S_Box =
            {
                new int[][]
                {
                    new int[] {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
                    new int[] {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
                    new int[] {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
                    new int[] {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
                },
                new int[][]
                {
                    new int[] {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
                    new int[] {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
                    new int[] {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
                    new int[] {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
                },
                new int[][]
                {
                    new int[] {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
                    new int[] {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
                    new int[] {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
                    new int[] {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
                },
                new int[][]
                {
                    new int[] {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
                    new int[] {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
                    new int[] {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
                    new int[] {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
                },
                new int[][]
                {
                    new int[] {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
                    new int[] {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
                    new int[] {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
                    new int[] {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
                },
                new int[][]
                {
                    new int[] {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
                    new int[] {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
                    new int[] {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
                    new int[] {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
                },
                new int[][]
                {
                    new int[] {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
                    new int[] {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
                    new int[] {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
                    new int[] {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
                },
                new int[][]
                {
                    new int[] {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
                    new int[] {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
                    new int[] {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
                    new int[] {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
                }
            };


            //permutation table
            int[] per = { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14,
                          32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };

            for (int i = 0; i < 16; i++)
            {
                string Right_Expanded = permutation(R, ExpansionTable);
                string x = XOR(keys[i], Right_Expanded);
                string Ope = "";
                for (int j = 0; j < 8; j++)
                {
                    int ROW = Convert.ToInt32(x[j * 6].ToString() + x[j * 6 + 5].ToString(), 2);
                    int COL = Convert.ToInt32(x[j * 6 + 1].ToString() + x[j * 6 + 2].ToString() +
                                                x[j * 6 + 3].ToString() + x[j * 6 + 4].ToString(), 2);
                    int val = S_Box[j][ROW][COL];
                    Ope = Ope + Convert.ToString(val, 2).PadLeft(4, '0');

                }
                Ope = permutation(Ope, per);
                L = XOR(Ope, L);


                string tmp = L;
                L = R;
                R = tmp;


            }

            string Combined_key = R + L;

            int[] Final_Permutation = { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 6,
                46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35,
                3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };

            string Final = "0x" + Convert.ToInt64(permutation(Combined_key, Final_Permutation), 2).ToString("x").PadLeft(16, '0');

            return Final;
        }
    }
}
