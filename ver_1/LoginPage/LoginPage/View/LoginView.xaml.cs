using System.Windows;
using System.Windows.Controls;

namespace LoginPage.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {


            // Uri uri = new Uri("Page2.xaml", UriKind.Relative);
            //this.NavigationService.Navigate(uri);


        }


    }
}
