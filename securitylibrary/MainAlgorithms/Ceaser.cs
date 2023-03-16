using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            char[] Original_Text = plainText.ToCharArray();
            char[] encrypted_txt = new char[Original_Text.Length];

            int New_index;

            for (int i = 0; i < Original_Text.Length; i++)
            {
                New_index = ((Original_Text[i] - 'a') + key) % 26;
                encrypted_txt[i] = (char)('a' + New_index);

            }
            return new string(encrypted_txt);
        }

        public string Decrypt(string cipherText, int key)
        {
            //All letters are in upper case in CIPHER_TEXT

            char[] encrypted_txt = cipherText.ToCharArray();
            char[] Orignal_txt = new char[encrypted_txt.Length];

            int New_index;

            for (int i = 0; i < encrypted_txt.Length; i++)
            {
                New_index = (((encrypted_txt[i] - 'A') - key) + 26) % 26;
                Orignal_txt[i] = (char)('A' + New_index);

            }
            return new string(Orignal_txt);
        }

        public int Analyse(string plainText, string cipherText)
        {
            int key;
            key = (((cipherText[0] - 'A') - (plainText[0] - 'a')) + 26) % 26;
            return key;
        }
    }
}
