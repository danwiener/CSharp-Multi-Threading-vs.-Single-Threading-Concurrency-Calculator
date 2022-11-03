using System.Diagnostics;
using System.Runtime.CompilerServices;

public class Concurrency
{
    public TimeSpan ConcurrentTimeElapsed { get; set; }

    public ulong TotalSum { get;set; }

    // This method will concurrently run 100 threads summing up to 100 million
    public async void AddToTenBillionConcurrently()
    {
        Task[] taskList = new Task[100];

        ulong start = 1;
        ulong end = 100000000;

        for (int i = 0; i < 100; i++)
        { 
            taskList[i] = (new Task(() => AddToOneHundredMillion(start, end)));
            ulong s = AddToOneHundredMillion(start, end);
            TotalSum += s;
            start += 100000000;
            end += 100000000;
        }
        Stopwatch sw = Stopwatch.StartNew();
        foreach (Task t in taskList)
        {
            t.Start();
        }
        Task.WaitAll(taskList);
        sw.Stop();

        ConcurrentTimeElapsed += sw.Elapsed;

        Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);
        Console.WriteLine("Total sum: " + TotalSum);
    } // end method

    // This method will add the sum of each number from 0 to 100 million
    public ulong AddToOneHundredMillion(ulong start, ulong end)
    {
        //Stopwatch sw = Stopwatch.StartNew();
        ulong sum = 0;
        for (ulong i = start; i <= end; i++)
        {
            sum += i;
        }

        //ConcurrentTimeElapsed += sw.Elapsed;
        //Console.WriteLine("Time elapsed: " + ConcurrentTimeElapsed);

        return sum;
    } // end method 

    // This method syncrhonously sums to 10 billion.
    public void SumTo10Billion()
    {
        Stopwatch sw = Stopwatch.StartNew();
        ulong sum = 0;
        for (ulong i = 1; i <= 10000000000; i++)
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
        c.AddToTenBillionConcurrently(); // Time elapsed: 00:00:03.0661120      Total sum: 13106511857580896768

        //c.AddToOneHundredMillion(1, 100000000); // Time elapsed: 00:00:00.1756455

        //c.SumTo10Billion(); // Time elapsed: 00:00:17.0516249                 Total Sum: 13106511857580896768
    } // end Main
} // end class