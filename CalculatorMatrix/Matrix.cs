﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MatrixCalculation
{
    public class Matrix<T> where T : struct
    {
        private T[,] _data;
        public int Rows { get; }
        public int Cols { get; }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            _data = new T[rows, cols];
        }

        public T this[int row, int col]
        {
            get => _data[row, col];
            set => _data[row, col] = value;
        }

        public void Generate(Func<int, int, T> generator)
        {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    _data[i, j] = generator(i, j);
        }

        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new ArgumentException("Размеры матриц должны совпадать для сложения.");

            var result = new Matrix<T>(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    result[i, j] = (dynamic)a[i, j] + b[i, j];

            return result;
        }

        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            if (a.Cols != b.Rows)
                throw new ArgumentException("Число столбцов первой матрицы должно быть равно числу строк второй матрицы.");

            var result = new Matrix<T>(a.Rows, b.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < b.Cols; j++)
                {
                    dynamic sum = default(T);
                    for (int k = 0; k < a.Cols; k++)
                        sum += (dynamic)a[i, k] * b[k, j];
                    result[i, j] = sum;
                }

            return result;
        }

        public void SaveToCsv(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < Rows; i++)
                {
                    var row = new List<string>();
                    for (int j = 0; j < Cols; j++)
                    {
                        double value = Convert.ToDouble(this[i, j]);
                        row.Add(value.ToString("F2", CultureInfo.InvariantCulture).Replace('.', ','));
                    }
                    writer.WriteLine(string.Join(";", row));
                }
            }
        }
    }
}
