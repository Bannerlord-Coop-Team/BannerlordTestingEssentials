using EnvDTE;
using Microsoft.VisualStudio.Setup.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BannerlordSystemTestingLibrary
{
    public static class ProcessExtentions
    {

        /// <summary>
        ///     Attaches Visual Studio to the specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="maxTries">The number of tries to get the process.</param>
        public static void Attach(this System.Diagnostics.Process process, int maxTries = 5)
		{
			// Reference visual studio core
			DTE dte = null;
			int version = 9;
			while (version < 20 && dte == null)
			{
				try
				{
                    version++;
                    dte = (DTE)Marshal.GetActiveObject($"VisualStudio.DTE.{version}.0");
                }
				catch (COMException)
				{
#if DEBUG
					Console.WriteLine(String.Format("Visual studio {0} not found.", version));
#endif
				}
			}

			if (dte == null)
			{
				Console.WriteLine("No debugger found, nothing attached...");
				return;
			}

            // Try loop - visual studio may not respond the first time.
            // We also don't want it to stall the main thread
            new System.Threading.Thread(() =>
			{
				while (maxTries-- > 0)
				{
					try
					{
						Processes processes = dte.Debugger.LocalProcesses;
						foreach (EnvDTE.Process proc in processes)
						{
                            try
							{
								if (proc.Name.Contains(process.ProcessName))
								{
									proc.Attach();
#if DEBUG
									Console.WriteLine(String.Format("Attached to process {0} successfully.", process.ProcessName));
#endif
									return;
								}
							}
							catch { }
						}
					}
					catch { }
					// Wait for debugger and application and debugger to find application
					System.Threading.Thread.Sleep(1500);
				}
			}).Start();
		}
	}
}
