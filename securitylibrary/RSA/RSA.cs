using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public bool AreRelativelyPrime(long a, long b)
        {
            if (a < b)
                for (int i = 2; i <= a; i++)
                {
                    // If both numbers are divisible by i, they are not relatively prime
                    if (a % i == 0 && b % i == 0)
                    {
                        return false;
                    }
                }
            else
                for (int i = 2; i <= b; i++)
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
        public long GetMultiplicativeInverse(long number, long baseN)
        {
            if (AreRelativelyPrime(number, baseN))
            {
                long number1 = (long)number;
                int i = 1;
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
        public static long fast_power(long a, long b, long c)
        {
            //base case 
            if (b == 0) return 1;
            //transition
            long R = fast_power(a, b/2, c);
            R = ((R % c) * (R % c)) % c;
            if (b % 2 == 1) return (R * (a % c)) % c;
            else return R; 
            
        }
        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q;
            int tot_n = (p - 1) * (q - 1);
            return (int)fast_power( M,e , n);
            
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            long n = p * q;
            long tot_n = (p - 1) * (q - 1);
            long d = GetMultiplicativeInverse(e, tot_n);

            return (int)fast_power(C, d, n);
        }
    }
}
