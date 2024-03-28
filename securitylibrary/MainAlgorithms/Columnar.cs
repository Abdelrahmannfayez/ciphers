using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            List<int> key = new List<int>();
            int flag = 0;
            for (int k = 1; k < plainText.Length; k++)
            {
                int[] arr = new int[k];
                for (int i = 0; i < k; i++)
                {
                    arr[i] = i + 1;
                }
                List<int[]> allpos = new List<int[]>();
                getallsol(arr, 0, allpos);
                foreach (var oparr in allpos)
                {
                    foreach (int num in oparr)
                    {
                        key.Add(num);
                    }
                    string res = Encrypt(plainText, key);
                    if (res != cipherText)
                    {
                        key = new List<int>();
                    }
                    else
                    {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1) { break; }
            }

            void getallsol(int[] arr2, int start, List<int[]> allpos)
            {
                if (start == arr2.Length - 1)
                {
                    allpos.Add((int[])arr2.Clone());
                    return;
                }

                for (int i = start; i < arr2.Length; i++)
                {
                    Swap(arr2, start, i);
                    getallsol(arr2, start + 1, allpos);
                    Swap(arr2, start, i);
                }
            }
            void Swap(int[] arr1, int i, int j)
            {
                int temp = arr1[i];
                arr1[i] = arr1[j];
                arr1[j] = temp;
            }
            return key;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            string output = "";
            int col = key.Count();
            int len = cipherText.Length;
            float nr = ((float)(len) / (float)(col));
            int y = len / col;
            int row = y;
            if (nr > y) { row = row + 1; }
            int mod = (row * col) - len;
            int index = 0;
            char[,] arr = new char[row, col];
            for (int it = 0; it < key.Count; it += 1)
            {
                int c = key.IndexOf(it + 1);
                int exist = (col - (c + 1));
                if (exist < mod)
                {
                    for (int r = 0; r < row - 1; r += 1)
                    {
                        arr[r, c] = cipherText[index];
                        ++index;
                    }
                }
                else
                {
                    for (int r = 0; r < row; r += 1)
                    {
                        arr[r, c] = cipherText[index];
                        ++index;
                    }
                }
            }
            for (int i = 0; i < row; i += 1)
            {
                for (int j = 0; j < col; j += 1)
                {
                    if ((arr[i, j].Equals(""))) { continue; }
                    else { output = output + arr[i, j]; }
                }
            }
            return output;

        }

        public string Encrypt(string plainText, List<int> key)
        {
            string output = "";
            int col = key.Count();
            int len = plainText.Length;
            float r = ((float)(len) / (float)(col));
            int y = len / col;
            int row = y;
            if (r > y) { row = row + 1; }
            int mod = (row * col) - len;
            for (int i = 1; i <= col; i++)
            {
                int c = key.IndexOf(i);
                int exist = (col - (c + 1));
                if (exist < mod)
                {
                    int it = 0;
                    for (int j = c; j < len; j += col)
                    {
                        output += plainText[j];
                        it++;
                        if (it == row)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    for (int j = c; j < len; j += col)
                    {
                        output += plainText[j];
                    }
                }
            }
            return output;         
        }

    }
}
