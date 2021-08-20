using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace PRSpline
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Process[] myProcesses = Process.GetProcessesByName("PRSpline");
            if (!(myProcesses.Length > 1))
            {
                Application.Run(new frmMain());
            }
            else
            {

                Process pCurrentProcess = Process.GetCurrentProcess();

                for (int ii = 0; ii < myProcesses.Length; ii++)
                {
                    if (myProcesses[ii].Id != pCurrentProcess.Id)
                        myProcesses[ii].Kill();
                }

                Application.Run(new frmMain());
            }
        }
    }
}
