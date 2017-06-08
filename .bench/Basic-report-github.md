``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316022 Hz, Resolution=301.5662 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-YPJJZU : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=10000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  WarmupCount=5  

```
 |             Method |      Mean |     Error |    StdDev |    StdErr |    Median |       Min |        Q1 |        Q3 |       Max |            Op/s | Allocated |
 |------------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------------:|----------:|
 |               Noop | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |        Infinity |      0 kB |
 |  IndexingFirstItem | 0.2830 ns | 0.0672 ns | 0.0351 ns | 0.0124 ns | 0.2905 ns | 0.2453 ns | 0.2453 ns | 0.3056 ns | 0.3357 ns |   3533948210.87 |      0 kB |
 | IndexingMiddleItem | 0.2573 ns | 0.0673 ns | 0.0445 ns | 0.0141 ns | 0.2634 ns | 0.2031 ns | 0.2031 ns | 0.2935 ns | 0.3237 ns |   3885962543.21 |      0 kB |
 |   IndexingLastItem | 0.2339 ns | 0.0534 ns | 0.0318 ns | 0.0106 ns | 0.2372 ns | 0.2071 ns | 0.2071 ns | 0.2523 ns | 0.2975 ns |   4275676625.83 |      0 kB |
 |        AdditionInt | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |        Infinity |      0 kB |
 |       AdditionByte | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |        Infinity |      0 kB |
 |       AdditionChar | 0.0094 ns | 0.0227 ns | 0.0150 ns | 0.0047 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0161 ns | 0.0462 ns | 105829064894.38 |      0 kB |
 |       IncrementInt | 0.0165 ns | 0.0375 ns | 0.0223 ns | 0.0074 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0372 ns | 0.0523 ns |  60495254483.37 |      0 kB |
 |      IncrementByte | 0.0181 ns | 0.0333 ns | 0.0220 ns | 0.0070 ns | 0.0121 ns | 0.0000 ns | 0.0000 ns | 0.0241 ns | 0.0543 ns |  55267244762.05 |      0 kB |
 |      IncrementChar | 0.0536 ns | 0.1954 ns | 0.1163 ns | 0.0388 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0704 ns | 0.3418 ns |  18652617791.28 |      0 kB |
