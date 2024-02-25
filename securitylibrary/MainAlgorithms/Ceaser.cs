using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        private string alpha = "abcdefghijklmnopqrstuvwxyz";
        public string Encrypt(string plainText, int key)
        {
            string output = "";
            foreach (char c in plainText)
            {
                int index = (alpha.IndexOf(c) + key) % 26;
                output += alpha[index];
            }
            return output.ToUpper();
        }

        public string Decrypt(string cipherText, int key)
        {
            string output = "";
            string lowerCase = cipherText.ToLower();
            foreach (char c in lowerCase)
            {
                int index = (alpha.IndexOf(c) - key + 26) % 26;
                output += alpha[index];
            }
            return output;
        }

        public int Analyse(string plainText, string cipherText)
        {
            int k = 0;
            string lowerCase = cipherText.ToLower();
            while ((alpha.IndexOf(plainText[0]) + k) % 26 != alpha.IndexOf(lowerCase[0]))
            {
                k++;
            }
            return k;
        }
    }
}