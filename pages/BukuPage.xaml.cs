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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grahita.pages
{
    public partial class BukuPage : Page
    {
        Action<object,RoutedEventArgs> onClick;
        private Point startPoint;
        private double startHorizontalOffset;
        private bool isButtonClick;
        public BukuPage(Action<object, RoutedEventArgs> onClick)
        {
            InitializeComponent();
            using (var db = new GrahitaDBEntities())
            {
                var query = from b in db.Books orderby b.Title select b;
                BookList.ItemsSource = query.Take(100).ToList();

                query = from b in db.Books orderby b.Id descending select b;
                LatestBook.ItemsSource = query.Take(10).ToList();
            }
            this.onClick = onClick;
        }

        // Search bar
        private void onSearchGotFocus(object sender, RoutedEventArgs e)
        {
            SearchPlaceholder.Visibility = Visibility.Collapsed;
        }
        private void onSearchLostFocus(object sender, RoutedEventArgs e)
        {
            if(SearchText.Text == "") SearchPlaceholder.Visibility = Visibility.Visible;
        }
        private void onSearchEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var searchString = SearchText.Text;
                if(searchString == "")
                {
                    TextBukuTerbaru.Visibility = Visibility.Visible;
                    CarouselScrollViewer.Visibility = Visibility.Visible;
                    TextDaftarBuku.Text = "Daftar Buku";
                } else
                {
                    TextBukuTerbaru.Visibility =Visibility.Collapsed;
                    CarouselScrollViewer.Visibility = Visibility.Collapsed;
                    TextDaftarBuku.Text = "Hasil Pencarian";
                }
                using(var db = new GrahitaDBEntities())
                {
                    var searchWords = searchString.Split(' ').Where(i => !string.IsNullOrEmpty(i));
                    var query = from b in db.Books orderby b.Title where searchWords.All(searchWord => b.Title.Contains(searchWord)) select b;
                    BookList.ItemsSource = query.Take(100).ToList();
                }
            }
        }

        // Fungsi-fungsi untuk swipe carousel
        private void Carousel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(this);
            startHorizontalOffset = CarouselScrollViewer.HorizontalOffset;
            CarouselScrollViewer.CaptureMouse();
            isButtonClick = true;
        }
        private void Carousel_MouseMove(object sender, MouseEventArgs e)
        {
            if (CarouselScrollViewer.IsMouseCaptured)
            {
                Point currentPoint = e.GetPosition(this);
                double offsetX = currentPoint.X - startPoint.X;
                CarouselScrollViewer.ScrollToHorizontalOffset(startHorizontalOffset - offsetX/GetViewboxScale());
                if(Math.Abs(offsetX) > SystemParameters.MinimumHorizontalDragDistance)
                {
                    isButtonClick = false;
                }
            }
        }
        private void Carousel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CarouselScrollViewer.ReleaseMouseCapture();
            if(isButtonClick)
            {
                var hitTestResult = VisualTreeHelper.HitTest(LatestBook, e.GetPosition(LatestBook));
                if (hitTestResult != null)
                {
                    var button = FindVisualParent<Button>(hitTestResult.VisualHit);
                    if (button != null)
                    {
                        onClick(button, null);
                    }
                }
            }
            isButtonClick = false;
        }
        // Helper function untuk carousel
        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }
        private double GetViewboxScale()
        {
            var viewbox = FindVisualParent<Viewbox>(CarouselScrollViewer);
            if (viewbox != null)
            {
                return viewbox.ActualWidth / CarouselScrollViewer.ViewportWidth;
            }
            return 1.0;
        }

        //
    }
}
