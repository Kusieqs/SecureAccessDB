using System.Net;
using System.Windows;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace SecureAccessDB
{
    public partial class LoginPage : Window
    {
        private string login;
        private string password;
        private Registration registration;
        public LoginPage()
        {
            InitializeComponent();
        }
        private void ConnectNetworkCheck(object sender, EventArgs e)
        {
            NetworkCheck(this);
        } // forwarding to NetworkCheck
        public static void NetworkCheck(Window window)
        {
            while(true)
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
                    MessageBoxResult result = MessageBox.Show("No internet connection\nDo you want try again?" , "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.No)
                        window.Close();
                    else
                        continue;
                }
                break;
            }
        }// checking whether the computer is connected to the Internet
        private void SignIn(object sender, RoutedEventArgs e)
        {
            NetworkCheck(this);

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
        } // Checking correct login and password ( if l and p is right, messagebox will show)
        private void SignUp(object sender, RoutedEventArgs e)
        {
            registration = new Registration();
            registration.Show();
            this.IsEnabled = false;
            registration.Closed += (s, args) =>
            {
                this.IsEnabled = true;
                Focus();
            };
        } // forwarding to registration window
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

        } // Checking correct login in db
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
        }// Checking correct password to login in db
        private void CheckingEmptyText(string login, string password, out bool correct) // if box is empty red text under the box will show
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
        private void LoginChanged(object sender, RoutedEventArgs e) // if login box will change, red text will disappear
        {
            if (LoginFailed.Text != "")
                LoginFailed.Text = "";
        }
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordFailed.Text != "")
                PasswordFailed.Text = "";
        }// if password box will change, red text will disappear
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if(registration != null && registration.IsVisible)
                e.Cancel = true;
        } // if registration is open, loginpage can not be close

    }
}