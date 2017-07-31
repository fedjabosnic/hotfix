using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace HotFix.Utilities
{
    internal static class Os
    {
        internal const int SIO_LOOPBACK_FAST_PATH = -1744830448;
        internal const int WSAEOPNOTSUPP = 10045;

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        internal static extern void GetSystemTimePreciseAsFileTime(out long filetime);

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
    }
}