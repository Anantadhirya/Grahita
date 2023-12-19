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
        }
    }
}
