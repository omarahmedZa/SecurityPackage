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
            throw new NotImplementedException();
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
                    //Console.WriteLine(result);
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

    }
}
