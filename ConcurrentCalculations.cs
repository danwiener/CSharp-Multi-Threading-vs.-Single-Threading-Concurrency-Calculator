using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

public class Concurrency
{
    // Data Fields
    private const double TenMillion = 10000000;
    private const double OneBillion = 1000000000;

    // Properties
    public TimeSpan ConcurrentTimeElapsed { get; set; }

    public double TotalSum { get;set; }

    // This method will concurrently run 100 threads summing 10 million numbers
    public async void AddToOneBillionConcurrently()
    {
        Task<double>[] taskList = new Task<double>[100];
        double[] results = new double[taskList.Length];

        double start = 1;
        double end = TenMillion;
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < 100; i++)
        {
            int index = i;
            taskList[index] = Task<double>.Factory.StartNew(() => AddToTenMillion(start, end));

            results[i] = taskList[index].Result;
            
            start += TenMillion; // add 10 million
            end += TenMillion; // add 10 million
        }
        Task.WaitAll(taskList);
        TotalSum += results.Sum();
        sw.Stop();
        // sum in increments of 10 million 100 times concurrently up to 10 billion




        ConcurrentTimeElapsed += sw.Elapsed;

        Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);
        Console.WriteLine("Total sum: " + TotalSum);
    } // end method

    // This method will add the sum of each number from 0 to 10 million
    public double AddToTenMillion(double start, double end)
    {
        //Stopwatch sw = Stopwatch.StartNew();
        double sum = 0;
            for (double i = start; i <= end; i++)
            {
                sum += i;
            }

        //ConcurrentTimeElapsed += sw.Elapsed;
        //Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);

        return sum;
    } // end method 

    // This method syncrhonously sums to 1 billion.
    public void SumToOneBillion()
    {
        Stopwatch sw = Stopwatch.StartNew();
        double sum = 0;
        for (double i = 1; i <= OneBillion; i++)
        {
            sum += i;
        }
        sw.Stop();

        ConcurrentTimeElapsed += sw.Elapsed;
        Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);

        TotalSum = sum;
        Console.WriteLine("Total Sum: " + TotalSum);
    } // end method

    static void Main(string[] args)
    {
        Concurrency c = new();
        c.AddToOneBillionConcurrently(); // Time elapsed: 00:00:02.7011199    Total sum:  Total sum: 5.0000000049746195E+17

        //c.AddToTenMillion(1, TenMillion); // Time elapsed: 00:00:00.2705015

        //c.SumToOneBillion(); // Time elapsed: 00:00:02.6888783                 Total Sum: 5.00000000067109E+17


    } // end Main
} // end class