# ConcurrentCalculations

This application makes use of the asynchronous built-in .NET class Task to evaluate calculation time using concurrency/asyncrhonisity. 

It contains three methods.

One method synchronously sums every number from 1 to 10 billion.
Results:

//Time elapsed: 00:00:01.7290499   Total Sum: 500000000500000000

Another sums synchronously from 1 to 100 million.
Results:

// Time elapsed: 00:00:00.0147405

The last method creates 100 asynchronous tasks which simultaneously run the method that sums from 1 to 100 million, but modifies each one so the first sums from 1 to 100 million, the next from 100 million + 1 to 200 million, the next from 200 million + 1 to 300 million, and so forth, up to 10 billion.
Results:

//Time elapsed: 00:00:00.3327248   Total sum: 503000000500000000

