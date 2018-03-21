using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AIOFlipper
{
    public static class SetWindowPosition
    {
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll")]
        private static extern bool LockSetForegroundWindow(uint uLockCode);
        private const UInt32 LSFW_LOCK = 1;

        public static void ForceWindowToStayOnBottom(Process process)
        {
            SetWindowPos(
                process.MainWindowHandle, // The handle of the browser window
                HWND_BOTTOM, // Tells it the position, in this case the bottom
                0, 0, 0, 0, // Coordinates for sizing of the window - Will be overriden by NOSIZE
                SWP_NOSIZE | // Says to keep the window its current size
                SWP_NOMOVE | // Says to keep the window in its current spot
                SWP_NOACTIVATE // Activation brings the window to the top, we don't want that
            );

            // If you don't notice this helping, it can probably be deleted.
            // It only deals with other applications and gets automatically undone on user interaction
            LockSetForegroundWindow(
                LSFW_LOCK); // Locks calls to SetForegroundWindow 
        }

        /// <returns> Returns the parent process of each process by the name of processName </returns>
        public static List<Process> GetPrimaryProcesses(string processName) =>

            Process.GetProcesses() // Gets a list of every process on computer
                .Where(process => process.ProcessName.Contains(processName) // Reduces the list to every process by the name we are looking for
                    && process.MainWindowHandle != IntPtr.Zero) // Removes any process without a MainWindow (which amounts to every child process)
                .ToList();
    }
}
