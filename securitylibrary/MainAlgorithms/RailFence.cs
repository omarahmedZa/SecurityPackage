﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();

            int i = 1;
            int z = 0;

            while (i <= plainText.Length)
            {
                string s = Encrypt(plainText, i);

                if (s.Equals(cipherText))
                {
                    return i;
                }

                i++;
            }

            return z;
        }


        public string Decrypt(string cipherText, int key)
        {
            int numRows = (int)Math.Ceiling((double)cipherText.Length / key);
            char[,] matrix = new char[key, numRows];
            int index = 0;

            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < numRows; j++)
                {
                    if (index < cipherText.Length)
                    {
                        matrix[i, j] = cipherText[index++];
                    }
                }
            }

            StringBuilder plainText = new StringBuilder(cipherText.Length);

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < key; j++)
                {
                    if (matrix[j, i] != '\0')
                    {
                        plainText.Append(matrix[j, i]);
                    }
                }
            }

            return plainText.ToString();
        }

        public string Encrypt(string plainText, int key)
        {
            int numRows = (int)Math.Ceiling((double)plainText.Length / key);
            char[,] matrix = new char[key, numRows];
            int index = 0;

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < key; j++)
                {
                    if (index < plainText.Length)
                    {
                        matrix[j, i] = plainText[index++];
                    }
                }
            }

            StringBuilder cipherText = new StringBuilder(plainText.Length);

            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < numRows; j++)
                {
                    if (matrix[i, j] != '\0')
                    {
                        cipherText.Append(matrix[i, j]);
                    }
                }
            }
            return cipherText.ToString().ToUpper();
        }
    }

}
