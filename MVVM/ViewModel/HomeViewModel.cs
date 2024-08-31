using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Core;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace WpfApp1.MVVM.ViewModel
{
    internal class HomeViewModel : ObservableObject
    {
        public RelayCommand InstallTS { get; set; }


        public HomeViewModel()
        {
            InstallTS = new RelayCommand(o =>
            {
                // Extract and save Program1.exe
                ExtractResource("WpfApp1.Installers.Program1.exe", "Program1.exe");

                // Extract and save Program2.exe
                ExtractResource("WpfApp1.Installers.Program2.ts3_plugin", "Program2.ts3_plugin");

                // Run the extracted programs
                RunProgram("Program1.exe");

                Console.WriteLine("Waiting 5 seconds");
                Thread.Sleep(5000); // Pause for 5000 milliseconds (5 seconds)

                OpenFileWithDefaultApplication("Program2.ts3_plugin");

                Console.WriteLine("Complete");
            });
        }

        public void ExtractResource(string resourceName, string outputPath)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new ArgumentException("Resource not found: " + resourceName);

                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }

        public void RunProgram(string filePath)
        {
            Process process = new Process();
            process.StartInfo.FileName = filePath;
            process.Start();
            process.WaitForExit();  // Wait for the process to exit
        }

        public void OpenFileWithDefaultApplication(string filePath)
        {
            Process process = new Process();
            process.StartInfo.FileName = filePath;
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();  // Wait for the process to exit
        }
    }
}
