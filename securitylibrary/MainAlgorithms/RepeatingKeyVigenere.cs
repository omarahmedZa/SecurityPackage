using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            var matrix = from i in Enumerable.Range(0, 26)
                         from j in Enumerable.Range(0, 26)
                         select alphabet[(i + j) % 26];

            string key = "";

            for (int i = 0; i < plainText.Length; i++)
            {
                var possibleKeys = from j in Enumerable.Range(0, 26)
                                   where matrix.ElementAt(j * 26 + (plainText[i] - 'a')) == cipherText[i]
                                   select alphabet[j];

                if (possibleKeys.Count() == 1)
                {
                    key += possibleKeys.First();
                }
                else
                {
                    return "";
                }

                if (Encrypt(plainText, key).ToLower() == cipherText)
                {
                    return key;
                }
            }

            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();

            StringBuilder plaintext = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i++)
            {
                char keyChar = key[i % key.Length];

                char decryptedChar = (char)(((cipherText[i] - keyChar + 26) % 26) + 'A');

                plaintext.Append(decryptedChar);
            }

            // Return the plaintext as a string
            return plaintext.ToString();
        }

        public string Encrypt(string plainText, string key)
        {
           
            plainText = plainText.ToUpper();
            key = key.ToUpper();

           
            StringBuilder ciphertext = new StringBuilder();

          
            for (int i = 0; i < plainText.Length; i++)
            {
                
                char keyChar = key[i % key.Length];

               
                char encryptedChar = (char)(((plainText[i] + keyChar - 2 * 'A') % 26) + 'A');

               
                ciphertext.Append(encryptedChar);
            }

           
            return ciphertext.ToString();
        }
    }
}