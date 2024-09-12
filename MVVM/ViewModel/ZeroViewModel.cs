using System;
using WpfApp1.Core;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfApp1.MVVM.ViewModel
{
    internal class ZeroViewModel : ObservableObject
    {
        public RelayCommand InstallZeroTier { get; set; }
        public RelayCommand UninstallZeroTier { get; set; }

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



        public ZeroViewModel()
        {
            InstallIsEnabled = true;
            InstallIsNotEnabled = false;
            UninstallIsEnabled = true;
            UninstallIsNotEnabled = false;
            InstallZeroTier = new RelayCommand(async o =>
            {
                InstallIsEnabled = false;
                InstallIsNotEnabled = true;

                // Extract and save Program2.exe
                ExtractResource("WpfApp1.Installers.ZeroTier One.msi", "ZeroTier One.msi");

                OpenFileWithDefaultApplication("ZeroTier One.msi");

                Console.WriteLine("Zero Installed");

                // Get the local path (relative to the WPF application executable)
                ExtractResource("WpfApp1.Installers.ZeroTierConfigurator.ZeroTier Configurator.deps.json", "ZeroTier Configurator.deps.json");
                ExtractResource("WpfApp1.Installers.ZeroTierConfigurator.ZeroTier Configurator.dll", "ZeroTier Configurator.dll");
                ExtractResource("WpfApp1.Installers.ZeroTierConfigurator.ZeroTier Configurator.exe", "ZeroTierConfig.exe");
                ExtractResource("WpfApp1.Installers.ZeroTierConfigurator.ZeroTier Configurator.pdb", "ZeroTier Configurator.pdb");
                ExtractResource("WpfApp1.Installers.ZeroTierConfigurator.ZeroTier Configurator.runtimeconfig.json", "ZeroTier Configurator.runtimeconfig.json");
                string localPath = "ZeroTierConfig.exe";
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = localPath,  // Full path to your console app
                    Verb = "runas",  // This ensures the process runs as an admin
                    UseShellExecute = true  // Allows the UAC prompt to appear
                };

                try
                {
                    Process.Start(processInfo);  // This triggers the UAC and runs the console app as admin
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    // The user canceled the UAC prompt or an error occurred
                    MessageBox.Show("The command requires administrative privileges.");
                }

                InstallIsEnabled = true;
                InstallIsNotEnabled = false;
            });

            UninstallZeroTier = new RelayCommand(async o =>
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
