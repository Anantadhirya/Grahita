﻿using Microsoft.Win32;
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
        Action<object, RoutedEventArgs> onClick;
        public DashboardPage(bool isSignedIn, User user, Action<MainWindow.Navigation> Navigate, Action<object, RoutedEventArgs> onClick)
        {
            InitializeComponent();
            updateSignedIn(isSignedIn, user);
            this.Navigate = Navigate;
            MainWindow.UserSignedInChanged += updateSignedIn;
            this.onClick = onClick;
        }
        private void updateSignedIn(bool isSignedIn, User user)
        {
            this.isSignedIn = isSignedIn;
            this.user = user;

            SignInRequired.Visibility = !isSignedIn ? Visibility.Visible : Visibility.Collapsed;
            Dashboard.Visibility = isSignedIn ? Visibility.Visible : Visibility.Collapsed;
            Username.Text = user?.Name;
            Gambar.Source = new BitmapImage(new Uri(user?.Image != null ? user?.Image : "pack://application:,,,/public/images/blank_profile.jpg"));
            BookListText.Visibility = Visibility.Collapsed;
            if(user != null)
            {
                using (var db = new GrahitaDBEntities())
                {
                    var query = from book in db.Books where book.Owner == user.Id select book;
                    if(query.Any())
                    {
                        BookListText.Visibility = Visibility.Visible;
                    }
                    BookList.ItemsSource = query.ToList();
                }
            }
        }
        private void NavigateSignIn(object sender, RoutedEventArgs e)
        {
            Navigate(MainWindow.Navigation.signin);
        }
        private void NavigateTambahBuku(object sender, RoutedEventArgs e)
        {
            Navigate(MainWindow.Navigation.tambahBuku);
        }

        /*
        <!-- Temp -->
        <Button x:Name="btnLoadImage" Content="Load Image" Click="btnLoadImage_Click" Margin="0,100,0,0" Style="{StaticResource DefaultButton}"/>
        <TextBlock x:Name="txtImagePath" Grid.Row="2" Text="Image Path will be displayed here." TextWrapping="Wrap" VerticalAlignment="Center"/>
        <Image x:Name="pictureBox" Grid.Row="3" Stretch="Uniform"/>


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
        */
    }
}
