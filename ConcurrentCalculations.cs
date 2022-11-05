using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

//Created by Henrik Wiener : 11/3/2023
public class Concurrency
{
    // Data Fields
    private const ulong OneHundredMillion = 10_000_000;
    private const ulong TenBillion = 1_000_000_000;

    private ulong _sum; // sum of values

    public TimeSpan ConcurrentTimeElapsed { get; set; }

    // This method will concurrently run 100 threads each summing 10 million numbers up to 1 billion
    public async void AddToOneBillionConcurrently()
    {
        Thread[] taskList = new Thread[100];
        ulong[] results = new ulong[taskList.Length];

        ulong start = 1;
        ulong end = OneHundredMillion;

        Stopwatch sw = Stopwatch.StartNew();

        // Sum in increments of 10 million 100 times concurrently up to 1 billion
        for (ulong i = 0; i < 100; i++)
        {
            ulong index = i;
            taskList[index] = new Thread(() => AddToTenMillion(start, end));
            taskList[index].Name = $"Thread({index})";
            taskList[index].Start();


            start += OneHundredMillion; // add 10 million
            end += OneHundredMillion; // add 10 million
        }
        // Wait for tasks to finish running before stopping the stopwatch. 
        foreach (Thread t in taskList)
        {
            t.Join();
        }
        sw.Stop();

        //_sum = results.Sum();
        ConcurrentTimeElapsed += sw.Elapsed;
 
        Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);
        Console.WriteLine("Total sum: " + _sum);
    } // end method

    // This method will add the sum of each number from 0 to 10 million
    private object myLock = new object();
    public void AddToTenMillion(ulong start, ulong end)
    {
        ulong sum = 0;
        //Stopwatch sw = Stopwatch.StartNew();
        for (ulong i = start; i <= end; i++)
        {
            sum += i;
        }
        //sw.Stop();
        //ConcurrentTimeElapsed += sw.Elapsed;
        //Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);

        Interlocked.Add(ref _sum, sum);
    } // end method 

    // This method syncrhonously sums to 1 billion.
    public void SumToOneBillion()
    {
        ulong sum = 0;
        Stopwatch sw = Stopwatch.StartNew();
        //ulong sum = (TenBillion * (TenBillion + 1)) / 2;     // Using Gauss Summation
        for (ulong i = 1; i <= TenBillion; i++)
        {
            sum += i;
        }

        sw.Stop();

        ConcurrentTimeElapsed += sw.Elapsed;
        Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);
        Console.WriteLine("Total Sum: " + sum);
    } // end method

    static void Main(string[] args)
    {
        Concurrency c = new();
        c.AddToOneBillionConcurrently(); //Time elapsed: 00:00:00.3327248   Total sum: 503000000500000000

        //c.AddToTenMillion(1, OneHundredMillion); // Time elapsed: 00:00:00.0147405

        //c.SumToOneBillion(); //Time elapsed: 00:00:01.7290499   Total Sum: 500000000500000000

    } // end Main
} // end class