using System;
using WpfApp1.Core;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace WpfApp1.MVVM.ViewModel
{
    internal class ArmaViewModel : ObservableObject
    {
        public RelayCommand InstallTS { get; set; }
        public RelayCommand UninstallTS { get; set; }

        private bool _installIsEnabled;

        public bool InstallIsEnabled
        {
            get => _installIsEnabled;
            set
            {
                _installIsEnabled = value;
                OnPropertyChanged(nameof(InstallIsEnabled));
            }
        }

        private bool _installIsNotEnabled;

        public bool InstallIsNotEnabled
        {
            get => _installIsNotEnabled;
            set
            {
                _installIsNotEnabled = value;
                OnPropertyChanged(nameof(InstallIsNotEnabled));
            }
        }

        private bool _uninstallIsEnabled;

        public bool UninstallIsEnabled
        {
            get => _uninstallIsEnabled;
            set
            {
                _uninstallIsEnabled = value;
                OnPropertyChanged(nameof(UninstallIsEnabled));
            }
        }

        private bool _uninstallIsNotEnabled;

        public bool UninstallIsNotEnabled
        {
            get => _uninstallIsNotEnabled;
            set
            {
                _uninstallIsNotEnabled = value;
                OnPropertyChanged(nameof(UninstallIsNotEnabled));
            }
        }



        public ArmaViewModel()
        {
            InstallIsEnabled = true;
            InstallIsNotEnabled = false;
            UninstallIsEnabled = true;
            UninstallIsNotEnabled = false;
            InstallTS = new RelayCommand(async o =>
            {
                InstallIsEnabled = false;
                InstallIsNotEnabled = true;

                string tsURL = "https://files.teamspeak-services.com/releases/client/3.6.2/TeamSpeak3-Client-win64-3.6.2.exe";
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string destinationFilePath = Path.Combine(currentDirectory, "teamspeak-client.exe");

                await Download(tsURL, destinationFilePath);

                RunProgram(destinationFilePath);

                // Extract and save Program2.exe
                ExtractResource("WpfApp1.Installers.Program2.ts3_plugin", "Program2.ts3_plugin");

                // Run the extracted programs
                Console.WriteLine("Waiting 5 seconds");
                Thread.Sleep(5000); // Pause for 5000 milliseconds (5 seconds)

                OpenFileWithDefaultApplication("Program2.ts3_plugin");

                Console.WriteLine("Complete");

                InstallIsEnabled = true;
                InstallIsNotEnabled = false;
            });

            UninstallTS = new RelayCommand(async o =>
            {
                UninstallIsEnabled = false;
                UninstallIsNotEnabled = true;

                // Extract and save Program2.exe
                ExtractResource("WpfApp1.Installers.TSUninstall.exe", "TSUninstall.exe");

                OpenFileWithDefaultApplication("TSUninstall.exe");

                UninstallIsEnabled = true;
                UninstallIsNotEnabled = false;
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

        public async Task Download(string fileURL, string destinationPath)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Send a request to download the file
                    HttpResponseMessage response = await client.GetAsync(fileURL);
                    response.EnsureSuccessStatusCode(); // Throw an exception if the HTTP request fails

                    // Read the response content as a stream
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                          fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }

                    Console.WriteLine("File downloaded successfully to " + destinationPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
