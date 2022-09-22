using FakeAtlas.Context.UnitOfWork;
using FakeAtlas.Models;
using FakeAtlas.ViewModels.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FakeAtlas.Encryption;
using FakeAtlas.Models.Entities;
using Microsoft.VisualStudio.PlatformUI;

namespace FakeAtlas.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {

        private BookingUser _selectedAccount = new();

        public BookingUser SelectedAccount
        {
            get { return _selectedAccount; }
            set 
            { 
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        private string _unsecurePassword;

        public string UnsecurePassword
        {
            get { return _unsecurePassword; }
            set 
            {
                _unsecurePassword = value;
                OnPropertyChanged(nameof(UnsecurePassword));
            }
        }
        public static LoginViewModel LoginView { get; set; }

        public SignInViewModel()
        {
        }

        private ICommand signInCommand;
        public ICommand SignInCommand => signInCommand ??= new DelegateCommand(SignIn);

        private void SignIn()
        {
            try
            {
                using (UnitOfWork unitOfWork = new())
                {
                    Encoding enc = Encoding.UTF8;
                    var salt = (from user in unitOfWork.BookingUsers.Get() where user.UserLogin.Equals(SelectedAccount.UserLogin) select user).Single();
                    byte[] result = AtlasCrypto.GenerateSaltedHash(enc.GetBytes(UnsecurePassword), Convert.FromBase64String(salt.Salt));


                    MainWindowViewModel.User = (from user in unitOfWork.BookingUsers.Get()
                                                where user.UserLogin.Equals(SelectedAccount.UserLogin)
                                                && user.UserPassword.Equals(Convert.ToBase64String(result))
                                                select user).Single();

                    MainWindowViewModel.Address = (from address in unitOfWork.AddressRepository.Get() where address.Id == MainWindowViewModel.User.Id select address).Single();

                    MainWindowFactory mainWindow = new();
                    WindowFactory factory = new(mainWindow);
                    factory.OpenWindow();
                    LoginView.CloseLoginWindow();
                }
            }
            catch (Exception e)
            {
                FakeAtlasMessageBoxService box = new();
                box.ShowMessage(FakeAtlasUIComponents.FakeAtlasMessageBox.MessageType.InvalidPasswordOrLogin, CurrentLocalization);
            }
        }
    }
}
