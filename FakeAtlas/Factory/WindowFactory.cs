using FakeAtlas.FakeAtlasUIComponents;
using FakeAtlas.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FakeAtlas.ViewModels.Management
{

    #region factory implemintation to prevert mvvm violation 
    public class WindowFactory
    {
        private readonly IWindowFactory m_windowFactory;
        private ICommand m_openNewWindow;

        public WindowFactory(IWindowFactory windowFactory)
        {
            m_windowFactory = windowFactory;
            m_openNewWindow = null;
        }

        public void OpenWindow()
        {
            m_windowFactory.CreateNewWindow();
        }

        public ICommand OpenWindowCommand { get { return m_openNewWindow; } }
    }

    public interface IWindowFactory
    {
        void CreateNewWindow();
    }
    #endregion

    #region specific factory implemintation
    public class MainWindowFactory : IWindowFactory
    {
        public void CreateNewWindow()
        {
            MainWindow window = new()
            {
                DataContext = new MainWindowViewModel()
            };
            window.Show();
        }
    }
    public class LoginWindowFactory : IWindowFactory
    {
        public void CreateNewWindow()
        {
            LoginWindow window = new()
            {
                DataContext = new LoginViewModel()
            };
            window.Show();
        }
    }
    #endregion
}
