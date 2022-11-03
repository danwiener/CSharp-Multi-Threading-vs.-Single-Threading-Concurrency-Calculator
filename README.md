# ConcurrentCalculations

This application makes use of the asynchronous built-in .NET class Task to evaluate calculation time using concurrency. 

It contains three methods.

One synchronously takes the sum of every number from 1 to 10 billion, and times the execution.

Another synchronously sums every number from 1 to 100 million.

Last, one created 100 tasks which will aynchronously run the method that sums from 1 to 100 million in parallel, 
but will modify each method so that the first sums from 1 to 100 million, the next from 100 million and 1 to 200 million, 
and so forth, up to 10 billion.

Results:
Synchronous summing to 10 billion: Time elapsed: 00:00:17.0516249  Total Sum: 13106511857580896768

Synchronous summing to 100 million : Time elapsed: 00:00:00.1756455

Asynchronous summing to 10 billion: Time elapsed: 00:00:03.0661120 Total sum: 13106511857580896768

