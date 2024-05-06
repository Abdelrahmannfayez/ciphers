using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SecurityLibrary.DiffieHellman
{


    public class DiffieHellman
    {

      public  int power(int f, int s, int sf)
        {
            if (s == 0)
                return 1;
            else if (s % 2 == 0)
            {
                int half = power(f, s / 2, sf);
                return (half * half) % sf;
            }
            else
            {
                return (f * power(f, s - 1, sf)) % sf;
            }
        }

        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            int ya = power(alpha, xa, q);
            int yb = power(alpha, xb, q);

            int ka = power(yb, xa, q);
            int kb = power(ya, xb, q);

            return new List<int> { ka, kb };
        }
    }
}