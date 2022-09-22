
using FakeAtlas.Context.UnitOfWork;
using FakeAtlas.Encryption;
using FakeAtlas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FakeAtlasTests
{
    public class ViewModelTests
    {
        /// <summary>
        /// SignInViewModel
        /// </summary>
        [Fact]
        public void SignInCommand_CanExeuteNThrowException_ReturnsTrue()
        {
            SignInViewModel view = new();
            Assert.True(view.SignInCommand.CanExecute(null));
            Assert.Throws<InvalidOperationException>(() => view.SignInCommand.Execute(null));
        }

        /// <summary>
        /// SignUpViewModel
        /// </summary>
        [Fact]
        public void SignUpCommand_CanExeuteNThrowException_ReturnsTrue()
        {
            SignUpViewModel view = new();
            Assert.True(view.SignUpCommand.CanExecute(null));
            Assert.Throws<InvalidOperationException>(() => view.SignUpCommand.Execute(null));
        }
        /// <summary>
        /// Testing sign in to existing admin account
        /// </summary>
        [Fact]
        public void SignIn_SuccessfulAdminExecuteNoException_ReturnsTrue()
        {
            using (UnitOfWork unitOfWork = new())
            {
                Encoding enc = Encoding.UTF8;
                var salt = (from user in unitOfWork.BookingUsers.Get()
                            where user.UserLogin.Equals("admin")
                            select user).Single();
                string result1 =
                    Convert.ToBase64String(AtlasCrypto.
                    GenerateSaltedHash(enc.GetBytes("125478963"), Convert.FromBase64String(salt.Salt)));
                Assert.Equal(result1, salt.UserPassword);
            }
        }

        /// <summary>
        /// SearchViewModel
        /// </summary>
        [Fact]
        public void FindCommand__CanExeute_ReturnsTrue()
        {
            SearchViewModel view = new();
            Assert.True(view.FindCommand.CanExecute(null));
        }

        /// <summary>
        /// SearchViewModel
        /// </summary>
        [Fact]
        public void SelectCommand__CanExeute_ReturnsTrue()
        {
            SearchViewModel view = new();
            Assert.True(view.SelectCommand.CanExecute(null));
        }

        /// <summary>
        /// OrdersViewModel
        /// </summary>
        [Fact]
        public void RemoveOrderCommand_CanExeute_ReturnsTrue()
        {
            OrdersViewModel view = new();
            Assert.True(view.RemoveOrderCommand.CanExecute(null));
        }
        /// <summary>
        /// AdminViewModel
        /// </summary>
        [Fact]
        public void SaveCompanyCommand_CanExeute_ReturnsTrue()
        {
            AdminViewModel view = new();
            Assert.True(view.SaveCompanyCommand.CanExecute(null));
        }
        /// <summary>
        /// AdminViewModel
        /// </summary>
        [Fact]
        public void RemoveCompanyCommand_CanExeute_ReturnsTrue()
        {
            AdminViewModel view = new();
            Assert.True(view.RemoveCompanyCommand.CanExecute(null));
        }

        /// <summary>
        /// AdminViewModel
        /// </summary>
        [Fact]
        public void RemoveCompanyCommand_Execute_ThrowException()
        {
            AdminViewModel view = new();
            Assert.Throws<InvalidOperationException>(() => view.RemoveCompanyCommand.Execute(null));
        }
        /// <summary>
        /// AdminViewModel
        /// </summary>
        [Fact]
        public void Admin_RemoveOrderCommand_Execute_ThrowException()
        {
            AdminViewModel view = new();
            Assert.Throws<InvalidOperationException>(() => view.RemoveOrderCommand.Execute(null));
        }

        /// <summary>
        /// MainWindowViewModel
        /// </summary>
        [Fact]
        public void LogoutCommand__CanExeute_ReturnsTrue()
        {
            MainWindowViewModel view = new();
            Assert.True(view.LogOutCommand.CanExecute(null));
        }

        /// <summary>
        /// MainWindowViewModel
        /// </summary>
        [Fact]
        public void InfoCommand__CanExeute_ReturnsTrue()
        {
            MainWindowViewModel view = new();
            Assert.True(view.InfoCommand.CanExecute(null));
        }
    }
}
