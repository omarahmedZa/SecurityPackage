﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            string plainText = "";

            return plainText;
        }

        public override string Encrypt(string plainText, string key)
        {
            string cipherText = "";

            return cipherText;
        }
    }
}
