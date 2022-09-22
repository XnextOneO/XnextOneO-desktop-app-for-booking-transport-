using FakeAtlas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using FakeAtlas.Encryption;
using FakeAtlas.Context.UnitOfWork;
using FakeAtlas.ViewModels.Management;
using System.Windows;
using System.Runtime.CompilerServices;
using FakeAtlas.Models.Entities;
using System.ComponentModel;
using System.Text.RegularExpressions;
using FakeAtlas.FakeAtlasUIComponents;

namespace FakeAtlas.ViewModels
{
    public class SignUpViewModel : ViewModelBase, IDataErrorInfo
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
            set { _unsecurePassword = value; OnPropertyChanged(nameof(UnsecurePassword)); }
        }

        private string _unsecurePasswordRepeat;

        public string UnsecurePasswordRepeat
        {
            get { return _unsecurePasswordRepeat; }
            set { _unsecurePasswordRepeat = value; OnPropertyChanged(nameof(UnsecurePasswordRepeat)); }
        }

        private Visibility errorVisibility = Visibility.Collapsed;

        public Visibility ErrorVisibility 
        {
            get { return errorVisibility; }
            set { errorVisibility = value; OnPropertyChanged(nameof(ErrorVisibility)); }
        }

        private ICommand signUpCommand;
        public ICommand SignUpCommand => signUpCommand ??= new DelegateCommand(SignUp);

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged(nameof(Error)); }
        }


        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case nameof(UnsecurePassword):
                        if (!UnsecurePassword.Equals(UnsecurePasswordRepeat))
                        {
                            error = "Возраст должен быть больше 0 и меньше 100";
                        }
                        break;
                    case nameof(SelectedAccount.UserLogin):
                        //Обработка ошибок для свойства Name
                        break;
                }
                return error;
            }
        }

        private void SignUp()
        {
            FakeAtlasMessageBoxService box = new();
            ErrorVisibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(SelectedAccount.UserLogin)
                || string.IsNullOrEmpty(UnsecurePassword) || string.IsNullOrEmpty(UnsecurePasswordRepeat))
            {
                box.ShowMessage(FakeAtlasMessageBox.MessageType.EmptyStringError, CurrentLocalization);
                return;
            }
            if (!UnsecurePassword.Equals(UnsecurePasswordRepeat))
            {
                ErrorVisibility = Visibility.Visible;
                return;
            }
            
            if (!loginRegex.IsMatch(SelectedAccount.UserLogin))
            {
                box.ShowMessage(FakeAtlasMessageBox.MessageType.FailLoginRegex, CurrentLocalization);
                return;
            }
            if (!passwordRegex.IsMatch(UnsecurePassword))
            {
                box.ShowMessage(FakeAtlasMessageBox.MessageType.FailPasswordRegex, CurrentLocalization);
                return;
            }
            try
            {
                var salt = AtlasCrypto.GetSalt();
                byte[] result = AtlasCrypto.GenerateSaltedHash(Encoding.UTF8.GetBytes(UnsecurePassword), salt);
                using (UnitOfWork unitOfWork = new())
                {
                    var address = new UniqueAddress();
                    unitOfWork.AddressRepository.Create(address);
                    unitOfWork.Save();
                    unitOfWork.BookingUsers.Create(new BookingUser
                    {
                        UserLogin = SelectedAccount.UserLogin,
                        UserPassword = Convert.ToBase64String(result),
                        Salt = Convert.ToBase64String(salt),
                        IdAddress = (from adrs in unitOfWork.AddressRepository.Get() select adrs).Last().Id
                    });
                    unitOfWork.Save();
                    box.ShowMessage(FakeAtlasMessageBox.MessageType.SuccessfulRegistration, CurrentLocalization);
                }
            }
            catch (Exception e)
            {
                box.ShowMessage(e.Message);
            }
        }
    }
}
