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
                static string Addroundkey(string plainText, string k)
        {
            string xor_res = "0x";
            string[,] state = new string[4, 4];
            string[,] cipherkey = new string[4, 4];
            string[,] output = new string[4, 4];
            if (plainText[0] == '0' && plainText[1] == 'x')
            {
                int idx = 2;
                for (int c = 0; c < 4; c++)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        int b1 = Convert.ToInt32(plainText.Substring(idx, 2), 16);
                        int b2 = Convert.ToInt32(k.Substring(idx, 2), 16);
                        int xorResult = b1 ^ b2;
                        xor_res += xorResult.ToString("X2").ToLower();
                        idx = idx + 2;
                    }
                }
            }
            return xor_res;
        }
        static string[,] xorfunction(string[,] x, string[,] y)
        {
            string[,] xor_res = new string[4, 1];
            for (int r = 0; r < 4; r++)
            {
                int b1 = Convert.ToInt32(x[r, 0], 16);
                int b2 = Convert.ToInt32(y[r, 0], 16);
                int xorResult = b1 ^ b2;
                xor_res[r, 0] = xorResult.ToString("X2").ToLower();
            }
            return xor_res;
        }
        static string[,] convString_Arr(string x)
        {
            int len = (x.Length) - 2;
            int col = len / (2 * 4);
            string[,] arr = new string[4, col];
            if (x[0] == '0' && x[1] == 'x')
            {
                int idx = 2;
                for (int c = 0; c < col; c++)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        arr[r, c] = x.Substring(idx, 2);
                        idx = idx + 2;
                    }
                }
            }
            return arr;
        }
        static string convArr_String(string[,] x)
        {
            string str = "0x";
            int row = x.GetLength(0);
            int col = x.GetLength(1);
            for (int c = 0; c < col; c++)
            {
                for (int r = 0; r < row; r++)
                {
                    str += x[r, c];
                }
            }
            return str;
        }
    }
}
