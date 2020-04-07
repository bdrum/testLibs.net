using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;

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

        public MatrixMult(int colCount, int colCount2, int rowCount)
        {
            this.colCount = colCount;
            this.colCount2 = colCount2;
            this.rowCount = rowCount;
            Setup();
        }

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
        public void MultiplyMatricesParallel(CancellationToken canc)
        {
            try
            {
                int matACols = matA.GetLength(1);
                int matBCols = matB.GetLength(1);
                int matARows = matA.GetLength(0);
                var result = new double[matARows, matBCols];
                var po = new ParallelOptions();

                // A basic matrix multiplication.
                // Parallelize the outer loop to partition the source array by rows.
                var res = Parallel.For(0, matARows, new ParallelOptions { CancellationToken = canc }, (i, state) =>
                 {
                     for (int j = 0; j < matBCols; j++)
                     {
                         double temp = 0;
                         for (int k = 0; k < matACols; k++)
                         {
                             temp += matA[i, k] * matB[k, j];
                             if (k == 900)
                             {
                                 Console.WriteLine($"{i}:  It will break:");
                                 state.Stop();
                             }
                         }
                         result[i, j] = temp;
                     }
                 }); // Parallel.For

                Console.WriteLine("Complete parallel loop");
            }
            catch (OperationCanceledException oec)
            {
                Console.WriteLine("Operation was canceled after timeout");
            }
        }
        #endregion

    }

    class Program
    { 
        static void Main(string[] args)
        {
            SyncLoop();

            //var b = BenchmarkRunner.Run<MatrixMult>();
            //var s = Stopwatch.StartNew();
            //var m = new MatrixMult(1000, 5000, 3000);

            //var cts = new CancellationTokenSource();
            //cts.CancelAfter(TimeSpan.FromSeconds(20));

            //m.MultiplyMatricesParallel(cts.Token);

            //s.Stop();

            //Console.WriteLine($"Elapsed time ms: {s.ElapsedMilliseconds}");
        }


        static void SyncLoop()
        {

            Console.WriteLine("Start example with sync:");
            var s = Stopwatch.StartNew();
            var size = 1000000000;
            var arr = Enumerable.Range(0, size).ToArray();

            long total = 0;

            var result = Parallel.For(0, size, i => {

                Interlocked.Add(ref total, i);

            });

            s.Stop();

            Console.WriteLine($"Sum of the arr is {total}");

            Console.WriteLine($"The loop was completed: {result.IsCompleted}");
            Console.WriteLine($"Elapsed time is {s.ElapsedMilliseconds} ms");

            // output

            //Start example with sync:
            //Sum of the arr is 499999999500000000
            //The loop was completed: True
            //Elapsed time is 28918 ms
        }
    }
}