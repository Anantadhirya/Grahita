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
        public EditBukuPage(Book book)
        {
            InitializeComponent();
            this.book = book;
            initDefaultValue();
        }

        public void initDefaultValue()
        {
            Judul.Text = book.Title;
            Author.Text=book.Author;
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
                    var dbBook = db.Books.Find(book.Id);
                    dbBook.Author =Author.Text;
                    dbBook.Title =Judul.Text;
                    dbBook.Image =imagePath;
                    db.SaveChanges();
                    MessageBox.Show("Edit buku berhasil", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

    }
}
