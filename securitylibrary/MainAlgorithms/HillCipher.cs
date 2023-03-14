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
            List<int> keyInv = matrixInverse(key);

            double keyDimentions = matrixDimentions(key.Count);

            int halfResult, result = 0, counter = 0;

            for (int i = 0; i < cipherText.Count; i++)
            {
                counter++;
                halfResult = cipherText[i] * keyInv[counter - 1];
                result += halfResult;

                if ((counter) % keyDimentions == 0)
                {
                    plainText.Add(result % 26);
                    result = 0;

                    if ((counter) % keyInv.Count != 0)
                    {
                        i -= int.Parse(keyDimentions.ToString());
                    }
                }

                if ((counter) % keyInv.Count == 0)
                {
                    counter = 0;
                }
            }

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
                determinand = (key[0] * key[2] - key[1] * key[3]);
            }
            else
            {
                determinand = (key[0] * (key[4] * key[8] - key[5] * key[7])
                    - key[1] * (key[3] * key[8] - key[5] * key[6])
                    + key[2] * (key[3] * key[7] - key[4] * key[6]));
            }

            if (determinand < 0)
            {
                for (; determinand < 0; determinand += 26) ;
            }
            else
            {
                determinand = determinand % 26;
            }

            return determinand;
        }

        public double multiplicativeInverse(double determinand, int mDim)
        {

            double b, c;

            int i;

            if (mDim > 2)
            {
                for (i = 1; (i / (26 - determinand)) != (int)(i / (26 - determinand)); i += 26) ;
                
                c = i / (26 - determinand);
                b = 26 - c;
            }
            else
            {
                for (i = 1; (i / (26 - determinand)) != (int)(i / (26 - determinand)); i ++) ;

                c = i / (26 - determinand);
                b = 26 - c;
            }
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
            List<int> dMatrix = new List<int>();
            for (int i = 0; i < mDim; i++)
            {
                for (int j = 0; j < mDim; j++)
                {
                    if(i != row && j != col)
                    {
                        dMatrix.Add(matrix[i, j]);
                    }
                }
            }

            determinand = (dMatrix[0] * dMatrix[3] - dMatrix[1] * dMatrix[2]);

            if (determinand < 0)
            {
                for (; determinand < 0; determinand += 26) ;
            }
            else
            {
                determinand = determinand % 26;
            }

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
            transformToMatrix(key, matrix);
            double b = multiplicativeInverse(determinand, mDim);

            if (mDim > 2)
            {
                for (int i = 0; i < mDim; i++)
                {
                    for (int j = 0; j < mDim; j++)
                    {
                        matrixTranc[i, j] = ((int)b * (int)Math.Pow(-1, i + j)
                            * elementDeterminand(i, j, matrix, mDim));

                        if (matrixTranc[i, j] < 0)
                        {
                            for (; matrixTranc[i, j] < 0; matrixTranc[i, j] += 26) ;
                        }
                        else
                        {
                            matrixTranc[i, j] = matrixTranc[i, j] % 26;
                        }
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
            }
            else
            {
                matrixInv[0, 0] = matrix[1, 1];
                matrixInv[1, 1] = matrix[0, 0];
                matrixInv[0, 1] = -matrix[0, 1];
                matrixInv[1, 0] = -matrix[1, 0];

                

                for (int i = 0; i < mDim; i++)
                {
                    for (int j = 0; j < mDim; j++)
                    {
                        matrixTranc[i, j] = matrixInv[i, j] * (int)b;

                        if (matrixTranc[i, j] < 0)
                        {
                            for (; matrixTranc[i, j] < 0; matrixTranc[i, j] += 26) ;
                        }
                        else
                        {
                            matrixTranc[i, j] = matrixTranc[i, j] % 26;
                        }

                        inverse.Add(matrixTranc[i, j]);
                    }
                }
            }
            

            return inverse;
        }
    }
}
