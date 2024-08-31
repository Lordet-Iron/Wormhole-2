using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Core;

namespace WpfApp1.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand ArmaViewCommand { get; set; }
        public RelayCommand uwu {  get; set; }
        public HomeViewModel HomeVM { get; set; }

        public ArmaViewModel ArmaVM { get; set; }

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
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });
            ArmaViewCommand = new RelayCommand(o =>
            {
                CurrentView = ArmaVM;
            });
            uwu = new RelayCommand(o =>
            {
                Console.WriteLine("aergerg");
            });
        }
    }
}
