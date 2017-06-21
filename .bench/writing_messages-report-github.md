``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 | Method |          Mean |      Error |     StdDev |    StdErr |           Min |            Q1 |        Median |            Q3 |           Max |      Op/s | Allocated |
 |------- |--------------:|-----------:|-----------:|----------:|--------------:|--------------:|--------------:|--------------:|--------------:|----------:|----------:|
 |  small |   277.9595 ns |  0.5645 ns |  0.4713 ns | 0.1307 ns |   277.0412 ns |   277.7630 ns |   277.9149 ns |   278.3388 ns |   278.7591 ns |   3597646 |      0 kB |
 | medium | 1,155.8750 ns |  5.7007 ns |  5.3324 ns | 1.3768 ns | 1,149.4811 ns | 1,150.8546 ns | 1,156.4018 ns | 1,158.2717 ns | 1,168.5320 ns | 865145.49 |      0 kB |
 |  large | 2,467.5611 ns | 17.9730 ns | 16.8119 ns | 4.3408 ns | 2,437.3904 ns | 2,458.1457 ns | 2,463.6641 ns | 2,480.4953 ns | 2,506.9473 ns | 405258.45 |      0 kB |
