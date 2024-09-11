using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Core;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand ArmaViewCommand { get; set; }
        public RelayCommand ZeroViewCommand { get; set; }
        public RelayCommand uwu {  get; set; }
        public RelayCommand Close {  get; set; }
        public RelayCommand Minimise { get; set; }
        public HomeViewModel HomeVM { get; set; }

        public ArmaViewModel ArmaVM { get; set; }
        public ZeroViewModel ZeroVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel() 
        { 
            HomeVM = new HomeViewModel();
            ArmaVM = new ArmaViewModel();
            ZeroVM = new ZeroViewModel();
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });
            ArmaViewCommand = new RelayCommand(o =>
            {
                CurrentView = ArmaVM;
            });
            ZeroViewCommand = new RelayCommand(o =>
            {
                CurrentView = ZeroVM;
            });
            uwu = new RelayCommand(o =>
            {
                Console.WriteLine("aergerg");
            });
            Close = new RelayCommand(o =>
            {
                Application.Current.Shutdown();
            });
            Minimise = new RelayCommand(o =>
            {
                if (o is Window window)
                {
                    window.WindowState = WindowState.Minimized;
                }
            });
        }
    }
}
