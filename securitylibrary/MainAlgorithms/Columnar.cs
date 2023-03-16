using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            int columns = GetColumnsNumber(plainText.Length);
            int rows = cipherText.Length / columns;
            cipherText = cipherText.ToLower();
            char[,] plainMatrix = FillPlainTextMatrix(columns, rows, plainText);
            char[,] cipherMatrix = FillCipherTextMatrix(columns, rows, cipherText);
            return GeneretKey(columns, rows, plainMatrix, cipherMatrix);
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            int cipherTextLength = cipherText.Length;
            int numbersOfColumn = cipherTextLength / key.Count;
            char[,] encriptionTable = new char[numbersOfColumn, key.Count];
            CreateEncreptionTable(key, numbersOfColumn, cipherText, encriptionTable);
            return GeneratePlainText(numbersOfColumn,key,encriptionTable);
        }

        public string Encrypt(string plainText, List<int> key)
        {
            int numberOfColumns = key.Count;
            int numberOfRows = (int)Math.Ceiling((double)plainText.Length / numberOfColumns);
            List<List<char>> table = new List<List<char>>();
            Dictionary<int, string> subcipherList = new Dictionary<int,string>();
            plainText = ModifyPlanText(plainText,numberOfRows,numberOfColumns);
            CreateTable(table,numberOfRows);
            FillTable(table,plainText,numberOfRows,numberOfColumns);
            CreateSubCiphers(subcipherList,table,numberOfRows,numberOfColumns,key);
            return GenerateCipherText(subcipherList);
        }

        /// Encryption Functions///
        private string ModifyPlanText(string plainText,int numberOfRows,int numberOfColumns)
        {
            if (plainText.Length != numberOfColumns * numberOfRows)
            {
                int x = (numberOfRows * numberOfColumns) - plainText.Length;
                string additionlChars = new string('x', x);
                plainText += additionlChars;
            }
            return plainText;
        }
        private List<List<char>> CreateTable(List<List<char>> table, int numberOfRows)
        {
            for (int i = 0; i < numberOfRows; i++)
            {
                table.Add(new List<char>());
            }
            return table;
        }
        private List<List<char>> FillTable(List<List<char>> table,string plainText,int numberOfRows,int numberOfColumns){
            int planTextIndex = 0;
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns && planTextIndex < plainText.Length; j++)
                {
                    table[i].Add(plainText[planTextIndex]);
                    planTextIndex++;
                }
            }
            return table;
        }
        private Dictionary<int,string>CreateSubCiphers(Dictionary<int, string> subcipherList, List<List<char>> table, int numberOfRows,int numberOfColumns, List<int> key)
        {
            for (int i = 0; i < numberOfColumns; i++)
            {
                string subString = "";
                for (int j = 0; j < numberOfRows; j++)
                {
                    subString += table[j][i];

                }
                subcipherList[key[i]] = subString;
            }
            return subcipherList;
        }
        private string GenerateCipherText(Dictionary<int, string> subcipher)
        {
            string cipherText = "";
            for (int i = 1; i <= subcipher.Count; i++)
            {
                cipherText += subcipher[i];
            }
            return cipherText;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////

        /// Decryption Functions///
        private char[,] CreateEncreptionTable(List<int> key, int col, string cipherText, char[,] encriptionTable)
        {
            int tmp = 0;
            for (int i = 0; i < key.Count; i++)
            {
                int k = key.IndexOf(i + 1);

                for (int j = 0; j < col; j++)
                {
                    if (tmp < cipherText.Length)
                    {
                        encriptionTable[j, k] = cipherText[tmp];
                        tmp++;
                    }
                }
            }
            return encriptionTable;
        }
        private string GeneratePlainText(int col, List<int> key, char[,] encriptionTable)
        {
            string plainText = "";
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < key.Count; j++)
                {
                    plainText += encriptionTable[i, j];
               
                }
            }
            return plainText;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////

        /// Analyse Functions/// <summary>
        public int GetColumnsNumber(int plainTextLength)
        {
            int columns = 0;
            for (int i = 2; i < 8; i++)
            {
                if (plainTextLength % i == 0)
                {
                    columns = i;
                }
            }
            return columns;
        }
        public char[,] FillPlainTextMatrix(int cols, int rows,string plaintext)
        {
            int counter = 0;
            char[,] matrix = new char[rows,cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (counter < plaintext.Length)
                    {
                        matrix[i, j] = plaintext[counter];
                        counter++;
                    }
                }
            }
            return matrix;
        }
        public char[,] FillCipherTextMatrix(int cols, int rows, string ciphertext)
        {
            int counter = 0;
            char[,] matrix = new char[rows, cols];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (counter < ciphertext.Length)
                    {
                        matrix[j, i] = ciphertext[counter];
                        counter++;
                    }
                }
            }
            return matrix;
        }
        public List<int> GeneretKey(int cols, int rows, char[,] plainMatrix, char[,] cipherMatrix)
        {
            int placedRight = 0;
            List<int> key = new List<int>(cols);
            for (int i = 0; i < cols; i++)
            {
                for (int k = 1; k <= cols; k++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if(plainMatrix[j, i] == cipherMatrix[j, k - 1])
                        {
                            placedRight++;
                        }
                        if (placedRight == rows)
                        {
                            key.Add(k);
                        }
                    }
                    placedRight = 0;
                }
            }
            if (key.Count == 0)
            {
                for (int i = 0; i < cols + 2; i++)
                {
                    key.Add(0);
                }
            }
            return key;
        }
    }
}
