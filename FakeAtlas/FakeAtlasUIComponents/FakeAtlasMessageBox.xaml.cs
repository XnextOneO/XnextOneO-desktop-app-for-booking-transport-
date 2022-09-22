using FakeAtlas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FakeAtlas.FakeAtlasUIComponents
{
    /// <summary>
    /// Логика взаимодействия для FakeAtlasMessageBox.xaml
    /// </summary>
    public partial class FakeAtlasMessageBox : Window
    {
        MessageBoxResult Result = MessageBoxResult.None;

        public enum MessageType
        {
            EmptyAccountString = 0x000,
            EmptyPasskeyString = 0x002,
            InvalidPasswordOrLogin = 0x004,
            DeleteOrderError = 0x006,
            DeleteCompanyError = 0x008,
            EmptyStringError = 0x010,
            SuccessfulRegistration = 0x012,
            AlrdyExistingOrder = 0x014,
            FailPasswordRegex = 0x016,
            FailLoginRegex = 0x018,
            InvalidNumber = 0x020
        }

        public class Message
        {
            public MessageType Type { get; set; }
            public ViewModelBase.Localization CurrentLocalization { get; set; }
            public string Value { get; set; }

            public Message(MessageType type, ViewModelBase.Localization currentLocalization, string value)
            {
                Type = type;
                CurrentLocalization = currentLocalization;
                Value = value;
            }
        }

        IList<Message> messages = new List<Message>()
        {
            new Message(MessageType.EmptyAccountString, ViewModelBase.Localization.en_GB, "Account field cannot be empty"),
            new Message(MessageType.EmptyPasskeyString, ViewModelBase.Localization.en_GB, "Password field cannot be empty"),
            new Message(MessageType.EmptyStringError, ViewModelBase.Localization.en_GB,   "Data field cannot be empty"),
            new Message(MessageType.SuccessfulRegistration, ViewModelBase.Localization.en_GB, "Your account has been signed up!"),
            new Message(MessageType.InvalidPasswordOrLogin, ViewModelBase.Localization.en_GB, "Wrong login or password"),
            new Message(MessageType.DeleteOrderError, ViewModelBase.Localization.en_GB, "Select order before deleting"),
            new Message(MessageType.DeleteCompanyError, ViewModelBase.Localization.en_GB, "Select company before deleting"),
            new Message(MessageType.AlrdyExistingOrder, ViewModelBase.Localization.en_GB, "You already have this order"),
            new Message(MessageType.FailLoginRegex, ViewModelBase.Localization.en_GB, "You cannot create a user using this login"),
            new Message(MessageType.FailPasswordRegex, ViewModelBase.Localization.en_GB, "You cannot create a user using this password"),
            new Message(MessageType.InvalidNumber, ViewModelBase.Localization.en_GB, "Invalid number"),

            new Message(MessageType.EmptyAccountString, ViewModelBase.Localization.ru_RU, "Поле аккаунта не может быть пустым"),
            new Message(MessageType.EmptyPasskeyString, ViewModelBase.Localization.ru_RU, "Поле пароля не может быть пустым"),
            new Message(MessageType.EmptyStringError, ViewModelBase.Localization.ru_RU,   "Поле данных не может быть пустым"),
            new Message(MessageType.SuccessfulRegistration, ViewModelBase.Localization.ru_RU, "Ваш аккаунт был зарегистрирован!"),
            new Message(MessageType.InvalidPasswordOrLogin, ViewModelBase.Localization.ru_RU, "Неверный логин или пароль"),
            new Message(MessageType.DeleteOrderError, ViewModelBase.Localization.ru_RU, "Выберите заказ перед удалением"),
            new Message(MessageType.DeleteCompanyError, ViewModelBase.Localization.ru_RU, "Выберите компанию перед удалением"),
            new Message(MessageType.AlrdyExistingOrder, ViewModelBase.Localization.ru_RU, "У вас уже есть этот заказ"),
            new Message(MessageType.FailLoginRegex, ViewModelBase.Localization.ru_RU, "Вы не можете создать пользователя с таким логином"),
            new Message(MessageType.FailPasswordRegex, ViewModelBase.Localization.ru_RU, "Вы не можете создать пользователя с таким паролем"),
            new Message(MessageType.InvalidNumber, ViewModelBase.Localization.ru_RU, "Введите другое число")
        };
        

        public FakeAtlasMessageBox()
        {
            InitializeComponent();
        }

        public new MessageBoxResult Show()
        {
            ShowDialog();
            return Result;
        }

        public MessageBoxResult Show(string message)
        {
            tbMessage.Text = message;
            tbMessage.FontSize = 14;
            ShowDialog();
            return Result;
        }

        public MessageBoxResult Show(MessageType messageType, ViewModelBase.Localization localization)
        {
            try
            {
                var mess = (from item in messages
                            where item.Type == messageType && item.CurrentLocalization == localization
                            select item).SingleOrDefault();
                tbMessage.Text = mess.Value;
                tbMessage.FontSize = 14;
                ShowDialog();
                return Result;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return Result;
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) 
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) 
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }
    }
}
