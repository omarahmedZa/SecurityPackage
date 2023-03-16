using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string key = "";

            char[] key1 = new char[26];
            int[] indx1 = new int[26];


            cipherText = cipherText.ToLower();
            cipherText = cipherText.ToUpper();
            cipherText = cipherText.ToLower();

            for (int i = 0; i < plainText.Length; i++)
            {
                int indx_p = plainText[i] - 'a';
                int indx_c = cipherText[i] - 'a';

                key1[indx_p] = cipherText[i];
                indx1[indx_c] = 1;
            }

            int j = 0;
            while (j < 26)
            {
                if (key1[j] >= 'a')
                {
                    if (key1[j] <= 'z')
                    {
                        j++;
                        continue;
                    }
                }
                int k = 0;
                while (k < 26)
                {
                    if (indx1[k] == 1)
                    {
                        k++;
                        continue;
                    }
                    char c = (char)('a' + k);
                    key1[j] = c;
                    indx1[k] = 1;

                    if (true)
                    {
                        if (true)
                        {

                        }
                    }
                    break;
                    k++;
                }
                j++;
            }
            for (int m = 0; m < 26; m++)
            {
                key += key1[m];
            }
            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            cipherText = cipherText.ToUpper();
            cipherText = cipherText.ToLower();

            char[] Decrypted_txt_arr = new char[cipherText.Length];
            char[] New_key = new char[26];

            char new_char;
            int index;
            int index2;

            for (int i = 0; i < New_key.Length; i++)
            {
                new_char = (char)('a' + i);
                index = key[i] - 'a';
                New_key[index] = new_char;
            }
            for (int j = 0; j < cipherText.Length; j++)
            {
                index2 = cipherText[j] - 'a';
                Decrypted_txt_arr[j] += New_key[index2];
            }
            return new string(Decrypted_txt_arr);
        }

        public string Encrypt(string plainText, string key)
        {
            char[] encrypted_txt_arr = new char[plainText.Length];
            int index;

            for (int i = 0; i < plainText.Length; i++)
            {
                index = plainText[i] - 'a';
                encrypted_txt_arr[i] += key[index];
            }

            return new string(encrypted_txt_arr);
        }


        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            string frequency = "ETAOINSRHLDCUMFPGWYBVKXJQZ".ToLower();
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            char[] cipher_arr = cipher.ToLower().ToCharArray();
            int[] count = new int[26];
            char[] arr = new char[26];
            int id;
            char[] plaintext = new char[cipher.Length];

            // Count the frequency of each character in the cipher array
            Dictionary<char, int> freq = new Dictionary<char, int>();
            foreach (char c in cipher_arr)
            {
                if (freq.ContainsKey(c))
                {
                    freq[c]++;
                }
                else
                {
                    freq[c] = 1;
                }
            }

            // Sort the characters in descending order of frequency
            var sortedFreq = freq.OrderByDescending(x => x.Value);

            // Map the most frequent cipher characters to their corresponding plaintext characters
            Dictionary<char, char> mapping = new Dictionary<char, char>();
            int i = 0;
            foreach (var item in sortedFreq)
            {
                mapping[item.Key] = frequency[i];
                i++;
            }

            // Decrypt the cipher text using the mapping
            for (int j = 0; j < cipher.Length; j++)
            {
                if (mapping.ContainsKey(cipher_arr[j]))
                {
                    plaintext[j] = mapping[cipher_arr[j]];
                }
                else
                {
                    plaintext[j] = cipher_arr[j];
                }
            }

            Console.WriteLine(plaintext);
            return new string(plaintext);
        }
    }
}


