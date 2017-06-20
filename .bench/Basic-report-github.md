``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |             Method |      Mean |     Error |    StdDev |    StdErr |       Min |        Q1 |    Median |        Q3 |       Max |           Op/s | Allocated |
 |------------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|---------------:|----------:|
 |               Noop | 0.0177 ns | 0.0272 ns | 0.0255 ns | 0.0066 ns | 0.0000 ns | 0.0000 ns | 0.0046 ns | 0.0308 ns | 0.0841 ns | 56480857076.18 |      0 kB |
 |  IndexingFirstItem | 0.2901 ns | 0.0071 ns | 0.0063 ns | 0.0017 ns | 0.2807 ns | 0.2850 ns | 0.2895 ns | 0.2948 ns | 0.3038 ns |  3446540340.57 |      0 kB |
 | IndexingMiddleItem | 0.2871 ns | 0.0089 ns | 0.0083 ns | 0.0021 ns | 0.2720 ns | 0.2827 ns | 0.2871 ns | 0.2939 ns | 0.3065 ns |  3482907041.31 |      0 kB |
 |   IndexingLastItem | 0.2847 ns | 0.0053 ns | 0.0045 ns | 0.0012 ns | 0.2744 ns | 0.2824 ns | 0.2852 ns | 0.2886 ns | 0.2900 ns |  3512542370.15 |      0 kB |
