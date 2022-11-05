using System.Diagnostics;

//Created by Henrik Wiener : 11/3/2023
public class Concurrency
{
    // Data Fields
    private const double OneHundredMillion = 100_000_000;
    private const double TenBillion = 10_000_000_000;

    private double _sum; // sum of values

    public TimeSpan ConcurrentTimeElapsed { get; set; }

    // This method will concurrently run 100 threads each summing 100 million numbers up to 10 billion
    public async void AddToTenBillionConcurrently()
    {
        Thread[] taskList = new Thread[100];

        double start = 1;
        double end = OneHundredMillion;

        Stopwatch sw = Stopwatch.StartNew();

        // Sum in increments of 100 million 100 times concurrently up to 10 billion
        for (double i = 0; i < 100; i++)
        {
            int index = (int)i;
            taskList[index] = new Thread(() => AddToOneHundredMillion(start, end));
            taskList[index].Name = $"Thread({index})";
            Task task = Task.Factory.StartNew(() => taskList[index].Start());
            task.Wait();

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
    // This method will add the sum of each number from 0 to 100 million
    private object myLock = new object();
    public void AddToOneHundredMillion(double start, double end)
    {
        double sum = 0;
        Stopwatch sw = Stopwatch.StartNew();
        for (double i = start; i <= end; i++)
        {
            sum += i;
        }
        sw.Stop();
        ConcurrentTimeElapsed += sw.Elapsed;
        Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);

        lock (myLock)
        {
            _sum += sum;
        }
        //Interlocked.Add(ref _sum, sum);
    } // end method 

    // This method syncrhonously sums to 10 billion.
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

    static void Main(string[] args)
    {
        Concurrency c = new();
        //c.AddToTenBillionConcurrently(); // Time elapsed: 00:00:04.2092471   Total sum: 5.000000000026831E+19

        //c.AddToOneHundredMillion(1, OneHundredMillion); // Time elapsed: 00:00:00.2667141

        //c.SumToTenBillion(); // Time elapsed: 00:00:27.3592470   Total Sum: 5.000000000006786E+19

    } // end Main
} // end class