using Grahita.components;
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
        private User user;
        public static event Action<bool, User> UserSignedInChanged;
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new BukuPage());
            setSignedIn(false, null);
        }
        private void setSignedIn(bool isSignedIn, User user)
        {
            this.isSignedIn = isSignedIn;
            this.user = user;

            AuthMenu.Visibility = !isSignedIn ? Visibility.Visible : Visibility.Collapsed;
            ProfileMenu.Visibility = isSignedIn ? Visibility.Visible : Visibility.Collapsed;

            UserSignedInChanged?.Invoke(isSignedIn, user);
        }
        private void SignIn(User user)
        {
            setSignedIn(true, user);
            mainFrame.Navigate(new DashboardPage(isSignedIn, user));
        }
        private void NavigateBuku(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new BukuPage());
        }
        private void NavigateDasboard(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new DashboardPage(isSignedIn, user));
        }
        private void NavigateSignIn(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new SignInPage(SignIn));
        }
        private void NavigateRegister(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new RegisterPage(SignIn));
        }
        private void NavigateProfile(object sender, RoutedEventArgs e)
        {
            setSignedIn(false, null);
        }
    }
}
