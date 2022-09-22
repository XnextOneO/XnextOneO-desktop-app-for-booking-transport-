using FakeAtlas.FakeAtlasUIComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FakeAtlas.ViewModels.Management
{
    #region preventing mvvm violation custom messagebox service
    interface IMessageBoxService
    {
        bool ShowMessage(string text);
        bool Show();
        bool ShowMessage(FakeAtlasMessageBox.MessageType type, ViewModelBase.Localization localization);
    }

    class FakeAtlasMessageBoxService : IMessageBoxService
    {
        public bool Show() => new FakeAtlasMessageBox().Show() == MessageBoxResult.OK;

        public bool ShowMessage(string text) => new FakeAtlasMessageBox().Show(text) == MessageBoxResult.OK;

        public bool ShowMessage(FakeAtlasMessageBox.MessageType type, ViewModelBase.Localization localization) => new FakeAtlasMessageBox().Show(type, localization) == MessageBoxResult.OK;
    }
    #endregion
}
