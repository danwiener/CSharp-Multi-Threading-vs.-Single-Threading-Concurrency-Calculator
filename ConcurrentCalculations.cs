using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

public class Concurrency
{
    // Data Fields
    private const double OneHundredMillion = 100000000;
    private const double TenBillion = 10000000000;

    //0 for false, 1 for true
    private static int usingResource = 0;

    private double _sum;

    // Properties
    public double TotalSum
    {
        get
        {
            return this._sum;
        }
        set
        {
            this._sum = value;
        }
    }
    public TimeSpan ConcurrentTimeElapsed { get; set; }

    // This method will concurrently run 100 threads each summing 100 million numbers
    public async void AddToTenBillionConcurrently()
    {
        Task<double>[] taskList = new Task<double>[100];
        double[] results = new double[taskList.Length];

        double start = 1;
        double end = OneHundredMillion;

        Stopwatch sw = Stopwatch.StartNew();

        // sum in increments of 100 million 100 times concurrently up to ten billion
        for (int i = 0; i < 100; i++)
        {
            int index = i;
            taskList[index] = Task<double>.Factory.StartNew(() => AddToOneHundredMillion(start, end));
            results[i] = taskList[i].Result;

            start += OneHundredMillion; // add 100 million
            end += OneHundredMillion; // add 100 million
        }
        Task.WaitAll(taskList);
        sw.Stop();

        _sum = results.Sum();
        ConcurrentTimeElapsed += sw.Elapsed;
 
        Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);
        Console.WriteLine("Total sum: " + _sum);
    } // end method

    // This method will add the sum of each number from 0 to 100 million
    public double AddToOneHundredMillion(double start, double end)
    {
        double sum = 0;
        //Stopwatch sw = Stopwatch.StartNew();
        for (double i = start; i <= end; i++)
        {
            sum += i;
        }
        //sw.Stop();
        //ConcurrentTimeElapsed += sw.Elapsed;
        //Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);

        return sum;
    } // end method 

    // This method syncrhonously sums to 1 billion.
    public void SumToTenBillion()
    {
        double sum = 0;
        Stopwatch sw = Stopwatch.StartNew();
        //double sum = (TenBillion * (TenBillion + 1)) / 2;     // Using Gauss Summation
        for (double i = 1; i <= TenBillion; i++)
        {
            sum += i;
        }

        sw.Stop();

        ConcurrentTimeElapsed += sw.Elapsed;
        Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);
        Console.WriteLine("Total Sum: " + sum);
    } // end method

    // Thread safe method to increment class sum field using Interlocked.
    public void IncrementSumBy(double value)
    {
        if (0 == Interlocked.Exchange(ref usingResource, 1))
        {
            _sum += value;

            Interlocked.Exchange(ref usingResource, 0);
        }
        else
        {
            Console.WriteLine("error");
        }
    }

    static void Main(string[] args)
    {
        Concurrency c = new();
        //c.AddToTenBillionConcurrently(); // Time elapsed: 00:00:26.7084345   Total sum: 5.0000000000268304E+19

        //c.AddToOneHundredMillion(1, TenMillion); // Time elapsed: 00:00:00.2705015

        //c.SumToTenBillion(); // Time elapsed: 00:00:26.4054789                Total Sum: 5.000000000006786E+19
    } // end Main
} // end class