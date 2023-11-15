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
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        private bool isSignedIn;
        private User user;
        Action<MainWindow.Navigation> Navigate;
        public DashboardPage(bool isSignedIn, User user, Action<MainWindow.Navigation> Navigate)
        {
            InitializeComponent();
            updateSignedIn(true, user);
            this.Navigate = Navigate;
            MainWindow.UserSignedInChanged += updateSignedIn;
        }
        private void updateSignedIn(bool isSignedIn, User user)
        {
            this.isSignedIn = isSignedIn;
            this.user = user;

            SignInRequired.Visibility = !isSignedIn ? Visibility.Visible : Visibility.Collapsed;
            Dashboard.Visibility = isSignedIn ? Visibility.Visible : Visibility.Collapsed;
            Username.Text = user?.Name;
            if(user != null )
            {
                using (var db = new GrahitaDBEntities())
                {
                    var query = from book in db.Books where book.Owner == user.Id select book;
                    var test = query.ToList();
                    BookList.ItemsSource = query.ToList();
                }
            }
        }
        private void NavigateSignIn(object sender, RoutedEventArgs e)
        {
            Navigate(MainWindow.Navigation.signin);
        }
        private async void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                string imageUrl = await BlobUploader.Main(imagePath);
                pictureBox.Source = new BitmapImage(new Uri(imageUrl));
                txtImagePath.Text = imageUrl;

            }
        }
    }
}
