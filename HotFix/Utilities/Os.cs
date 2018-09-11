using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace HotFix.Utilities
{
    internal unsafe static class Os
    {
        internal const int SIO_LOOPBACK_FAST_PATH = -1744830448;
        internal const int WSAEOPNOTSUPP = 10045;

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        internal static extern void GetSystemTimePreciseAsFileTime(out long filetime);

        [DllImport("ws2_32.dll", SetLastError = true)]
        internal static extern int select(
            [In] int ignoredParameter,
            [In, Out] IntPtr* readfds,
            [In, Out] IntPtr* writefds,
            [In, Out] IntPtr* exceptfds,
            [In] ref TimeValue timeout);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct TimeValue
        {
            public int Seconds;
            public int Microseconds;
        }

        /// <summary>
        /// Attempts to enable the fast path for localhost.
        /// <remarks>
        /// Does nothing if unavailable on the current platform.
        /// </remarks>
        /// </summary>
        /// <param name="socket">The socket.</param>
        internal static void EnableFastPath(this Socket socket)
        {
            try
            {
                // Attempt to set fast loopback if available
                socket.IOControl(SIO_LOOPBACK_FAST_PATH, BitConverter.GetBytes(1), null);
            }
            catch (SocketException e)
            {
                if (e.ErrorCode != WSAEOPNOTSUPP) throw;
            }
        }

        /// <summary>
        /// A garbage-free implementation of the <see cref="Socket.Poll"/> function (only providing read polling).
        /// </summary>
        /// <param name="socket">The socket to poll.</param>
        /// <param name="microseconds">The duration to wait.</param>
        /// <returns>Whether there is data available to be read.</returns>
        internal static bool Poll(this Socket socket, int microseconds)
        {
            if (microseconds == 0) return socket.Available != 0;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return socket.Poll(microseconds, SelectMode.SelectRead);

            var tv = new Os.TimeValue { Seconds = microseconds / 1000000, Microseconds = microseconds % 1000000 };
            var descriptor = stackalloc IntPtr[2];

            descriptor[0] = (IntPtr)1;
            descriptor[1] = socket.Handle;

            Os.select(0, descriptor, null, null, ref tv);

            return (int)descriptor[0] != 0 && descriptor[1] == socket.Handle;
        }
    }
}