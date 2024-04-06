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
            string[,] cipher = ConvertStringTo2DArray(cipherText);
            string[,] keyMatrix = ConvertStringTo2DArray(key);
            List<string[,]> roundKeys = GenerateRoundKeys(keyMatrix);

            string[,] result = AddRoundKeys(cipher, roundKeys[9]);
            result = InvShiftRows(result);
            result = InvSubBytes(result);

            for (int i = 8; i >= 0; i--)
            {
                result = AddRoundKeys(result, roundKeys[i]);
                result = InvMixColumns(result);
                result = InvShiftRows(result);
                result = InvSubBytes(result);

            }

            result = AddRoundKeys(result, keyMatrix);
            string plainText = "0x";

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    plainText += result[j, i].Replace("0x", "");
                }
            }

            return plainText;
        }

        public override string Encrypt(string plainText, string key)
        {
            string[,] plain = ConvertStringTo2DArray(plainText);
            string[,] keyy = ConvertStringTo2DArray(key);
            string[,] result = AddRoundKeys(plain, keyy);

            for (int i = 0; i < 10; i++)
            {
                result = SubBytes(result);
                result = ShiftRows(result);

                if (i != 9)
                {
                    result = MixColumns(result);
                }

                keyy = UpdateKey(keyy, i);
                result = AddRoundKeys(result, keyy);
            }

            string cipherText = "0x";

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    cipherText += result[j, i].Replace("0x", "");
                }
            }

            return cipherText;
        }


        private string[,] InvSubBytes(string[,] state)
        {

            string[,] output = new string[4, 4];
            Dictionary<char, int> map = new Dictionary<char, int>();
            map['0'] = 0; map['1'] = 1; map['2'] = 2; map['3'] = 3; map['4'] = 4; map['5'] = 5; map['6'] = 6; map['7'] = 7;
            map['8'] = 8; map['9'] = 9; map['a'] = 10; map['b'] = 11; map['c'] = 12; map['d'] = 13; map['e'] = 14;
            map['f'] = 15; map['A'] = 10; map['B'] = 11; map['C'] = 12; map['D'] = 13; map['E'] = 14; map['F'] = 15;

            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    string temp = state[i, j].Replace("0x", "");
                    int rowIndex = map[temp[0]];
                    int colIndex = map[temp[1]];
                    output[i, j] = "0x" + Constants.InvSbox[rowIndex, colIndex];
                }
            }

            return output;
        }


        private string[,] InvShiftRows(string[,] state)
        {
            for (int i = 1; i < 4; i++)
            {
                LinkedList<string> rowList = new LinkedList<string>();


                for (int j = 0; j < 4; j++)
                {
                    rowList.AddLast(state[i, j]);
                }


                for (int k = 0; k < i; k++)
                {
                    string lastElement = rowList.Last.Value;
                    rowList.RemoveLast();
                    rowList.AddFirst(lastElement);
                }


                int index = 0;
                foreach (string element in rowList)
                {
                    state[i, index++] = element;
                }
            }



            return state;
        }



        private string[,] InvMixColumns(string[,] state)
        {
            string[,] output = new string[4, 4];


            for (int c = 0; c < 4; c++)
            {
                for (int r = 0; r < 4; r++)
                {
                    output[r, c] = "0x00";

                    for (int i = 0; i < 4; i++)
                    {
                        output[r, c] = XOR(output[r, c], MultiplyHexNumbers(Constants.InvMixColumnsMatrix[r, i], state[i, c]));
                    }
                }
            }

            return output;
        }


        private List<string[,]> GenerateRoundKeys(string[,] key)
        {

            List<string[,]> roundKeys = new List<string[,]>();

            string[,] resultKey = UpdateKey(key, 0);
            roundKeys.Add(resultKey);
            for (int i = 1; i < 10; i++)
            {
                resultKey = UpdateKey(resultKey, i);
                roundKeys.Add(resultKey);

            }

            return roundKeys;
        }

        private string[,] ConvertStringTo2DArray(string hexText) {

            string[,] array2D = new string[4, 4];
            hexText = hexText.Replace("0x", "");
            for (int i = 0; i < hexText.Length; i += 2)
            {
                int row = i / 8; 
                int column = (i % 8) / 2; 
                array2D[column, row] = "0x" + hexText.Substring(i, 2);
            }

            return array2D;
        }
        private string[,] SubBytes(string[,] state)
        {

            string[,] output = new string[4, 4];
            Dictionary<char, int> map = new Dictionary<char, int>();
            map['0'] = 0; map['1'] = 1; map['2'] = 2; map['3'] = 3; map['4'] = 4; map['5'] = 5; map['6'] = 6; map['7'] = 7;
            map['8'] = 8; map['9'] = 9; map['a'] = 10; map['b'] = 11; map['c'] = 12; map['d'] = 13; map['e'] = 14;
            map['f'] = 15; map['A'] = 10; map['B'] = 11; map['C'] = 12; map['D'] = 13; map['E'] = 14; map['F'] = 15;

            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    string temp = state[i, j].Replace("0x", "");
                    int rowIndex = map[temp[0]];
                    int colIndex = map[temp[1]];
                    output[i, j] = "0x" + Constants.S_Box[rowIndex, colIndex];
                }
            }

            return output;
        }

        private string[] keySubBytes(LinkedList<string> key)
        {

            string[] output = new string[4];
            Dictionary<char, int> map = new Dictionary<char, int>();
            map['0'] = 0; map['1'] = 1; map['2'] = 2; map['3'] = 3; map['4'] = 4; map['5'] = 5; map['6'] = 6; map['7'] = 7;
            map['8'] = 8; map['9'] = 9; map['a'] = 10; map['b'] = 11; map['c'] = 12; map['d'] = 13; map['e'] = 14;
            map['f'] = 15; map['A'] = 10; map['B'] = 11; map['C'] = 12; map['D'] = 13; map['E'] = 14; map['F'] = 15;



            LinkedListNode<string> currentNode = key.First;
            int j = 0;

            while (currentNode != null)
            {
                string temp = currentNode.Value.Replace("0x", "");
                int rowIndex = map[temp[0]];
                int colIndex = map[temp[1]];
                output[j] = "0x" + Constants.S_Box[rowIndex, colIndex];
                j++;
                currentNode = currentNode.Next;
            }


            return output;
        }

        private string[,] ShiftRows(string[,] state)
        {
            for (int i = 1; i < 4; i++)
            {
                LinkedList<string> rowList = new LinkedList<string>();

                
                for (int j = 0; j < 4; j++)
                {
                    rowList.AddLast(state[i, j]);
                }

                
                for (int k = 0; k < i; k++)
                {
                    string firstElement = rowList.First.Value;
                    rowList.RemoveFirst();
                    rowList.AddLast(firstElement);
                }

                
                int index = 0;
                foreach (string element in rowList)
                {
                    state[i, index++] = element;
                }
            }

            return state;
        }

        private string XOR(string state, string key) {           
            int num1 = Convert.ToInt32(state, 16);
            int num2 = Convert.ToInt32(key, 16);
            
            int result = num1 ^ num2;
            
            string resultHex = "0x" + result.ToString("X2");

            return resultHex;
        }

        private string[,] AddRoundKeys(string[,] state, string[,] key) {

            string[,] result = new string[4, 4];

            for (int i = 0; i < 4; i++) 
            {
                for (int j = 0; j < 4; j++) 
                {
                    result[i, j] = XOR(state[i, j], key[i, j]);
                }
            }

            return result;
        }
        

       private string[,] MixColumns(string[,] state)
        {
            string[,] output = new string[4, 4];
           

            for (int c = 0; c < 4; c++)
            {
                for (int r = 0; r < 4; r++)
                {
                    output[r, c] = "0x00"; 

                    for (int i = 0; i < 4; i++)
                    {
                        output[r, c] = XOR(output[r, c], MultiplyHexNumbers(Constants.mixColumnsMatrix[r, i], state[i, c]));
                    }
                }
            }

            return output;
        }


        private string MultiplyHexNumbers(string num1, string num2)
        {
            int a = Convert.ToInt32(num1.Substring(2), 16);
            int b = Convert.ToInt32(num2.Substring(2), 16);

            int result = 0;
            while (b != 0) 
            {
                if ((b & 0x01) != 0)
                {
                    result ^= a;
                }
                int highBit = a & 0x80;
                a <<= 1;
                if (highBit != 0)
                {
                    a ^= 0x11B; 
                }
                b >>= 1;
            }

            string resultHex = "0x" + result.ToString("X2");
            return resultHex;
        }

        private string[,] UpdateKey(string[,] key, int round)
        {
            LinkedList<string> columnList = new LinkedList<string>();
            string[,] newKey = new string[4, 4];

            // add last column to the list
            for (int i = 0; i < 4; i++) 
            {
                columnList.AddLast(key[i, 3]);
            }

            // perform shifting
            string temp = columnList.First.Value;
            columnList.RemoveFirst();
            columnList.AddLast(temp);

            // perform sub-bytes
            string[] subKey = keySubBytes(columnList);

            //calculate first column
            for (int i = 0; i < 4; i++)
            {
                newKey[i, 0] = XOR(key[i, 0], subKey[i]);
                newKey[i, 0] = XOR(newKey[i, 0], Constants.Rcon[i, round]);
            }

            for (int i = 1; i < 4; i++) 
            {
                for (int j = 0; j < 4; j++)
                {
                    newKey[j, i] = XOR(newKey[j, i - 1], key[j, i]);
                }
            }

            return newKey;
        }

        

    }
}
