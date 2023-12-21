using Grahita.components;
using Microsoft.Win32;
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
    /// Interaction logic for EditProfilePage.xaml
    /// </summary>
    public partial class EditProfilePage : Page
    {
        User user;
        private string imagePath = "";
        private Action<MainWindow.Navigation> Navigate;
        private Action<User> UpdateSignIn;
        public EditProfilePage(User user, Action<MainWindow.Navigation> Navigate, Action<User> UpdateSignIn)
        {
            InitializeComponent();
            this.user = user;
            this.Navigate = Navigate;
            this.UpdateSignIn = UpdateSignIn;
            initDefaultValue();
        }
        public void initDefaultValue()
        {
            Username.Text = user.Name;
            Kontak.Text = user.Contact;
            Alamat.Text = user.Location;
            Gambar.Source = new BitmapImage(new Uri(user.Image != null ? user.Image : "pack://application:,,,/public/images/blank_profile.jpg"));
        }

        private void check(string text, ref TextBlock errorText, string type, ref bool valid, string password = "")
        {
            string errorMessage = "";
            if (text == "")
            {
                errorMessage = type + " tidak boleh kosong.";
            }
            errorText.Text = errorMessage;
            errorText.Visibility = errorMessage != "" ? Visibility.Visible : Visibility.Collapsed;
            if (errorMessage != "") valid = false;
        }
        private void onUbahGambar(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                Gambar.Source = new BitmapImage(new Uri(imagePath));
            }
        }
        private void onHapusGambar(object sender, RoutedEventArgs e)
        {
            imagePath = null;
            Gambar.Source = new BitmapImage(new Uri("pack://application:,,,/public/images/blank_profile.jpg"));
        }
        private async void onSave(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            check(Username.Text, ref UsernameError, "Username", ref valid);
            check(Kontak.Text, ref KontakError, "Nomor Telepon", ref valid);
            check(Alamat.Text, ref AlamatError, "Alamat", ref valid);
            if (valid)
            {
                using (var db = new GrahitaDBEntities())
                {
                    var dbUser = db.Users.Find(user.Id);
                    dbUser.Name = Username.Text;
                    dbUser.Contact = Kontak.Text;
                    dbUser.Location = Alamat.Text;
                    if(imagePath == null)
                    {
                        dbUser.Image = null;
                    } else if (imagePath != "")
                    {
                        string imageUrl = await BlobUploader.Main(imagePath);
                        dbUser.Image = imageUrl;
                    }
                    db.SaveChanges();
                    MessageBox.Show("Perubahan Disimpan", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateSignIn(dbUser);
                }
            }
        }
        private void HapusAkun(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus akun Anda?", "Penghapusan Akun", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new GrahitaDBEntities())
                {
                    var userToDelete = db.Users.Find(user.Id);
                    db.Users.Remove(userToDelete);

                    var booksToDelete = db.Books.Where(book => book.Owner == user.Id);
                    db.Books.RemoveRange(booksToDelete);

                    db.SaveChanges();
                    Navigate(MainWindow.Navigation.buku);
                    MessageBox.Show("Akun berhasil dihapus", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
