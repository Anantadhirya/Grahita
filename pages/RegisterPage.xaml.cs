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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        private void check(string text, ref TextBlock errorText, string type, ref bool valid, string password = "")
        {
            string errorMessage;
            if (text == "")
            {
                errorMessage = type + " tidak boleh kosong.";
            }
            else if (type == "Confirm Password" && text != password)
            {
                errorMessage = "Password tidak sesuai.";
            }
            else
            {
                errorMessage = "";
            }

            if(errorMessage != "")
            {
                valid = false;
                errorText.Text = errorMessage;
                errorText.Visibility = Visibility.Visible;
            } else
            {
                errorText.Text = "";
                errorText.Visibility = Visibility.Collapsed;
            }
        }
        private void onRegister(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            check(Username.Text, ref UsernameError, "Username", ref valid);
            check(Password.Password, ref PasswordError, "Password", ref valid);
            check(ConfirmPassword.Password, ref ConfirmPasswordError, "Confirm Password", ref valid, Password.Password);
            check(Kontak.Text, ref KontakError, "Kontak", ref valid);
            check(Alamat.Text, ref AlamatError, "Alamat", ref valid);
            if(valid)
            {
                using (var db = new GrahitaDBEntities())
                {
                    var user = new User { Name = Username.Text, Password = Password.Password, Contact = Kontak.Text, Location = Alamat.Text };
                    db.Users.Add(user);
                    db.SaveChanges();
                    MessageBox.Show("Pendaftaran berhasil", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
