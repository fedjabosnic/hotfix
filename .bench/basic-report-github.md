``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |               Method |      Mean |     Error |    StdDev |    StdErr |    Median |       Min |        Q1 |        Q3 |       Max |          Op/s | Allocated |
 |--------------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|--------------:|----------:|
 |                 noop | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |      Infinity |      0 kB |
 |  indexing_first_item | 0.4664 ns | 0.0449 ns | 0.0552 ns | 0.0118 ns | 0.4689 ns | 0.3566 ns | 0.4207 ns | 0.5065 ns | 0.5745 ns | 2144130970.19 |      0 kB |
 | indexing_middle_item | 0.3272 ns | 0.0321 ns | 0.0300 ns | 0.0078 ns | 0.3186 ns | 0.2987 ns | 0.3008 ns | 0.3483 ns | 0.3999 ns |    3056190251 |      0 kB |
 |   indexing_last_item | 0.3154 ns | 0.0213 ns | 0.0199 ns | 0.0051 ns | 0.3131 ns | 0.2755 ns | 0.3032 ns | 0.3312 ns | 0.3551 ns | 3170420617.83 |      0 kB |
