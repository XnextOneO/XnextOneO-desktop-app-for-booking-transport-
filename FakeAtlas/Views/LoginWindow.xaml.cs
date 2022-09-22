using FakeAtlas.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Net.Mail;
using FakeAtlas.Services;

namespace FakeAtlas.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(email.Text))
            {
                MessageBox.Show("Введите Email");
                return;
            }

            ValidatorEmail validatorEmail = new ValidatorEmail();
            FluentValidation.Results.ValidationResult validationResult = validatorEmail.Validate(email.Text);
            if (!validationResult.IsValid)
            {
                MessageBox.Show(validationResult.Errors[0].ErrorMessage);
                return;
            }

            MailAddress fromadress = new MailAddress("xnextone@gmail.com", "FAKEATLAS");
            MailAddress toadress = new MailAddress(email.Text);
            MailMessage message = new MailMessage(fromadress, toadress);
            message.Body = "Ваш временный аккаунт Login: Temporyaccaunt123 Password: 123321asd";
            message.Subject = "привет";

            SmtpClient smptpClient = new SmtpClient();
            smptpClient.Host = "smtp.gmail.com";
            smptpClient.Port = 587;
            smptpClient.EnableSsl = true;
            smptpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptpClient.UseDefaultCredentials = false;
            smptpClient.Credentials = new NetworkCredential(fromadress.Address, "123321ZXCqweasd/");


            smptpClient.Send(message);

            MessageBox.Show("Проверьте email");

        }
    }
}