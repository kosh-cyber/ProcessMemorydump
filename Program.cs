using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace ProcessMemorydump
{
    class Program
    {
        [DllImport("Dbghelp.dll")]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, int ProcessId,IntPtr hFile, int DumpType, IntPtr ExceptionParam,IntPtr UserStreamParam, IntPtr CallbackParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle,int processId);


        static void Main(string[] args)
        {
            if (args.Length < 1) {
                Console.WriteLine("Usage:"+AppDomain.CurrentDomain.FriendlyName + " processname " + "path" );
                System.Environment.Exit(1);
            }
            string processname = args[0];
            string dir = args[1];

            Process[] pi = Process.GetProcessesByName(processname);
            int processname_pid = pi[0].Id;

            IntPtr handle = OpenProcess(0x001F0FFF, false, processname_pid);

            string pathfile = dir + processname + ".dmp";

            FileStream dumpFile = new FileStream(pathfile, FileMode.Create);

            bool dumped = MiniDumpWriteDump(handle, processname_pid, dumpFile.SafeFileHandle.DangerousGetHandle(), 2, IntPtr.Zero, IntPtr.Zero,IntPtr.Zero);

            Console.WriteLine("Dump " + processname +" Process Memory is " + dumped);
        }
    }
}
