<<<<<<< HEAD
﻿using Microsoft.Win32;
=======
﻿using Grahita.components;
>>>>>>> 8dbec60bf06dc9044f30fef5a70b3c08de3db44c
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
        public DashboardPage(bool isSignedIn, User user)
        {
            InitializeComponent();
            setSignedIn(isSignedIn, user);
            MainWindow.UserSignedInChanged += setSignedIn;
        }
        private void setSignedIn(bool isSignedIn, User user)
        {
            this.isSignedIn = isSignedIn;
            this.user = user;
            MainText.Text = isSignedIn ? "Signed In" : "Signed Out";
        }
        private async void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                await BlobUploader.Main(imagePath);
                pictureBox.Source = new BitmapImage(new Uri(imagePath));

            }
        }
    }
}
