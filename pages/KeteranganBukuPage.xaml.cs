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
    /// Interaction logic for KeteranganBukuPage.xaml
    /// </summary>
    public partial class KeteranganBukuPage : Page
    {
        Book book;
        User owner, currentUser;
        bool isSignedIn;
        private Action<MainWindow.Navigation> Navigate;
        private Action<Book> NavigateEditBuku;
        public KeteranganBukuPage(Book book, User currentUser, bool isSignedIn, Action<MainWindow.Navigation> Navigate, Action<Book> navigateEditBuku)
        {
            InitializeComponent();
            this.book = book;
            this.currentUser = currentUser;
            using (var db = new GrahitaDBEntities())
            {
                var query = from user in db.Users
                            where user.Id == book.Owner
                            select user;
                this.owner = query.FirstOrDefault();
            }

            Judul.Text = book.Title;
            Gambar.Source = book.Image != null ? new BitmapImage(new Uri(book.Image)) : null;
            Author.Text = book.Author;
            Owner.Text = owner != null ? owner.Name : "";
            NamaPemilik.Text = owner != null ? owner.Name : "";
            KontakPemilik.Text = owner != null ? owner.Contact : "";
            LokasiPemilik.Text = owner != null ? owner.Location : "";
            GambarPemilik.Source = new BitmapImage(new Uri(owner != null && owner.Image != null ? owner.Image : "pack://application:,,,/public/images/blank_profile.jpg"));
            updateStatusBuku();

            this.isSignedIn = isSignedIn;
            this.Navigate = Navigate;
            this.NavigateEditBuku = navigateEditBuku;
        }

        private void updateStatusBuku()
        {
            if (book.Available)
            {
                Status.Text = "Tersedia";
                Status.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                Status.Text = "Dipinjam";
                Status.Foreground = new SolidColorBrush(Colors.Red);
            }
            if (currentUser != null && owner != null && currentUser.Id == owner.Id)
            {
                // Kalau mbuka bukunya sendiri
                ButtonEdit.Visibility = Visibility.Visible;
                ButtonPinjam.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Kalau mbuka bukunya orang lain
                ButtonEdit.Visibility = Visibility.Collapsed;
                ButtonPinjam.Visibility = book.Available ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void onPinjam(object sender,RoutedEventArgs e)
        {
            SignInRequired.Visibility = isSignedIn ? Visibility.Collapsed : Visibility.Visible;
            KeteranganBuku.Visibility = isSignedIn ? Visibility.Visible : Visibility.Collapsed;

            if (isSignedIn)
            {
                TriggerUserWindow();
            }

        }

        private void TriggerUserWindow()
        {
            KeteranganPeminjaman.Visibility = Visibility.Visible;
        }
        private void CloseUserWindow(object sender, RoutedEventArgs e)
        {
            KeteranganPeminjaman.Visibility = Visibility.Collapsed;
        }

        private void NavigateSignIn(object sender,RoutedEventArgs e)
        {
            Navigate(MainWindow.Navigation.signin);
        }

        private void ClickEditBuku(object sender, RoutedEventArgs e)
        {
            NavigateEditBuku(book);
        }

        private void onChangeStatus(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result=MessageBox.Show("Apakah Anda ingin mengubah status buku menjadi " + (book.Available ? "tidak tersedia" : "tersedia") + "?", "Perubahan Status Buku", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new GrahitaDBEntities())
                {

                    var updatedBook = db.Books.Find(book.Id);
                    updatedBook.Available = !updatedBook.Available;
                    book.Available = !book.Available;
                    db.SaveChanges();
                    updateStatusBuku();
                    MessageBox.Show("Status buku diubah", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void Back(object sender, MouseButtonEventArgs e)
        {
            Navigate(MainWindow.Navigation.back);
        }
    }
}
