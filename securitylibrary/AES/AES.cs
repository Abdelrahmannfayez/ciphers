using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();
        }
        private string shiftRows(string plainText)
        {
            return shift(plainText, false, 2);
        }

        private string InvShiftRows(string shiftedText)
        {
            return shift(shiftedText, true, 6);
        }

        private string shift(string text, bool invert, int index)
        {
            string output = text.Substring(0, 10);
            int startIndex = 10;
            int endIndex = 18;
            while (endIndex <= text.Length + 1)
            {
                int countEnd = endIndex - (startIndex + index);
                output += text.Substring(startIndex + index, countEnd);
                int countShiftedChars = (startIndex + index) - startIndex;
                output += text.Substring(startIndex, countShiftedChars);
                startIndex = endIndex;
                endIndex += 8;
                if (invert)
                    index -= 2;
                else
                    index += 2;
            }
            return output;
        }
    }
}
