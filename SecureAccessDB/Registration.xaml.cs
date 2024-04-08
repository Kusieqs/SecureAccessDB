using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SecureAccessDB
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {

        }
        private void LoginChanged(object sender, RoutedEventArgs e)
        {
            LoginFailed.Text = string.Empty;
        }
        private void EmailChanged(object sender, RoutedEventArgs e)
        {
            EmailFailed.Text = string.Empty;
        }
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordFailed.Text = string.Empty;
        }
        private void RepeatPasswordChanged(object sender, RoutedEventArgs e)
        {
            RepeatPasswordFailed.Text = string.Empty;
        }
    }
}
