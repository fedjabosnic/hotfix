``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 | Method |          Mean |      Error |     StdDev |    StdErr |           Min |            Q1 |        Median |            Q3 |           Max |       Op/s | Allocated |
 |------- |--------------:|-----------:|-----------:|----------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|----------:|
 |  small |   279.4297 ns |  0.8958 ns |  0.8380 ns | 0.2164 ns |   278.2038 ns |   278.5142 ns |   279.5319 ns |   279.9795 ns |   280.8717 ns | 3578718.05 |      0 kB |
 | medium | 1,134.4408 ns |  2.4653 ns |  2.1854 ns | 0.5841 ns | 1,130.6822 ns | 1,133.4685 ns | 1,134.5061 ns | 1,136.0080 ns | 1,137.9757 ns |  881491.57 |      0 kB |
 |  large | 2,436.5306 ns | 21.6161 ns | 19.1621 ns | 5.1213 ns | 2,409.1859 ns | 2,423.8752 ns | 2,431.4464 ns | 2,440.1991 ns | 2,481.5601 ns |  410419.64 |      0 kB |
