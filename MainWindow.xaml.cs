using Grahita.pages;
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

namespace Grahita
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isSignedIn;
        private int userID;
        public static event Action<bool, int> UserSignedInChanged;
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new BukuPage());
            setSignedIn(false, 0);
        }
        private void setSignedIn(bool isSignedIn, int userID)
        {
            this.isSignedIn = isSignedIn;
            this.userID = userID;

            AuthMenu.Visibility = !isSignedIn ? Visibility.Visible : Visibility.Collapsed;
            ProfileMenu.Visibility = isSignedIn ? Visibility.Visible : Visibility.Collapsed;

            UserSignedInChanged?.Invoke(isSignedIn, userID);
        }
        private void NavigateBuku(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new BukuPage());
        }
        private void NavigatePerpus(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new PerpusPage());
        }
        private void NavigateDasboard(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new DashboardPage(isSignedIn, userID));
        }
        private void NavigateLogin(object sender, RoutedEventArgs e)
        {
            setSignedIn(true, 0);
        }
        private void NavigateRegister(object sender, RoutedEventArgs e)
        {
            setSignedIn(true, 0);
        }
        private void NavigateProfile(object sender, RoutedEventArgs e)
        {
            setSignedIn(false, 0);
        }
    }
}
