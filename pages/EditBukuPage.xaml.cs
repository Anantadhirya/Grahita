using Microsoft.Win32;
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
using System.Drawing;

namespace Grahita.pages
{
    /// <summary>
    /// Interaction logic for EditBukuPage.xaml
    /// </summary>
    public partial class EditBukuPage : Page
    {
        Book book;
        private string imagePath = "";
        private Action<MainWindow.Navigation> Navigate;

        public EditBukuPage(Book book, Action<MainWindow.Navigation> navigate)
        {
            InitializeComponent();
            this.book = book;
            initDefaultValue();
            Navigate = navigate;
        }

        public void initDefaultValue()
        {
            Judul.Text = book.Title;
            Author.Text = book.Author;
            Gambar.Source = new ImageSourceConverter().ConvertFromString(book.Image) as ImageSource;
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
        private async void onSave(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            check(Judul.Text, ref JudulError, "Judul", ref valid);
            check(Author.Text, ref AuthorError, "Author", ref valid);
            if (valid)
            {
                using (var db = new GrahitaDBEntities())
                {
                    var dbBook = db.Books.Find(book.Id);
                    dbBook.Author = Author.Text;
                    dbBook.Title = Judul.Text;
                    if(imagePath != "")
                    {
                        string imageUrl = await BlobUploader.Main(imagePath);
                        dbBook.Image = imageUrl;
                    }
                    db.SaveChanges();
                    MessageBox.Show("Perubahan Disimpan", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    Navigate(MainWindow.Navigation.dashboard);
                }
            }
        }
        private void HapusBuku(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus buku ini?", "Penghapusan Buku", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new GrahitaDBEntities())
                {
                    var bookToDelete = db.Books.Find(book.Id);
                    db.Books.Remove(bookToDelete);
                    db.SaveChanges();
                    Navigate(MainWindow.Navigation.dashboard);
                    MessageBox.Show("Buku berhasil dihapus", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            Navigate(MainWindow.Navigation.back);
        }
    }
}
