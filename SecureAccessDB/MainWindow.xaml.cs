using System.Net;
using System.Windows;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
namespace SecureAccessDB
{
    public partial class LoginPage : Window
    {
        
        public LoginPage()
        {
            InitializeComponent();
        }

        private void NetworkCheck(object sender, EventArgs e)
        {
            try
            {
                var client = new WebClient();
                var stream = client.OpenRead("http://www.google.com");
                if(stream == null)
                {
                    client.Dispose();
                    throw new Exception();
                }
            }
            catch
            {
                MessageBox.Show("No internet connection", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
        
        private void SignIn(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Text;
            bool correct;
            CheckingEmptyText(login, password, out correct);

            if (!correct)
                return;

            SqlConnection sql = new SqlConnection();
            sql.Open();
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {

        }

        private void CheckingEmptyText(string login, string password,out bool correct)
        {
            correct = true;
            if (string.IsNullOrEmpty(login))
            {
                correct = false;
                // red text
            }

            if (string.IsNullOrEmpty(password))
            {
                correct = false;
                // red text
            }
        }
    }
}