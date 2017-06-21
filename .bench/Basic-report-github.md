``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |               Method |      Mean |     Error |    StdDev |    StdErr |       Min |        Q1 |    Median |        Q3 |       Max |            Op/s | Allocated |
 |--------------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------------:|----------:|
 |                 noop | 0.0017 ns | 0.0052 ns | 0.0046 ns | 0.0012 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0167 ns | 571636998848.75 |      0 kB |
 |  indexing_first_item | 0.3087 ns | 0.0197 ns | 0.0184 ns | 0.0048 ns | 0.2860 ns | 0.2923 ns | 0.3063 ns | 0.3266 ns | 0.3449 ns |   3239480961.77 |      0 kB |
 | indexing_middle_item | 0.3081 ns | 0.0195 ns | 0.0173 ns | 0.0046 ns | 0.2876 ns | 0.2990 ns | 0.3015 ns | 0.3151 ns | 0.3451 ns |   3245238090.27 |      0 kB |
 |   indexing_last_item | 0.2891 ns | 0.0091 ns | 0.0086 ns | 0.0022 ns | 0.2710 ns | 0.2832 ns | 0.2914 ns | 0.2962 ns | 0.2980 ns |   3458867577.36 |      0 kB |
