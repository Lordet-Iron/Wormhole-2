using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Installers
{
    internal class ZeroConfiguration
    {
        static void main(string[] args)
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = "zerotier-cli",   // Make sure zerotier-cli is in your PATH or provide the full path
                    Arguments = "status",        // ZeroTier command to execute
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true        // Do not show a console window
                };

                using (Process process = Process.Start(processInfo))
                {
                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (process.ExitCode == 0)
                    {
                        Console.WriteLine("Command succeeded: " + output);
                    }
                    else
                    {
                        Console.WriteLine("Command failed: " + error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing ZeroTier command: " + ex.Message);
            }
        }
    }
}
