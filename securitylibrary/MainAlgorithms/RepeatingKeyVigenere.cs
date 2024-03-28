using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            string NKey = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                int idx = alpha.IndexOf(cipherText[i]) - alpha.IndexOf(plainText[i]);
                if (idx < 0) idx += 26;
                idx = idx % 26;
                NKey += alpha[idx];
            }
            string temp = "";
            // Brute force over all combinations.
            for (int i = 0; i < NKey.Length; i++)
            {
                temp += NKey[i];
                if (plainText.Equals(Decrypt(cipherText, temp).ToLower()))
                {

                    return temp;
                }
            }
            return NKey;


        }


        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            string Nkey = "";
            while (cipherText.Length > Nkey.Length)
            {
                Nkey += key;
            }
            string plain = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                int idx = alpha.IndexOf(cipherText[i]) - alpha.IndexOf(Nkey[i]);
                if (idx < 0) idx += 26;
                idx = idx % 26;
                plain += alpha[idx];
            }
           
            return plain.ToUpper(); 
        }

        public string Encrypt(string plainText, string key)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            string Nkey = "";
            while (plainText.Length > Nkey.Length)
            {
                Nkey += key;
            }
            string cipher = "";
            for(int i=0;i<plainText.Length;i++)
            {
                int idx = alpha.IndexOf(plainText[i]) + alpha.IndexOf(Nkey[i]);
                idx = idx % 26;
                cipher += alpha[idx];
            }
            return cipher;
        }
    }
}

