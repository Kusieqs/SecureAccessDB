using System.Net;
using System.Windows;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Windows.Controls;
namespace SecureAccessDB
{
    public partial class LoginPage : Window
    {
        private string login;
        private string password;

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
                if (stream == null)
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
            login = Login.Text;
            password = Password.Text;

            bool correct;
            CheckingEmptyText(login, password, out correct);

            if (!correct)
                return;

            SqlConnection sql = new SqlConnection();
            sql.Open();


            if (CheckingLogin(sql))
                return;
            if (CheckingPassword(sql))
                return;

            MessageBox.Show("Correct sign in","Correct",MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {

        }
        private bool CheckingLogin(SqlConnection sql)
        {
            string sqlQuery = $"Select Username From Users Where Username = @login";
            SqlCommand command = new SqlCommand(sqlQuery, sql);
            command.Parameters.AddWithValue("@login", login);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                LoginFailed.Text = "User doesn't exist";
                reader.Close();
                return true;
            }
            reader.Close();
            return false;

        }
        private bool CheckingPassword(SqlConnection sql)
        {
            string sqlQuery = $"Select Password, Username From Users Where Password = @password and Username = @login";
            SqlCommand command = new SqlCommand(sqlQuery, sql);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@login", login);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                PasswordFailed.Text = "Password is not correct";
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }
        private void CheckingEmptyText(string login, string password, out bool correct)
        {
            correct = true;
            if (string.IsNullOrEmpty(login))
            {
                correct = false;
                LoginFailed.Text = "Username not provided";
            }

            if (string.IsNullOrEmpty(password))
            {
                correct = false;
                PasswordFailed.Text = "Password not provided";
            }

        }
        private void LoginChanged(object sender, RoutedEventArgs e)
        {
            if (LoginFailed.Text != "")
                LoginFailed.Text = "";
        }
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordFailed.Text != "")
                PasswordFailed.Text = "";
        }
    }
}