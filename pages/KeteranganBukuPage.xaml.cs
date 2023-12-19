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
        public KeteranganBukuPage(Book book)
        {
            InitializeComponent();
            this.book = book;
            Judul.Text = book.Title;
            Gambar.Source = book.Image != null ? new BitmapImage(new Uri(book.Image)) : null;
            Author.Text = book.Author;

            using (var db = new GrahitaDBEntities())
            {
                var query = from user in db.Users
                            where user.Id == book.Owner
                            select user;
                Owner.Text = query.FirstOrDefault() != null ? query.First().Name : "";
            }

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
        }   
    }
}
