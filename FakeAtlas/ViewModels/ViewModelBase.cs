using FakeAtlas.ViewModels.Management;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace FakeAtlas.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, ICloseWindow, IMinimizeWindow, IMaximizeWindow, IRestoreWindow
    {
        /// <summary>
        /// Sign in/up and password changing validation
        /// </summary>
        protected static Regex loginRegex = new(@"[a-zA-Z0-9]+([_ -]?[a-zA-Z0-9]){5,20}$");
        protected static Regex passwordRegex = new(@"(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}");

        /// <summary>
        /// Localization capture enum
        /// </summary>
        public enum Localization
        {
            ru_RU = 0x002,
            en_GB = 0x004
        }

        public static Localization CurrentLocalization { get; set; } = Thread.CurrentThread.CurrentCulture.Name == "ru-RU" ? Localization.ru_RU : Localization.en_GB;

        public ViewModelBase()
        {
            
        }

        /// <summary>
        /// A base view model that fires Property Changed events as needed
        /// </summary>      
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private Visibility restoreVisibility = Visibility.Collapsed;
        private Visibility maximizeVisibility = Visibility.Visible;

        public Visibility RestoreVisibility
        {
            get { return restoreVisibility; }
            set
            {
                restoreVisibility = value;
                OnPropertyChanged(nameof(RestoreVisibility));
            }
        }
        public Visibility MaximizeVisibility
        {
            get { return maximizeVisibility; }
            set
            {
                maximizeVisibility = value;
                OnPropertyChanged(nameof(MaximizeVisibility));
            }
        }

        private WindowState _currentState = WindowState.Normal;  

        public WindowState CurrentState
        {
            get { return _currentState; }
            set 
            { 
                _currentState = value;
                if (_currentState == WindowState.Maximized) 
                {
                    RestoreVisibility = Visibility.Visible;
                    MaximizeVisibility = Visibility.Collapsed;
                }
                else if(_currentState == WindowState.Normal)
                {
                    RestoreVisibility = Visibility.Collapsed;
                    MaximizeVisibility = Visibility.Visible;
                }
                OnPropertyChanged(nameof(CurrentState)); 

            }
        }

        #region Basic Commands


        private ICommand select_enGB_Command;
        public ICommand Select_enGB_Command => select_enGB_Command ??= new DelegateCommand(Select_enGB_);

        private void Select_enGB_()
        {
            var rsrc = "Localization/lang.en-GB.xaml";
            var currentRsrc = new Uri(rsrc, UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = currentRsrc };
            CurrentLocalization = Localization.en_GB;
        }

        private ICommand select_ruRU_Command;
        public ICommand Select_ruRU_Command => select_ruRU_Command ??= new DelegateCommand(Select_ruRU_);

        private void Select_ruRU_()
        {
            var rsrc = "Localization/lang.ru-RU.xaml";
            var currentRsrc = new Uri(rsrc, UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary() { Source = currentRsrc };
            CurrentLocalization = Localization.ru_RU;
        }
        private ICommand _closeCommand;

        public ICommand CloseCommand => _closeCommand ??= new DelegateCommand(CloseWindow);

        protected void CloseWindow() => Close?.Invoke();

        public bool CanClose() => true;

        public Action Close { get; set; }

        private ICommand _minimizeCommand;

        public ICommand MinimizeCommand => _minimizeCommand ??= new DelegateCommand(MinimizeWindow);

        private void MinimizeWindow() => Minimize?.Invoke();

        public bool CanMinimize() => true;

        public Action Maximize { get; set; }

        private ICommand _maximizeCommand;

        public ICommand MaximizeCommand => _maximizeCommand ??= new DelegateCommand(MaximizeWindow);

        private void MaximizeWindow()
        {
            Maximize?.Invoke();
            RestoreVisibility = Visibility.Visible;
            MaximizeVisibility = Visibility.Collapsed;
        }
        public bool CanMaximize() => true;

        public Action Minimize { get; set; }

        public Action Restore { get; set; }

        private ICommand _restoreCommand;

        public ICommand RestoreCommand => _restoreCommand ??= new DelegateCommand(RestoreWindow);

        private void RestoreWindow()
        {
            Restore?.Invoke();

            RestoreVisibility = Visibility.Collapsed;
            MaximizeVisibility = Visibility.Visible;
        }

        public bool CanRestore() => true;

        #endregion
    }
}
