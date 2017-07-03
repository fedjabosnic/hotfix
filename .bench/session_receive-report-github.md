``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 | Method |          Mean |      Error |     StdDev |     StdErr |           Min |            Q1 |        Median |            Q3 |           Max |       Op/s | Allocated |
 |------- |--------------:|-----------:|-----------:|-----------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|----------:|
 |  small |   922.5739 ns |  8.9980 ns |  8.4168 ns |  2.1732 ns |   911.8879 ns |   915.3006 ns |   921.6549 ns |   928.0504 ns |   939.5186 ns | 1083924.04 |      0 kB |
 | medium | 2,519.4718 ns | 14.4516 ns | 13.5180 ns |  3.4903 ns | 2,499.4726 ns | 2,509.0117 ns | 2,516.9755 ns | 2,525.3850 ns | 2,545.0181 ns |  396908.59 |      0 kB |
 |  large | 6,046.8065 ns | 46.6824 ns | 43.6667 ns | 11.2747 ns | 5,995.6543 ns | 6,010.3130 ns | 6,030.0913 ns | 6,094.8681 ns | 6,136.6264 ns |  165376.55 |      0 kB |
