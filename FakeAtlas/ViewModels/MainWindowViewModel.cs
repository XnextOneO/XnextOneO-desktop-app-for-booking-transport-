using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using FakeAtlas.ViewModels.Management;
using FakeAtlas.Models.Entities;

namespace FakeAtlas.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Target current user
        private static BookingUser _user = new();

        public static BookingUser User { get => _user; set => _user = value; }

        private static UniqueAddress _address = new();

        public static UniqueAddress Address { get => _address; set => _address = value; }
        #endregion

        private ViewModelBase _selectedViewModel = new BookingUserViewModel();

        public ViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set 
            { 
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        private Visibility _adminVisibility = Visibility.Collapsed;

        public Visibility AdminVisibility
        {
            get { return _adminVisibility; }
            set { _adminVisibility = value; }
        }

        public MainWindowViewModel()
        {
            if (User.IsAdmin)
            {
                AdminVisibility = Visibility.Visible;
            }
        }

        private ICommand findCommand;
        public ICommand FindCommand => findCommand ??= new DelegateCommand(Find);

        private void Find() => SelectedViewModel = new SearchViewModel();

        private ICommand logOutCommand;
        public ICommand LogOutCommand => logOutCommand ??= new DelegateCommand(LogOut);

        private void LogOut()
        {
            User = null;
            Address = null;
            LoginWindowFactory window = new();
            WindowFactory factory = new(window);
            factory.OpenWindow();
            CloseWindow();
        }

        private ICommand infoCommand;
        public ICommand InfoCommand => infoCommand ??= new DelegateCommand(Info);

        private void Info() => SelectedViewModel = new BookingUserViewModel();

        private ICommand ordersCommand;
        public ICommand OrdersCommand => ordersCommand ??= new DelegateCommand(Orders);

        private void Orders() => SelectedViewModel = new OrdersViewModel();

        private ICommand helpCommand;
        public ICommand HelpCommand => helpCommand ??= new DelegateCommand(Help);

        private void Help() => SelectedViewModel = new HelpViewModel();

        private ICommand shipperCommand;
        public ICommand ShipperCommand => shipperCommand ??= new DelegateCommand(Shipper);

        private void Shipper() => SelectedViewModel = new AdminViewModel();
    }
}
