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
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new BukuPage());
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
            mainFrame.Navigate(new DashboardPage());
        }
    }
}
