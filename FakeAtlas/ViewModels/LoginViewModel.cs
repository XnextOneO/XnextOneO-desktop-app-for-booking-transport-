using FakeAtlas.Context.Repository;
using FakeAtlas.Context.UnitOfWork;
using FakeAtlas.ViewModels.Management;
using FakeAtlas.Views;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FakeAtlas.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        private Visibility _isToSignUpVisible = Visibility.Visible;
        private Visibility _isToSignInVisible = Visibility.Collapsed;


        private ViewModelBase _selectedViewModel;

        public ViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set 
            { 
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
               
            }
        }

        #region Bindings


        public Visibility ToSignUpVisibility
        {
            get { return _isToSignUpVisible; }
            set
            {
                _isToSignUpVisible = value;
                OnPropertyChanged(nameof(ToSignUpVisibility));
            }
        }
        public Visibility ToSignInVisibility
        {
            get { return _isToSignInVisible; }
            set
            {
                _isToSignInVisible = value;
                OnPropertyChanged(nameof(ToSignInVisibility));
            }
        }

        public void CloseLoginWindow() => CloseWindow();

        #endregion

        public LoginViewModel()
        {
            _selectedViewModel = new SignInViewModel();
            SignInViewModel.LoginView = this;
        }

        #region Commands

        private ICommand _signInCommand;
        public ICommand SignInCommand => _signInCommand ??= new DelegateCommand(signInClick);
        private ICommand _signUpCommand;
        public ICommand SignUpCommand => _signUpCommand ??= new DelegateCommand(signUpClick);

        private void signInClick()
        {
            ToSignUpVisibility = Visibility.Visible;
            ToSignInVisibility = Visibility.Collapsed;
            SelectedViewModel = new SignInViewModel();
        }

        public ICommand sSignUpCommand { get; set; }

        private void signUpClick()
        {
            SelectedViewModel = new SignUpViewModel();
            ToSignUpVisibility = Visibility.Collapsed;
            ToSignInVisibility = Visibility.Visible;
        }
        #endregion
    }
}
