using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{

    public class DES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
            int[,] PC1 = Constants.PC1;


            key = ConvertHexNumberToBinary(key);
            string k1 = PerformKeyPermutaion(key, PC1);

            List<string> keys = GenerateKeys(k1);

            string binaryEncryptedText = PerformEncryption(plainText, keys);

            string resultedCipherText = ConvertBinaryToHex(binaryEncryptedText);

            resultedCipherText = "0x" + resultedCipherText;

            return resultedCipherText;
        }

        private string HexDigitToBinary(string hexDigit)
        {
            int decimalValue = Convert.ToInt32(hexDigit, 16);
            string binary = Convert.ToString(decimalValue, 2);
            binary = binary.PadLeft(4, '0');

            return binary;
        }

        private string ConvertHexNumberToBinary(string hexNumber)
        {
            string keyInBits = "";

            for (int i = 2; i < hexNumber.Length; i++)
            {
                keyInBits += HexDigitToBinary(hexNumber[i].ToString());
            }

            return keyInBits;
        }

        private string PerformKeyPermutaion(string key, int[,] permutationMatrix)
        {
            string resultedKey = "";

            for (int i = 0; i < permutationMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < permutationMatrix.GetLength(1); j++)
                {
                    resultedKey += key[permutationMatrix[i, j] - 1];
                }
            }

            return resultedKey;
        }

        private List<string> GenerateKeys(string k1)
        {
            int[,] PC2 = Constants.PC2;

            List<string> keys = new List<string>();

            List<int> oneShift = new List<int>() { 1, 2, 9, 16 };

            string c = SplitString(k1, 0, k1.Length / 2);
            string d = SplitString(k1, k1.Length / 2, k1.Length);

            for (int i = 1; i <= 16; i++)
            {
                int shiftCount = 2;
                if (oneShift.Contains(i))
                {
                    shiftCount = 1;
                }

                c = PerformLeftCircularShift(c, shiftCount);
                d = PerformLeftCircularShift(d, shiftCount);

                string cd = c + d;

                string k = PerformKeyPermutaion(cd, PC2);

                keys.Add(k);
            }

            return keys;
        }

        private string PerformLeftCircularShift(string binaryString, int shiftValue)
        {
            LinkedList<char> binaryList = new LinkedList<char>(binaryString);

            for (int i = 0; i < shiftValue; i++)
            {
                char firstChar = binaryList.First.Value;
                binaryList.RemoveFirst();
                binaryList.AddLast(firstChar);
            }

            return string.Join("", binaryList);
        }

        private string SplitString(string k1, int start, int end)
        {
            string temp = "";
            for (int i = start; i < end; i++)
            {
                temp += k1[i];
            }

            return temp;
        }

        private string PerformEncryption(string mainPlain, List<string> keys)
        {
            mainPlain = ConvertHexNumberToBinary(mainPlain);
            mainPlain = PerformKeyPermutaion(mainPlain, Constants.initialPermutationMatrix);

            string L = SplitString(mainPlain, 0, mainPlain.Length / 2);
            string R = SplitString(mainPlain, mainPlain.Length / 2, mainPlain.Length);

            for (int i = 1; i <= 16; i++)
            {
                string Ltemp = L;
                L = R;
                R = XOR(Ltemp, F(R, keys[i - 1]));
            }

            string binaryEncryptedText = R + L;

            binaryEncryptedText = PerformKeyPermutaion(binaryEncryptedText, Constants.FinalPermutationTable);

            return binaryEncryptedText;
        }

        private string XOR(string binary1, string binary2)
        {
            char[] resultArray = new char[binary1.Length];
            for (int i = 0; i < binary1.Length; i++)
            {
                resultArray[i] = binary1[i] == binary2[i] ? '0' : '1';
            }

            return new string(resultArray);
        }

        private string F(string R, string K)
        {
            string E = PerformKeyPermutaion(R, Constants.eBitSelectionTable);
            string result = XOR(E, K);

            List<string> resulted_6_Bits = GenerateSboxList(result);
            string SB = "";
            for (int i = 0; i < resulted_6_Bits.Count; i++)
            {
                string row = "";
                string col = "";

                for (int j = 0; j < resulted_6_Bits[i].Length; j++)
                {
                    if (j == 0 || j == resulted_6_Bits[i].Length - 1)
                    {
                        row += resulted_6_Bits[i][j];
                    }
                    else
                    {
                        col += resulted_6_Bits[i][j];
                    }
                }

                int row_num = ConvertFromBinaryToDecimal(row);
                int col_num = ConvertFromBinaryToDecimal(col);

                int val = Constants.sBoxes[i][row_num, col_num];

                SB += ConvertFromDecimalToBinary(val);
            }

            SB = PerformKeyPermutaion(SB, Constants.PBoxPermutation);

            return SB;
        }

        private int ConvertFromBinaryToDecimal(string binary)
        {
            int decimalNumber = 0;

            for (int i = binary.Length - 1; i >= 0; i--)
            {
                if (binary[i] == '1')
                {
                    int power = binary.Length - 1 - i;
                    decimalNumber += (int)Math.Pow(2, power);
                }
            }

            return decimalNumber;
        }

        private string ConvertFromDecimalToBinary(int decimalNumber)
        {
            string binary = Convert.ToString(decimalNumber, 2);
            binary = binary.PadLeft(4, '0');

            return binary;
        }

        private List<string> GenerateSboxList(string result)
        {
            List<string> resulted_6_Bits = new List<string>();

            string accumulator = "";
            for (int i = 0; i < result.Length; i++)
            {
                if (i != 0 && i % 6 == 0)
                {
                    resulted_6_Bits.Add(accumulator);
                    accumulator = "";
                }

                accumulator += result[i];
            }

            resulted_6_Bits.Add(accumulator);

            return resulted_6_Bits;
        }

        private string ConvertBinaryToHex(string binary)
        {
            string hex = "";
            string temp = "";
            for (int i = 0; i < binary.Length; i++)
            {
                if (i != 0 && i % 4 == 0)
                {
                    hex += ConvertFourBitsToHex(temp);
                    temp = "";
                }

                temp += binary[i];
            }
            hex += ConvertFourBitsToHex(temp);

            return hex;
        }

        private char ConvertFourBitsToHex(string binary)
        {
            int decimalValue = Convert.ToInt32(binary, 2);
            char hexValue = decimalValue < 10 ? (char)(decimalValue + '0') : (char)(decimalValue - 10 + 'A');

            return hexValue;
        }
    }
}
