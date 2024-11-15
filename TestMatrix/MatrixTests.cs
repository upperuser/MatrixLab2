using Microsoft.VisualStudio.TestTools.UnitTesting;
using MatrixCalculation;
using System;

namespace TestsForMatrix
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        public void CorrectRowNumberInMatrixTest()
        {
            var matrixA = new Matrix<int>(2, 3);
            Assert.AreEqual(2, matrixA.Rows);
        }

        [TestMethod]
        public void CorrectColumnNumberInMatrixTest()
        {
            var matrixA = new Matrix<int>(3, 3);
            Assert.AreEqual(3, matrixA.Cols);
        }

        [TestMethod]
        public void MatrixAddition_ValidInput_ReturnsCorrectResult()
        {
            var matrixA = new Matrix<int>(2, 2);
            var matrixB = new Matrix<int>(2, 2);

            matrixA[0, 0] = 1; matrixA[0, 1] = 2;
            matrixA[1, 0] = 3; matrixA[1, 1] = 4;

            matrixB[0, 0] = 5; matrixB[0, 1] = 6;
            matrixB[1, 0] = 7; matrixB[1, 1] = 8;

            var expected = new Matrix<int>(2, 2);
            expected[0, 0] = 6; expected[0, 1] = 8;
            expected[1, 0] = 10; expected[1, 1] = 12;

            var result = matrixA + matrixB;

            for (int i = 0; i < expected.Rows; i++)
            {
                for (int j = 0; j < expected.Cols; j++)
                {
                    Assert.AreEqual(expected[i, j], result[i, j]);
                }
            }
        }

        [TestMethod]
        public void MatrixMultiplication_ValidInput_ReturnsCorrectResult()
        {
            var matrixA = new Matrix<int>(2, 2);
            var matrixB = new Matrix<int>(2, 2);

            matrixA[0, 0] = 1; matrixA[0, 1] = 2;
            matrixA[1, 0] = 3; matrixA[1, 1] = 4;

            matrixB[0, 0] = 5; matrixB[0, 1] = 6;
            matrixB[1, 0] = 7; matrixB[1, 1] = 8;

            var expected = new Matrix<int>(2, 2);
            expected[0, 0] = 19; expected[0, 1] = 22;
            expected[1, 0] = 43; expected[1, 1] = 50;

            var result = matrixA * matrixB;

            for (int i = 0; i < expected.Rows; i++)
            {
                for (int j = 0; j < expected.Cols; j++)
                {
                    Assert.AreEqual(expected[i, j], result[i, j]);
                }
            }
        }

        [TestMethod]
        public void MatrixAddition_DifferentDimensions_ThrowsException()
        {
            var matrixA = new Matrix<int>(1, 2);
            var matrixB = new Matrix<int>(2, 2);

            Assert.ThrowsException<ArgumentException>(() => { var result = matrixA + matrixB; });
        }

        [TestMethod]
        public void MatrixMultiplication_IncompatibleDimensions_ThrowsException()
        {
            var matrixA = new Matrix<int>(1, 2);
            var matrixB = new Matrix<int>(1, 2);

            Assert.ThrowsException<ArgumentException>(() => { var result = matrixA * matrixB; });
        }

        [TestMethod]
        public void MatrixIndexing_GetAndSetValues_ReturnsCorrectValues()
        {
            var matrix = new Matrix<int>(2, 2);
            matrix[0, 0] = 42;

            Assert.AreEqual(42, matrix[0, 0]);
        }

        [TestMethod]
        public void MatrixEquality_SameMatrices_ReturnsTrue()
        {
            var matrixA = new Matrix<int>(2, 2);
            var matrixB = new Matrix<int>(2, 2);

            matrixA[0, 0] = 1; matrixA[0, 1] = 2;
            matrixA[1, 0] = 3; matrixA[1, 1] = 4;

            matrixB[0, 0] = 1; matrixB[0, 1] = 2;
            matrixB[1, 0] = 3; matrixB[1, 1] = 4;

            for (int i = 0; i < matrixA.Rows; i++)
            {
                for (int j = 0; j < matrixA.Cols; j++)
                {
                    Assert.AreEqual(matrixA[i, j], matrixB[i, j]);
                }
            }
        }

        [TestMethod]
        public void MatrixEquality_DifferentMatrices_ReturnsFalse()
        {
            var matrixA = new Matrix<int>(2, 2);
            var matrixB = new Matrix<int>(2, 2);

            matrixA[0, 0] = 1; matrixA[0, 1] = 2;
            matrixA[1, 0] = 3; matrixA[1, 1] = 4;

            matrixB[0, 0] = 5; matrixB[0, 1] = 6;
            matrixB[1, 0] = 7; matrixB[1, 1] = 8;

            Assert.AreNotEqual(matrixA, matrixB);
        }

        [TestMethod]
        public void MatrixAddition_WithFloatValues_ReturnsCorrectResult()
        {
            var matrixA = new Matrix<double>(2, 2);
            var matrixB = new Matrix<double>(2, 2);

            matrixA[0, 0] = 1.1; matrixA[0, 1] = 2.2;
            matrixA[1, 0] = 3.3; matrixA[1, 1] = 4.4;

            matrixB[0, 0] = 0.9; matrixB[0, 1] = 1.8;
            matrixB[1, 0] = 2.7; matrixB[1, 1] = 3.6;

            var expected = new Matrix<double>(2, 2);
            expected[0, 0] = 2.0; expected[0, 1] = 4.0;
            expected[1, 0] = 6.0; expected[1, 1] = 8.0;

            var result = matrixA + matrixB;

            for (int i = 0; i < expected.Rows; i++)
            {
                for (int j = 0; j < expected.Cols; j++)
                {
                    Assert.AreEqual(expected[i, j], result[i, j], 0.001);
                }
            }
        }
    }
}
