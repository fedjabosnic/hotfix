``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 | Method |          Mean |      Error |     StdDev |    StdErr |           Min |            Q1 |        Median |            Q3 |           Max |       Op/s | Allocated |
 |------- |--------------:|-----------:|-----------:|----------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|----------:|
 |  small |   396.1210 ns |  1.2315 ns |  1.0917 ns | 0.2918 ns |   393.8838 ns |   395.5651 ns |   396.4373 ns |   396.9119 ns |   397.5838 ns | 2524481.02 |      0 kB |
 | medium | 1,262.2504 ns |  5.8264 ns |  5.1649 ns | 1.3804 ns | 1,253.0371 ns | 1,260.1321 ns | 1,262.1798 ns | 1,264.2028 ns | 1,273.3541 ns |  792235.84 |      0 kB |
 |  large | 3,306.4962 ns | 19.8277 ns | 18.5468 ns | 4.7888 ns | 3,283.8474 ns | 3,289.7386 ns | 3,299.2868 ns | 3,322.8409 ns | 3,347.3280 ns |  302434.94 |      0 kB |
