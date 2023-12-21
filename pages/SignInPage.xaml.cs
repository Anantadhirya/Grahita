using Grahita.components;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grahita.pages
{
    /// <summary>
    /// Interaction logic for SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        Action<User> SignIn;
        Action<MainWindow.Navigation> Navigate;
        public SignInPage(Action<User> SignIn, Action<MainWindow.Navigation> Navigate)
        {
            InitializeComponent();
            this.SignIn = SignIn;
            this.Navigate = Navigate;
        }
        private void setError(TextBlock errorText, string message)
        {
            errorText.Text = message;
            errorText.Visibility = message != "" ? Visibility.Visible : Visibility.Collapsed;
        }
        private void onSignIn(object sender, RoutedEventArgs e)
        {
            setError(UsernameError, "");
            setError(PasswordError, "");
            if (Username.Text == "")
            {
                setError(UsernameError, "Username tidak boleh kosong.");
                return;
            }
            if (Password.Password == "")
            {
                setError(PasswordError, "Password tidak boleh kosong.");
                return;
            }
            using (var db = new GrahitaDBEntities())
            {
                var query = from user in db.Users
                            where user.Name == Username.Text
                            select user;
                if(!query.Any())
                {
                    setError(UsernameError, "Username tidak ditemukan.");
                    return;
                }
                if(!HashHelper.VerifyPassword(Password.Password, query.First().Password))
                {
                    setError(PasswordError, "Password salah.");
                    return;
                }
                SignIn(query.First());
                MessageBox.Show("Sign In berhasil", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void onEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                onSignIn(sender, null);
            }
        }

        private void NavigateRegister(object sender, RoutedEventArgs e)
        {
            Navigate(MainWindow.Navigation.register);
        }
    }
}
