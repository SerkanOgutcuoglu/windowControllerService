using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowControlService
{
    public partial class Controller : ServiceBase
    {
        public Controller()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartAppInActiveSession();
        }

        protected override void OnStop()
        {
        }

        private void StartAppInActiveSession()
        {
            uint sessionId = WTSGetActiveConsoleSessionId();

            if (WTSQueryUserToken(sessionId, out IntPtr token))
            {
                STARTUPINFO si = new STARTUPINFO();
                si.cb = Marshal.SizeOf(si);

                PROCESS_INFORMATION pi = new PROCESS_INFORMATION();

                string exePath = @"C:\Users\Serkan\source\repos\windowController\windowController\bin\Debug\net9.0\windowController.exe";

                bool result = CreateProcessAsUser(
                    token,
                    null,
                    exePath,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    false,
                    0,
                    IntPtr.Zero,
                    null,
                    ref si,
                    out pi
                );

                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    EventLog.WriteEntry($"CreateProcessAsUser failed: {error}", EventLogEntryType.Error);
                }
            }
        }

        // Gerekli API ve yapılar:
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint WTSGetActiveConsoleSessionId();

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSQueryUserToken(uint sessionId, out IntPtr Token);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            int dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation
        );

        [StructLayout(LayoutKind.Sequential)]
        struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX, dwY, dwXSize, dwYSize;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput, hStdOutput, hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }
    }
}
