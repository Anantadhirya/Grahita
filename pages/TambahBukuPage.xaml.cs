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
    /// Interaction logic for TambahBukuPage.xaml
    /// </summary>
    public partial class TambahBukuPage : Page
    {
        private User user;
        private Action<MainWindow.Navigation> Navigate;
        private string imagePath = "";
        public TambahBukuPage(User user, Action<MainWindow.Navigation> Navigate)
        {
            InitializeComponent();
            this.user = user;
            this.Navigate = Navigate;
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
        private void onTambahGambar(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                Gambar.Source = new BitmapImage(new Uri(imagePath));
            }
        }
        private async void onTambahBuku(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            check(Judul.Text, ref JudulError, "Judul", ref valid);
            check(Author.Text, ref AuthorError, "Author", ref valid);
            check(imagePath, ref ImageError, "Gambar", ref valid);
            if (valid)
            {
                using (var db = new GrahitaDBEntities())
                {
                    string imageUrl = await BlobUploader.Main(imagePath);
                    var book = new Book { Title = Judul.Text, Author = Author.Text, Image = imageUrl, Owner = user.Id, Available = true };
                    db.Books.Add(book);
                    db.SaveChanges();
                    Navigate(MainWindow.Navigation.dashboard);
                    MessageBox.Show("Penambahan buku berhasil", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
