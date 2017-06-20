``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |   Method |       Mean |     Error |    StdDev |    StdErr |        Min |         Q1 |     Median |         Q3 |        Max |        Op/s | Scaled | ScaledSD | Allocated |
 |--------- |-----------:|----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|------------:|-------:|---------:|----------:|
 | Standard | 14.4605 ns | 2.7060 ns | 1.4153 ns | 0.5004 ns | 11.3287 ns | 14.1963 ns | 14.9510 ns | 15.2528 ns | 15.5547 ns | 69154117.21 |   1.00 |     0.00 |   0.03 kB |
 |   Hotfix | 15.3086 ns | 0.8522 ns | 0.3784 ns | 0.1430 ns | 14.7912 ns | 15.0930 ns | 15.0930 ns | 15.6968 ns | 15.6968 ns | 65322570.79 |   1.07 |     0.12 |   0.03 kB |
