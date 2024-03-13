using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using SecurityLibrary;
using System.Collections.Generic;

namespace SecurityLibrary.MainAlgorithms
{
    internal class totest
    {
        List<int> plain4 = new List<int> { 5, 21, 2, 5, 2, 16, 19, 14, 1 };
        List<int> cipher4 = new List<int> { 7, 6, 17, 25, 4, 20, 3, 21, 16 };
        List<int> key4 = new List<int> { 1, 10, 0, 0, 20, 1, 2, 15, 2 };


        public void HillCipherTestEnc6()
        {
            HillCipher algorithm = new HillCipher();

            List<int> key2 = algorithm.Analyse3By3Key(plain4, cipher4);
            for (int i = 0; i < key2.Count; i++)
            {
                Console.Write(key2[i] + " ");
            }
            Console.WriteLine();

            for (int i = 0; i < key4.Count; i++)
            {
                Console.Write(key4[i] + " ");
            }
        }


    }
}
