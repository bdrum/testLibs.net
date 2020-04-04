using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

// TODO: show how to break the Parallel.For
// TODO: show how to get the state of Parallel.For
// TODO: show how to add local var and sync(agregate) iteration between itself

namespace flow
{
    [RPlotExporter, RankColumn]
    public class MatrixMult
    {
        private double[,] matA;
        private double[,] matB;

        #region Helper_Methods
        public double[,] InitializeMatrix(int rows, int cols)
        {
            double[,] matrix = new double[rows, cols];

            Random r = new Random();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = r.Next(100);
                }
            }
            return matrix;
        }
        #endregion

        [Params(200, 500, 1000, 2000)]
        public int colCount;
        [Params(200, 500)]
        public int rowCount;
        [Params(200)]
        public int colCount2;

        [GlobalSetup]
        public void Setup()
        {
            matA = InitializeMatrix(rowCount, colCount);
            matB = InitializeMatrix(colCount, colCount2);
        }

        #region Sequential_Loop
        [Benchmark]
        public void MultiplyMatricesSequential()
        {
            int matACols = matA.GetLength(1);
            int matBCols = matB.GetLength(1);
            int matARows = matA.GetLength(0);
            var result = new double[matARows, matBCols];

            for (int i = 0; i < matARows; i++)
            {
                for (int j = 0; j < matBCols; j++)
                {
                    double temp = 0;
                    for (int k = 0; k < matACols; k++)
                    {
                        temp += matA[i, k] * matB[k, j];
                    }
                    result[i, j] += temp;
                }
            }
        }
        #endregion

        #region Parallel_Loop
        [Benchmark]
        public void MultiplyMatricesParallel()
        {
            int matACols = matA.GetLength(1);
            int matBCols = matB.GetLength(1);
            int matARows = matA.GetLength(0);
            var result = new double[matARows, matBCols];

            // A basic matrix multiplication.
            // Parallelize the outer loop to partition the source array by rows.
            Parallel.For(0, matARows, i =>
            {
                for (int j = 0; j < matBCols; j++)
                {
                    double temp = 0;
                    for (int k = 0; k < matACols; k++)
                    {
                        temp += matA[i, k] * matB[k, j];
                    }
                    result[i, j] = temp;
                }
            }); // Parallel.For
        }
        #endregion

    }

    class Program
    { 
        static void Main(string[] args)
        {
            var b = BenchmarkRunner.Run<MatrixMult>();
        }
    }
}