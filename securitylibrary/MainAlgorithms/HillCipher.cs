using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{

    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {

            throw new NotImplementedException();
        }
        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }
        private int getCofactor(int[,] matrix, int i, int j)
        {
            int[,] result = new int[2, 2];

            int row = 0, col = 0;

            for (int ii = 0; ii < matrix.GetLength(0); ii++)
            {
                if (ii == i) continue;
                col = 0;
                for (int jj = 0; jj < matrix.GetLength(1); jj++)
                {
                    if (jj == j) continue;
                    result[row, col] = matrix[jj, ii];
                    col++;
                }
                row++;
            }


            int cofactor = (result[1, 1] * result[0, 0]) - (result[0, 1] * result[1, 0]);

            if ((i + j) % 2 == 1) cofactor *= -1;


            return cofactor;
        }

        private int calculateMatrixDeterminant(int[,] matrix)
        {
            int determinant;
            if (matrix.GetLength(0) == 3)
            {
                determinant = matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1])
                                - matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0])
                                + matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);
            }
            else
            {
                determinant = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            determinant = determinant % 26;
            if (determinant < 0) determinant += 26;
            return determinant;
        }

        private int extendedEuclidean(int a, int b, out int x, out int y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            int x1, y1;
            int gcd = extendedEuclidean(b % a, a, out x1, out y1);

            x = y1 - (b / a) * x1;
            y = x1;

            return gcd;
        }

        private int modularInverse(int a, int m)
        {
            int x, y;
            int gcd = extendedEuclidean(a, m, out x, out y);

            if (gcd != 1)
            {
                throw new Exception("Modular inverse does not exist.");
            }

            x = (x % m + m) % m;
            return x;
        }

        private int[,] calculateMatrixInverse(int[,] keyMatrix)
        {
            int[,] matrixInverse = new int[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrixInverse[i, j] = getCofactor(keyMatrix, i, j);
                    matrixInverse[i, j] = matrixInverse[i, j] % 26;
                    if (matrixInverse[i, j] < 0) matrixInverse[i, j] += 26;
                }
            }

            int determinanate = calculateMatrixDeterminant(keyMatrix);

            int modInverse = modularInverse(determinanate, 26);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrixInverse[i, j] = (matrixInverse[i, j] * modInverse) % 26;
                }
            }

            return matrixInverse;
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int[,] keyMatrix = getKeyMatrix(key);

            int[,] matrixInverse = calculateMatrixInverse(keyMatrix);

            List<List<int>> cipher = getPlainText(cipherText, keyMatrix.GetLength(0));

            List<List<int>> plainText = new List<List<int>>();

            foreach (List<int> sublist in cipher)
            {
                List<int> resultedPlain = multiplySubListByKeyMatrix(sublist, matrixInverse);
                plainText.Add(resultedPlain);
            }


            List<int> flattenedPlainText = new List<int>();
            foreach (List<int> sublist in plainText)
            {
                flattenedPlainText.AddRange(sublist);
            }

            return flattenedPlainText;
        }


        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int[,] keyMatrix = getKeyMatrix(key);
            List<List<int>> plain = getPlainText(plainText, keyMatrix.GetLength(0));

            List<List<int>> cipherText = new List<List<int>>();

            foreach (List<int> sublist in plain)
            {
                List<int> resultedCipher = multiplySubListByKeyMatrix(sublist, keyMatrix);
                cipherText.Add(resultedCipher);
            }

            List<int> flattenedCipherText = new List<int>();
            foreach (List<int> sublist in cipherText)
            {
                flattenedCipherText.AddRange(sublist);
            }

            return flattenedCipherText;
        }
        public string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {

            throw new NotImplementedException();
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }

        private List<int> multiplySubListByKeyMatrix(List<int> sublist, int[,] keyMatrix)
        {
            int rows = keyMatrix.GetLength(0);
            int columns = keyMatrix.GetLength(1);
            List<int> result = new List<int>();

            for (int i = 0; i < rows; i++)
            {
                int sum = 0;
                for (int j = 0; j < columns; j++)
                {
                    sum += sublist[j] * keyMatrix[i, j];
                }
                result.Add(sum % 26);
            }

            return result;
        }


        private List<List<int>> getPlainText(List<int> plainText, int size)
        {
            List<List<int>> result = new List<List<int>>();

            for (int i = 0; i < plainText.Count; i += size)
            {
                List<int> sublist = new List<int>();

                for (int j = i; j < Math.Min(i + size, plainText.Count); j++)
                {
                    sublist.Add(plainText[j]);
                }

                while (sublist.Count < size)
                {
                    sublist.Add((int)'x');
                }

                result.Add(sublist);
            }

            return result;
        }

        private int[,] getKeyMatrix(List<int> key)
        {
            int matrixDimensions = (int)Math.Sqrt(key.Count);

            int[,] keyMatrix = new int[matrixDimensions, matrixDimensions];

            for (int i = 0; i < matrixDimensions; i++)
            {
                for (int j = 0; j < matrixDimensions; j++)
                {
                    int index = i * matrixDimensions + j;
                    keyMatrix[i, j] = key[index];
                }
            }

            return keyMatrix;
        }



    }
}
 