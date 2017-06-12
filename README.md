# Hotfix

Hotix is a **FIX engine** written in pure .net geared towards **low latency** and **high throughput**.

### Overview

Hotfix is geared towards low latency and high throughput on the **inbound** side of things - *the immediate requirement being to support market data sessions with a high throughput of large messages with multiple groups*. While we will make every effort to ensure that sending messages is efficient, it is not the main priority for day one. 

#### Features
- Run as initiator
- FIX Protocol conformance
    - Required field checks
    - Message length and checksum verification
    - Logon and logoff procedures and validation
    - Heartbeat handling and issue resolution (test requests handling)
    - Sequence number tracking and resolution (full reset/resend handling)

The core library will be lean and focused on protocol basics so the out-of-the-box feature set will be minimal. Once the core project is stable we plan to later add extensibility libraries for:
- Running as acceptor (*accepting inbound connections and managing multiple client connections*)
- Session persistance (*persisting sequence numbers and messages for reconnection/resend*)
- Validation (*against a fix dictionary or similar*)
- Security (*SSL, message encryption etc*)
- Logging (*file/network etc*)

> Some of the above should be possible now if you want to hook in and handle them yourself...

### Performance

So how fast is it?

> As with all benchmarks and statistics - use a light pinch of salt

#### TCP overhead

Coming soon...

#### Message parsing

 | Message size |      Min |   Median |      Max |     Mean |     StdDev |         **Op/s** | Allocated |
 |------------- |---------:|---------:|---------:|---------:|-----------:|-----------------:|----------:|
 | Small        | 0.223 us | 0.224 us | 0.226 us | 0.224 us |  1.2260 ns | **4,448,779.01** |      0 kB |
 | Medium       | 0.656 us | 0.664 us | 0.685 us | 0.667 us |  9.3169 ns | **1,498,608.51** |      0 kB |
 | Large        | 1.741 us | 1.763 us | 1.777 us | 1.759 us | 10.1999 ns |   **568,278.89** |      0 kB |

- `Small message`: `0.10 KB` message with `10 fields` (*eg. heartbeat*)
- `Med'm message`: `0.35 KB` message with `30 fields` (*eg. execution report*)
- `Large message`: `1.00 KB` message with `90 fields` (*eg. market data incremental refresh*)

#### Field extraction

 | Field type   |          Min |       Median |          Max |         Mean |    StdDev |           **Op/s** | Allocated |
 |------------- |-------------:|-------------:|-------------:|-------------:|----------:|-------------------:|----------:|
 | Int          |    9.4541 ns |    9.4541 ns |    9.7557 ns |    9.5833 ns | 0.1612 ns | **104,347,706.83** |      0 kB |
 | Long         |   14.8220 ns |   14.8220 ns |   14.8220 ns |   14.8220 ns | 0.3976 ns |  **67,467,369.41** |      0 kB |
 | Float        |   22.1199 ns |   22.1199 ns |   24.2309 ns |   22.4214 ns | 0.7387 ns |  **44,600,153.49** |      0 kB |
 | String       |    9.5898 ns |   15.4704 ns |   19.5415 ns |   14.7164 ns | 3.2167 ns |  **67,951,248.78** |   0.03 kB |
 | DateTime     |   56.6492 ns |   56.6492 ns |   56.6492 ns |   56.6492 ns | 0.0000 ns |  **17,652,493.71** |      0 kB |

> Allocation when retrieving a string is unavoidable in .net - avoid if possible

#### Garbage allocation/collection
`Almost zero` - *we currently allocate one string per message and accessing any field as a string will cause an additional allocation (we will avoid doing this in the core engine)*

#### Further statistics
For more *statistics* and *benchmarks*, see the `.bench` folder in the repository root.

> The benchmarks will be run and committed as/when core functionality that affects the figures is changed.
