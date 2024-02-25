using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
           
        }

        public string Decrypt(string cipherText, string key)
        {
            
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            cipherText = cipherText.ToLower();
            string plain = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                //get the letter index in key
                int idx = key.IndexOf(cipherText[i]);
                plain += alpha[idx];
                
            }
            return plain.ToLower();
        }

        public string Encrypt(string plainText, string key)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            string cipher = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                //get the letter index alpha
                int idx = alpha.IndexOf(plainText[i]);
                cipher += key[idx];
            }
            return cipher;
          
        }







        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	=
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        /// 

        public string AnalyseUsingCharFrequency(string cipher)
        {

            throw new NotImplementedException();


        }
    }
}