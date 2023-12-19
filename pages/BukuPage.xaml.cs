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
    /// Interaction logic for BukuPage.xaml
    /// </summary>
    public partial class BukuPage : Page
    {
        Action<object,RoutedEventArgs> onClick;
        public BukuPage(Action<object, RoutedEventArgs> onClick)
        {
            InitializeComponent();
            using (var db = new GrahitaDBEntities())
            {
                var query = from b in db.Books orderby b.Title select b;
                BookList.ItemsSource = query.ToList();
                LatestBook.ItemsSource = query.ToList();
            }
            this.onClick = onClick;
        }
    }
}
