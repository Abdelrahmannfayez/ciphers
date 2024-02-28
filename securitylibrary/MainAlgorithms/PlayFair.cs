using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {

            char[,] matrix = constructMatrix(key);
            List<string> stringDiagrams = constructDiagrams(plainText);
            string encryptedString = "";

            foreach (string diagram in stringDiagrams)
            {
                int firstCipherRow;
                int firstCipherColumn;
                int secondCipherRow;
                int secondCipherColumn;
                KeyValuePair<int, int> firstLetter = getCharacterPosition(diagram[0], matrix);
                KeyValuePair<int, int> secondLetter = getCharacterPosition(diagram[1], matrix);

                if (firstLetter.Key == secondLetter.Key) // in the same row
                {
                    firstCipherRow = firstLetter.Key;
                    firstCipherColumn = (firstLetter.Value + 1) % 5;

                    secondCipherRow = secondLetter.Key;
                    secondCipherColumn = (secondLetter.Value + 1) % 5;
                }
                else if (firstLetter.Value == secondLetter.Value) // in the same column
                {
                    firstCipherRow = (firstLetter.Key + 1) % 5;
                    firstCipherColumn = firstLetter.Value;

                    secondCipherRow = (secondLetter.Key + 1) % 5;
                    secondCipherColumn = secondLetter.Value;
                }
                else
                {
                    firstCipherRow = firstLetter.Key;
                    firstCipherColumn = secondLetter.Value;

                    secondCipherRow = secondLetter.Key;
                    secondCipherColumn = firstLetter.Value;
                }
                encryptedString += matrix[firstCipherRow, firstCipherColumn].ToString() + matrix[secondCipherRow, secondCipherColumn].ToString(); ;
            }

            return encryptedString;
        }

        private char[,] constructMatrix(string key)
        {

            char[,] matrix = new char[5, 5];
            Dictionary<char, bool> keyMap = new Dictionary<char, bool>();

            for (int i = 0; i < key.Length; i++)
            {
                char currentChar = char.ToUpper(key[i]);

                if (currentChar == 'J')
                {
                    currentChar = 'I';
                }

                if (!keyMap.ContainsKey(currentChar))
                {
                    keyMap.Add(currentChar, true);
                }
            }


            for (char i = 'A'; i <= 'Z'; i++)
            {
                char currentChar = i;
                if (currentChar == 'J')
                {
                    currentChar = 'I';
                }

                if (!keyMap.ContainsKey(currentChar))
                {
                    keyMap.Add(currentChar, true);
                }


            }


            int j = 0;

            foreach (char c in keyMap.Keys)
            {
                int row = j / 5;
                int col = j % 5;
                j++;
                matrix[row, col] = c;

            }

            return matrix;
        }

        private List<string> constructDiagrams(string plainText)
        {

            List<string> stringList = new List<string>();
            LinkedList<char> linkedList = new LinkedList<char>();

            plainText = plainText.ToUpper();
            char filler = 'X';

            for (int i = 0; i < plainText.Length; i += 2)
            {
                char firstChar = plainText[i];

                char secondChar;
                if (i + 1 < plainText.Length)
                {
                    secondChar = plainText[i + 1];
                }
                else
                {
                    secondChar = filler;
                }

                if (firstChar != secondChar)
                {
                    linkedList.AddLast(firstChar);
                    linkedList.AddLast(secondChar);
                }
                else
                {
                    linkedList.AddLast(firstChar);
                    linkedList.AddLast(filler);
                    i--;
                }
            }

            string temp = "";
            foreach (var item in linkedList)
            {
                temp += item;
                if (temp.Length == 2)
                {
                    stringList.Add(temp);
                    temp = "";
                }
            }
            return stringList;
        }

        private KeyValuePair<int, int> getCharacterPosition(char c, char[,] matrix)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (matrix[i, j] == c)
                    {
                        return new KeyValuePair<int, int>(i, j);
                    }
                }
            }

            return new KeyValuePair<int, int>(0, 0);

        }

    }
}