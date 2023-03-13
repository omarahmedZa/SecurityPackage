using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> plainText = new List<int>();


            return plainText;
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> cipherText = new List<int>();

            //dot product

            double keyDimentions = matrixDimentions(key.Count);

            int halfResult, result = 0, counter = 0;

            for (int i = 0; i < plainText.Count; i++)
            {
                counter++;
                halfResult = plainText[i] * key[counter - 1];
                result += halfResult;

                if ((counter) % keyDimentions == 0)
                {
                    cipherText.Add(result % 26);
                    result = 0;
                    
                    if ((counter) % key.Count != 0)
                    {
                        i -= int.Parse(keyDimentions.ToString());
                    }
                }

                if ((counter) % key.Count == 0)
                {
                    counter = 0;
                }
            }

            return cipherText;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

        public double matrixDimentions(double listCounter)
        {
            return Math.Sqrt(listCounter);
        }

        public double matrixDeterminand(List<int> key)
        {
            double keyDimentions = matrixDimentions(key.Count);

            double determinand = 0;

            if (keyDimentions < 3)
            {
                determinand = (key[0] * key[2] - key[1] * key[3]) % 26;
            }
            else
            {
                determinand = (key[0] * (key[4] * key[8] - key[5] * key[7])
                    - key[1] * (key[3] * key[8] - key[5] * key[6])
                    + key[2] * (key[3] * key[7] - key[4] * key[6])) % 26;
            }

            return determinand;
        }

        public double multiplicativeInverse(double determinand)
        {

            double b, c;

            int i;

            for (i = 1; (i / (26 - determinand)) == (int)(i / (26 - determinand)); i += 26) ;

            c = i / (26 - determinand);
            b = 26 - c;

            return b;
        }


        public void transformToMatrix(List<int> list, int[,] matrix)
        {
            int mDim = (int)matrixDimentions(list.Count);
            for (int i = 0; i < mDim; i++)
            {
                for (int j = 0; j < mDim; j++)
                {
                    matrix[i, j] = list[i * mDim + j];
                }
            }
        }

        public int elementDeterminand(int row, int col, int[,] matrix, int mDim)
        {
            int determinand;
            int[,] dMatrix = new int[mDim - 1, mDim - 1];
            for (int i = 0; i < mDim; i++)
            {
                for (int j = 0; j < mDim; j++)
                {
                    if(i != row && j != col)
                    {
                        dMatrix[i, j] = matrix[i, j];
                    }
                }
            }

            determinand = (dMatrix[0, 0] * dMatrix[1, 1] - dMatrix[0, 1] * dMatrix[1, 0]);

            return determinand;
        }

        public List<int> matrixInverse(List<int> key)
        {
            List<int> inverse = new List<int>();
            int mDim = (int)matrixDimentions(key.Count);
            int[,] matrix = new int [mDim, mDim];
            int[,] matrixInv = new int[mDim, mDim];
            int[,] matrixTranc = new int[mDim, mDim];

            double determinand = matrixDeterminand(key);
            double b = multiplicativeInverse(determinand);
            transformToMatrix(key, matrix);


            for (int i = 0; i < mDim; i++)
            {
                for (int j = 0; j < mDim; j++)
                {
                    matrixTranc[i, j] = ((int)b * (int)Math.Pow(-1, i + j) 
                        * elementDeterminand(i, j, matrix, mDim)) % 26;
                }
            }

            for (int a = 0; a < mDim; a++)
            {
                for (int c = 0; c < mDim; c++)
                {
                    matrixInv[a, c] = matrixTranc[c, a];
                }
            }


            for (int d = 0; d < mDim; d++)
            {
                for (int e = 0; e < mDim; e++)
                {
                    inverse.Add(matrixInv[d, e]);
                }
            }

            return inverse;
        }
    }
}
