﻿using Grahita.components;
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
        }
        public void Navigate(Navigation target)
        {
            if (target == Navigation.buku) mainFrame.Navigate(new BukuPage());
            else if (target == Navigation.dashboard) mainFrame.Navigate(new DashboardPage(isSignedIn, user, Navigate));
            else if (target == Navigation.signin) mainFrame.Navigate(new SignInPage(SignIn));
            else if (target == Navigation.register) mainFrame.Navigate(new RegisterPage(SignIn));
            else if (target == Navigation.tambahBuku) mainFrame.Navigate(new TambahBukuPage(user, Navigate));
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
    }
}
