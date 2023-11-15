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
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        private bool isSignedIn;
        private int userID;
        public DashboardPage(bool isSignedIn, int userID)
        {
            InitializeComponent();
            setSignedIn(isSignedIn, userID);
            MainWindow.UserSignedInChanged += setSignedIn;
        }
        private void setSignedIn(bool isSignedIn, int userID)
        {
            this.isSignedIn = isSignedIn;
            this.userID = userID;
            MainText.Text = isSignedIn ? "Signed In" : "Signed Out";
        }
    }
}
