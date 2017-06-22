``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 | Method |          Mean |     Error |    StdDev |    StdErr |           Min |            Q1 |        Median |            Q3 |           Max |       Op/s | Allocated |
 |------- |--------------:|----------:|----------:|----------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|----------:|
 |  small |   273.1194 ns | 1.6129 ns | 1.4298 ns | 0.3821 ns |   270.6998 ns |   272.1641 ns |   273.0994 ns |   273.9549 ns |   275.2516 ns | 3661402.84 |      0 kB |
 | medium | 1,283.0991 ns | 5.4302 ns | 4.5345 ns | 1.2576 ns | 1,277.3756 ns | 1,279.2442 ns | 1,282.9264 ns | 1,285.4503 ns | 1,292.6267 ns |  779363.04 |      0 kB |
 |  large | 2,847.0672 ns | 9.5055 ns | 8.4264 ns | 2.2521 ns | 2,831.8931 ns | 2,840.4603 ns | 2,849.9262 ns | 2,854.8725 ns | 2,856.2969 ns |  351238.64 |      0 kB |
