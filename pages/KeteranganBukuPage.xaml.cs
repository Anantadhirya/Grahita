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
                ButtonPinjam.Visibility = Visibility.Visible;
            }

            this.isSignedIn = isSignedIn;
            this.Navigate = Navigate;
            this.NavigateEditBuku = navigateEditBuku;
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
            OwnerWindow ownerWindow = new OwnerWindow(owner);
            ownerWindow.ShowDialog();
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
            MessageBoxResult result=MessageBox.Show("Apakah Anda yakin?", "Ubah status buku", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                using (var db = new GrahitaDBEntities())
                {

                    var updatedBook = db.Books.Find(book.Id);
                    updatedBook.Available = !updatedBook.Available;
                    db.SaveChanges();

                    if (updatedBook.Available)
                    {
                        Status.Text = "Tersedia";
                        Status.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        Status.Text = "Dipinjam";
                        Status.Foreground = new SolidColorBrush(Colors.Red);
                    }

                    MessageBox.Show("Status buku diubah", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

       

    }
}
