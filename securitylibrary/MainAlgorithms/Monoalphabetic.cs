using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        private string alpha = "abcdefghijklmnopqrstuvwxyz";
        public string Analyse(string plainText, string cipherText)
        {
            string output = "";
            string lowerCase = cipherText.ToLower();
            foreach (char c in alpha)
            {
                if (plainText.IndexOf(c) != -1)
                {
                    output += lowerCase[plainText.IndexOf(c)];
                }
                else
                {
                    output += '0';
                }
            }
            if (output.IndexOf('0') != -1)
            {
                foreach (char c in alpha)
                {
                    if (output.IndexOf(c) == -1)
                    {
                        if (output.IndexOf('0') != -1)
                        {
                            int i = output.IndexOf('0');
                            output = output.Substring(0, i) +
                                                    c +
                                                    output.Substring(i + 1);
                        }
                        else
                            return output;
                    }
                }
            }
            return output;
        }

        public string Decrypt(string cipherText, string key)
        {
            string output = "";
            string lowerCase = cipherText.ToLower();
            foreach (char c in lowerCase)
            {
                output += alpha[key.IndexOf(c)];
            }
            return output;
        }

        public string Encrypt(string plainText, string key)
        {
            string output = "";
            foreach (char c in plainText)
            {
                output += key[alpha.IndexOf(c)];
            }
            return output.ToUpper();
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
            Dictionary<char, double> map = new Dictionary<char, double>();
            foreach (char c in cipher)
            {
                if (!map.ContainsKey(c))
                {
                    map.Add(c, 1);
                }
                else
                {
                    map[c]++;
                }
            }
            double totalCharacters = cipher.Length;
            foreach (var key in map.Keys.ToList())
            {
                map[key] /= totalCharacters;
            }
            var sortedDict = from entry in map orderby entry.Value descending select entry;
            string output = "";
            Dictionary<char, char> decryptionMap = new Dictionary<char, char>();
            string expectedFrequencyCharacters = "ETAOINSRHLDCUMFPGWYBVKXJQZ";
            int i = 0;
            foreach (var e in sortedDict)
            {
                decryptionMap[e.Key] = expectedFrequencyCharacters[i];
                ++i;
            }
            foreach (char c in cipher)
            {
                if (decryptionMap.ContainsKey(c))
                {
                    output += decryptionMap[c];
                }
                else
                {
                    output += c;
                }
            }
            return output.ToLower();
        }
    }
}