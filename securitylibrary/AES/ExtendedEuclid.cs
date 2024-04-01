using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            if (AreRelativelyPrime(number, baseN))
            {
                long number1 = (long)number;
                int  i = 1;
                while (true)
                {
                    if ((number1 * i) % baseN == 1)
                        return i;
                    else
                        i++;
                }
            }
            else { return -1; }
        }

        public bool AreRelativelyPrime(int a, int b)
        {
            if (a<b)
            for (int i = 2; i <= a; i++)
            {
                // If both numbers are divisible by i, they are not relatively prime
                if (a % i == 0 && b % i == 0)
                {
                    return false;
                }
            }
            else
                for (int i = 2; i <=  b; i++)
                {
                    // If both numbers are divisible by i, they are not relatively prime
                    if (a % i == 0 && b % i == 0)
                    {
                        return false;
                    }
                }

            // If no common divisor found, they are relatively prime
            return true;
        }

    }
}
