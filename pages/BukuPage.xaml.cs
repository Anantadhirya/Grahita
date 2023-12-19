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
                BookList.ItemsSource = query.ToList();
                LatestBook.ItemsSource = query.ToList();
            }
            this.onClick = onClick;
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
