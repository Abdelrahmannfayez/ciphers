using System;
using System.Collections.Generic;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="q"></param>
        /// <param name="alpha"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <param name="m"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            List<long> cipher = new List<long>();

            long c1 = ModPower(alpha, k, q);

            long c2 = (m * ModPower(y, k, q)) % q;

            cipher.Add(c1);
            cipher.Add(c2);

            return cipher;
        }

        /// <summary>
        /// Decryption
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="x"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public int Decrypt(int c1, int c2, int x, int q)
        {
            long S = ModPower(c1, x, q);

            long m = (c2 * ModInverse(S, q)) % q;

            return (int)m;
        }

        private long ModPower(long baseNum, long exponent, long modulus)
        {
            if (modulus == 1) return 0;

            long result = 1;
            baseNum = baseNum % modulus;

            while (exponent > 0)
            {
                if (exponent % 2 == 1)
                    result = (result * baseNum) % modulus;
                exponent = exponent >> 1;
                baseNum = (baseNum * baseNum) % modulus;
            }

            return result;
        }

        private long ModInverse(long a, long m)
        {
            long m0 = m;
            long y = 0, x = 1;

            if (m == 1)
                return 0;

            while (a > 1)
            {
                long q = a / m;
                long t = m;

                m = a % m;
                a = t;
                t = y;

                y = x - q * y;
                x = t;
            }

            if (x < 0)
                x += m0;

            return x;
        }
    }
}
