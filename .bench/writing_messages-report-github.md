``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 | Method |          Mean |      Error |     StdDev |    StdErr |           Min |            Q1 |        Median |            Q3 |           Max |       Op/s |  Gen 0 | Allocated |
 |------- |--------------:|-----------:|-----------:|----------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|-------:|----------:|
 |  small |   364.5799 ns |  6.1630 ns |  5.4633 ns | 1.4601 ns |   358.9158 ns |   360.9089 ns |   363.0835 ns |   366.3830 ns |   374.8369 ns | 2742882.98 | 0.2476 |      0 GB |
 | medium | 1,213.7570 ns | 15.8178 ns | 14.7960 ns | 3.8203 ns | 1,195.1996 ns | 1,201.4928 ns | 1,208.7351 ns | 1,226.5384 ns | 1,241.7551 ns |  823888.15 | 0.2298 |      0 GB |
 |  large | 2,740.2430 ns | 22.8667 ns | 19.0948 ns | 5.2959 ns | 2,717.7951 ns | 2,726.9838 ns | 2,737.0456 ns | 2,743.3814 ns | 2,793.2316 ns |  364931.14 | 0.2060 |      0 GB |
