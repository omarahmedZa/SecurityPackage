using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            // throw new NotImplementedException();

            // Calculate the values of C1 and C2
            long C1 = Modulo_Exponentiation(alpha, k, q);
            long C2 = (m * Modulo_Exponentiation(y, k, q)) % q;

            // Create a list to store the values of C1 and C2
            List<long> result_Cipher_Text = new List<long>();
            result_Cipher_Text.Add(C1);
            result_Cipher_Text.Add(C2);

            return result_Cipher_Text;

        }
        public int Decrypt(int c1, int c2, int x, int q)
        {
            //throw new NotImplementedException();
            // Calculate the value of the shared key
            long Ephemeral_Key_KE = Modulo_Exponentiation(c1, x, q);

            // Calculate the inverse of the shared secret
            long inverse = ModuloInverse__Extended_Euclidean_Algorithm(Ephemeral_Key_KE, q);

            // Calculate the decrypted message
            int M = (int)((c2 * inverse) % q);

            return M;
        }
        private long Modulo_Exponentiation(long baseValue, long exponent, long modulo)
        {
            long res = 1;

            while (exponent > 0)
            {
                if (exponent % 2 == 1)
                {
                    res = (res * baseValue) % modulo;
                }

                baseValue = (baseValue * baseValue) % modulo;
                exponent = exponent / 2;
            }

            return res;
        }
        private long ModuloInverse__Extended_Euclidean_Algorithm(long number, long modulo)
        {
            long a = modulo;
            long b = number % modulo;
            long x0 = 0;
            long x1 = 1;

            while (b > 1)
            {
                long q = b / a;
                long tmp = a;
                a = b % a;
                b = tmp;
                tmp = x0;
                x0 = x1 - q * x0;
                x1 = tmp;
            }

            if (x1 < 0)
            {
                x1 = x1 + modulo;
            }

            return x1;
        }
    }
}
