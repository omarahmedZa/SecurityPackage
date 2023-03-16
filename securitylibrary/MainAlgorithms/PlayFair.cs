using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        
        public string Decrypt(string cipherText, string key)
        {
            string plainText = "";
            string newCipherText;
            List<char> tempCipherText = new List<char>();
            List<char> keyList = new List<char>();
            List<char> editCipherText = new List<char>();
            char[,] matrix = new char[5, 5];

            keyList = keyTable(key);
            listToMatrix(keyList, matrix);

            for (int i = 0; i < cipherText.Length; i += 2)
            {
                char a;
                char b;
                decTwoL(matrix, cipherText[i], cipherText[i + 1], out a, out b);
                tempCipherText.Add(a);
                tempCipherText.Add(b);
            }

            newCipherText = string.Join("", tempCipherText);

            if (cipherText.Length % 2 != 0)
            {
                newCipherText = newCipherText.Remove(newCipherText.Length - 1, 1);
            }
            editCipherText = handelCipherText(newCipherText);

            plainText = string.Join("", editCipherText);

            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            string cipherText = "";
            string newPlainText;
            List<char> tempCipherText = new List<char>();
            List<char> keyList = new List<char>();
            List<char> editPlainText = new List<char>();
            char[,] matrix = new char[5, 5];
            
            keyList = keyTable(key);
            listToMatrix(keyList, matrix);

            editPlainText = handlePlainText(plainText);

            newPlainText = string.Join("", editPlainText);


            if (newPlainText.Length % 2 != 0)
            {
                newPlainText = newPlainText + 'x';
            }
            
            for (int i = 0; i < newPlainText.Length; i += 2)
            {
                char a;
                char b;
                encTwoL(matrix, newPlainText[i], newPlainText[i + 1], out a, out b);
                tempCipherText.Add(a);
                tempCipherText.Add(b);
            }

            cipherText = string.Join("", tempCipherText);
            

            return cipherText;
        }

        public List<char> handelCipherText(string ciphertext)
        {
            bool isCipherTextbeEdite = false;
            bool thereIsAnx;
            List<char> editCipherText = new List<char>();
            string tempCiphertext;

            for (int j = 0; j < ciphertext.Length - 1; j += 2)
            {
                editCipherText.Add(ciphertext[j]);
                thereIsAnx = false;
                
                if (ciphertext[j] == ciphertext[j + 2] && ciphertext[j + 1] == 'x' && !isCipherTextbeEdite)
                {
                    editCipherText.Add(ciphertext[j + 2]);
                    isCipherTextbeEdite = true;
                    thereIsAnx = true;
                }

                if (!thereIsAnx)
                {
                    editCipherText.Add(ciphertext[j + 1]);
                }
            }

            if (ciphertext.Length % 2 != 0)
            {
                editCipherText.Add(ciphertext[ciphertext.Length - 1]);
            }

            tempCiphertext = string.Join("", editCipherText);

            if (isCipherTextbeEdite)
            {
                editCipherText = handlePlainText(tempCiphertext);
            }

            return editCipherText;
        }

        public List<char> handlePlainText(string plainText)
        {
            bool isplainTextbeEdite = false;
            List<char> editPlainText = new List<char>();
            string tempPlaintext;

            for (int j = 0; j < plainText.Length - 1; j += 2)
            {
                editPlainText.Add(plainText[j]);
                if (plainText[j] == plainText[j + 1] && !isplainTextbeEdite)
                {
                    editPlainText.Add('x');
                    isplainTextbeEdite = true;
                }
                editPlainText.Add(plainText[j + 1]);
            }

            if(plainText.Length % 2 != 0)
            {
                editPlainText.Add(plainText[plainText.Length - 1]);
            }

            tempPlaintext = string.Join("", editPlainText);

            if (isplainTextbeEdite)
            {
                editPlainText =  handlePlainText(tempPlaintext);
            }

            return editPlainText;
        }

        public List<char> keyTable(string key)
        {
            List<char> alphabit = new List<char>();
            List<char> keyList = new List<char>();
            bool isThereAnI = false;
           

            for (char f = (char)97; f <= 122; f++)
            {
                if (f != (char)105)
                {
                    alphabit.Add(f);
                } 
            }

            for (int j = 0; j < key.Length; j++)
            {
                bool isCharInList = false;
                isThereAnI = false;

                if (key[j] == (char)105)
                {
                    isThereAnI = true;
                }

                for (int k = 0; k < keyList.Count; k++)
                {
                    if (key[j] == keyList[k] && !isThereAnI)
                    {
                        isCharInList = true;
                    }
                    else if(key[j] == keyList[k] && isThereAnI)
                    {
                        for (int l = 0; l < keyList.Count; l++)
                        {
                            if (keyList[l] == j)
                            {
                                isCharInList = true;
                                break;
                            }
                        }
                    }
                }

                if (!isCharInList && !isThereAnI)
                {
                    keyList.Add(key[j]);
                    alphabit.Remove(key[j]);
                }
                else if (isThereAnI && !isCharInList)
                {
                    keyList.Add('j');
                    alphabit.Remove('j');
                }
            }

            for(int r = 0; r < alphabit.Count; r++)
            {
                keyList.Add(alphabit[r]);
            }
            
            return keyList;
        }

        public void listToMatrix(List<char> keyList, char[,] matrix)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if(keyList[i * 5 + j] == 'j')
                    {
                        matrix[i, j] = 'i';
                    }
                    else
                    {
                        matrix[i, j] = keyList[i * 5 + j];
                    }
                    
                }
            }
        }


        public void decTwoL(char[,] matrix, char a1, char b1, out char a2, out char b2)
        {
            int i1 = 0, i2 = 0, j1 = 0, j2 = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {

                    if (a1 == matrix[i, j])
                    {
                        i1 = i;
                        j1 = j;
                    }

                    if (b1 == matrix[i, j])
                    {
                        i2 = i;
                        j2 = j;
                    }
                }
            }

            if (i1 == i2)
            {
                a2 = matrix[i1, (j1 - 1) % 5];
                b2 = matrix[i2, (j2 - 1) % 5];
            }
            else if (j1 == j2)
            {
                a2 = matrix[(i1 - 1) % 5, j1];
                b2 = matrix[(i2 - 1) % 5, j2];
            }
            else
            {
                a2 = matrix[i1, j2];
                b2 = matrix[i2, j1];
            }

        }

        public void encTwoL(char[,] matrix, char a1, char b1, out char a2, out char b2)
        {
            int i1 = 0, i2 = 0, j1 = 0, j2 = 0;
            

            for (int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                   
                    if(a1 == matrix[i, j])
                    {
                        i1 = i;
                        j1 = j;
                    }

                    if(b1 == matrix[i, j])
                    {
                        i2 = i;
                        j2 = j;
                    }
                }
            }


            if(i1 == i2)
            {
                a2 = matrix[i1, (j1 + 1) % 5];
                b2 = matrix[i2, (j2 + 1) % 5];
            }
            else if (j1 == j2)
            {
                a2 = matrix[(i1 + 1) % 5, j1];
                b2 = matrix[(i2 + 1) % 5, j2];
            }
            else
            {
                a2 = matrix[i1, j2];
                b2 = matrix[i2, j1];
            }
        }
    }
}
