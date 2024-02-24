using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {

        public string Encrypt(string plainText, int key)
        {
            string cipher = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                int ci = (plainText[i] + key);

                if (plainText[i] >= 'a' && plainText[i] <= 'z')
                {

                    ci %= (26 + 'a');
                    if (ci != plainText[i] + key)
                        ci += 'a';
                    char current_cipher = (char)ci;
                    cipher += current_cipher;
                }
                else if (plainText[i] >= 'A' && plainText[i] <= 'Z')
                {


                    ci %= (26 + 'A');
                    if (ci != plainText[i] + key)
                        ci += 'A';
                    char current_cipherr = (char)ci;
                    cipher += current_cipherr;

                }
            }
            return cipher;
        }

        public string Decrypt(string cipherText, int key)
        {
            string plain = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                int ci = (cipherText[i] - key);

                if (cipherText[i] >= 'a' && cipherText[i] <= 'z')
                {
                    if (!(ci >= 'a' && ci <= 'z'))
                    {
                        ci = 'z' - key + (cipherText[i] - 'a' + 1);
                    }
                    char current_cipher = (char)ci;

                    plain += current_cipher;
                }
                else if (cipherText[i] >= 'A' && cipherText[i] <= 'Z')
                {

                    if (!(ci >= 'A' && ci <= 'Z'))
                    {
                        ci = 'Z' - key + (cipherText[i] - 'A' + 1);
                    }
                    char current_plain = (char)ci;

                    plain += current_plain;
                }
            }
            return plain;

        }

        public int Analyse(string plainText, string cipherText)
        {
            int key;
            plainText=plainText.ToLower();
            cipherText=cipherText.ToLower();
            for ( key = 0; key < 26; key++)
            {
                if (Encrypt(plainText, key) == cipherText)

                {
                    return key; 
                }

            }
            return key;
        }
    }
}