using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        private readonly string Alphabets_Char = "abcdefghijklmnopqrstuvwxyz";
        public string Encrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(key))
                return string.Empty;

            var result = new StringBuilder();
            var keyWithPlainText = string.Concat(key, plainText).Substring(0, key.Length + plainText.Length - key.Length);

            for (int i = 0, len = plainText.Length; i < len; i++)
            {
                var plainCharIndex = Alphabets_Char.IndexOf(char.ToLower(plainText[i]));
                var keyCharIndex = Alphabets_Char.IndexOf(char.ToLower(keyWithPlainText[i]));
                var cipherCharIndex = (plainCharIndex + keyCharIndex) % 26;
                result.Append(Alphabets_Char[cipherCharIndex]);
            }

            return result.ToString();
        }
        public string Analyse(string plainText, string cipherText)
        {
            // Convert plainText to lowercase and assign it to a new variable
            string plainTextLower = plainText.ToLower();
            string key = "";
            // Loop through each character in the plainText and cipherText
            for (int i = 0; i < plainTextLower.Length; i++)
            {
                char cipherCharLower = cipherText.ToLower()[i];
                char plainCharLower = plainTextLower[i];
                int cipherIndex = Alphabets_Char.IndexOf(cipherCharLower);
                int plainIndex = Alphabets_Char.IndexOf(plainCharLower);
                // If the difference between the indices is negative, add 26 to it to get the correct index
                if (cipherIndex - plainIndex < 0)
                {
                    key += Alphabets_Char[(cipherIndex - plainIndex) + 26];
                }
                else
                {
                    key += Alphabets_Char[cipherIndex - plainIndex];
                }
            }
            string decryptedText = "";
            // Loop through each character in the key
            for (int i = 0; i < key.Length; i++)
            {
                string currentSubstring = "";
                // Loop through each character in key starting from the i-th index
                for (int j = i; j < key.Length; j++)
                {
                    currentSubstring += key[j];
                }
                // If the plainText contains the current substring, break out of the loop
                if (plainTextLower.Contains(currentSubstring))
                {
                    break;
                }
                // Otherwise, add the i-th character in key to the decrypted text
                else
                {
                    decryptedText += key[i];
                }
            }
            return decryptedText;
        }


        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();
            string Cipher_Text = cipherText;
            String Plain_Text = "";
            int x = 0;

            for (int i = 0; i < Cipher_Text.ToLower().Length; i++)
            {
                x++;
                //get index of cipher
                int get_index_of_cipher = (Alphabets_Char.IndexOf(Cipher_Text.ToLower()[i]) - Alphabets_Char.IndexOf(key[i]) % 26);
                //chek negative
                if (get_index_of_cipher < 0) get_index_of_cipher += 26;
                //complete key with rest of plain text
                key += Alphabets_Char[get_index_of_cipher];
                //set Plain text
                Plain_Text += Alphabets_Char[get_index_of_cipher];
            }
            Console.WriteLine("Plain_Text legnth is" + x);
            return Plain_Text;
        }


    }
}
