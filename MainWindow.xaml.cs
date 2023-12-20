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
using static System.Net.Mime.MediaTypeNames;

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
            Navigate(Navigation.buku);
            setSignedIn(false, null);
        }
        private void setSignedIn(bool isSignedIn, User user)
        {
            this.isSignedIn = isSignedIn;
            this.user = user;

            AuthMenu.Visibility = !isSignedIn ? Visibility.Visible : Visibility.Collapsed;
            ProfileMenu.Visibility = isSignedIn ? Visibility.Visible : Visibility.Collapsed;
            Profile.Header = user?.Name;

            UserSignedInChanged?.Invoke(isSignedIn, user);
        }
        public enum Navigation
        {
            buku,
            dashboard,
            signin,
            register,
            tambahBuku,
            keteranganBuku,
        }
        public void Navigate(Navigation target)
        {
            switch(target)
            {
                case Navigation.buku:
                    mainFrame.Navigate(new BukuPage(NavigateKeterangan));
                    break;
                case Navigation.dashboard:
                    mainFrame.Navigate(new DashboardPage(isSignedIn, user, Navigate, NavigateKeterangan));
                    break;
                case Navigation.signin:
                    mainFrame.Navigate(new SignInPage(SignIn));
                    break;
                case Navigation.register:
                    mainFrame.Navigate(new RegisterPage(SignIn));
                    break;
                case Navigation.tambahBuku:
                    mainFrame.Navigate(new TambahBukuPage(user, Navigate));
                    break;
            }
        }
        private void SignIn(User user)
        {
            setSignedIn(true, user);
            Navigate(Navigation.dashboard);
        }
        private void NavigateBuku(object sender, RoutedEventArgs e)
        {
            Navigate(Navigation.buku);
        }
        private void NavigateDasboard(object sender, RoutedEventArgs e)
        {
            Navigate(Navigation.dashboard);
        }
        private void NavigateSignIn(object sender, RoutedEventArgs e)
        {
            Navigate(Navigation.signin);
        }
        private void NavigateRegister(object sender, RoutedEventArgs e)
        {
            Navigate(Navigation.register);
        }
        private void SignOut(object sender, RoutedEventArgs e)
        {
            setSignedIn(false, null);
            Navigate(Navigation.buku);
        }
        private void NavigateKeterangan(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Book clickedBook)
            {
                mainFrame.Navigate(new KeteranganBukuPage(clickedBook, user));
            }
        }
    }
}
