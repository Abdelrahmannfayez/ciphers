using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            cipherText = cipherText.Replace("x", "");
            if (plainText == cipherText)
                return 0;

            int key = 1;
            string result = Encrypt(plainText, key);
            while (result != cipherText)
            {
                ++key;
                result = Encrypt(plainText, key);
            }
            return key;
        }

        public string Decrypt(string cipherText, int key)
        {
            string output = "";
            int length = cipherText.Length;
            int rem = length % key;
            int columns = (length / key) + rem;
            int index = 0;
            while (index < columns)
            {
                for (int j = index; j < length; j += columns)
                {
                    output += cipherText[j];
                }
                ++index;
            }
            return output;
        }

        public string Encrypt(string plainText, int key)
        {
            string output = "";
            int length = plainText.Length;
            for (int i = 0; i < key; ++i)
            {
                for (int j = i; j < length; j += key)
                {
                    output += plainText[j];
                }
            }
            return output;
        }
    }
}
