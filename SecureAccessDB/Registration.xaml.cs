using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            LoginPage.NetworkCheck(this);

            if (!CheckingEmptyString())
                return;

            SqlConnection sql = new SqlConnection();
            sql.Open();

            if (!CheckAllParameters(sql))
                return;


            sql.Close();
        }
        #region Feature when text was changing
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
        #endregion
        private void ConnectNetworkCheck(object sender, EventArgs e)
        {
            LoginPage.NetworkCheck(this);
        }
        private bool CheckingEmptyString()
        {
            bool correct = true;
            if(string.IsNullOrEmpty(Login.Text))
            {
                LoginFailed.Text = "Login is empty";
                correct = false;
            }

            if(string.IsNullOrEmpty(Password.Text))
            {
                PasswordFailed.Text = "Password is empty";
                correct = false;
            }

            if(string.IsNullOrEmpty(RepeatPassword.Text))
            {
                RepeatPasswordFailed.Text = "Password repeat is empty";
                correct = false;
            }

            if(string.IsNullOrEmpty(Email.Text))
            {
                EmailFailed.Text = "Email is empty";
                correct = false;
            }
            return correct;

        }
        private bool CheckAllParameters(SqlConnection sql)
        {
            return LoginCheck(sql) & PasswordCheck() & RepeatPasswordCheck();
        }
        private bool LoginCheck(SqlConnection sql)
        {
            if(Login.Text.Length < 4) 
            {
                LoginFailed.Text = "Login is too short";
                return false;
            }

            if(Login.Text.Length > 20)
            {
                LoginFailed.Text = "Login is too long";
                return false;
            }

            string sqlQuery = $"Select Username From Users Where Username = @login";
            SqlCommand command = new SqlCommand(sqlQuery, sql);
            command.Parameters.AddWithValue("@login", Login.Text);
            SqlDataReader reader = command.ExecuteReader();

            if(reader.HasRows)
            {
                LoginFailed.Text = "This login is locked";
                reader.Close();
                return false;
            }
            reader.Close();
            return true;

        }
        private bool PasswordCheck()
        {
            if(Password.Text.Length < 6)
            {
                PasswordFailed.Text = "Password is too short";
                return false;
            }

            if(PasswordFailed.Text.Length > 20)
            {
                PasswordFailed.Text = "Password is to long";
                return false;
            }

            return true;

        }
        private bool RepeatPasswordCheck()
        {
            if (RepeatPassword.Text.Trim() != Password.Text.Trim())
            {
                RepeatPasswordFailed.Text = "Passwords are not the same";
                return false;
            }
            return true;
        }
        private bool EmailCheck(SqlConnection sql)
        {
            string sqlQuery = $"Select email From Users Where email = @email";
            SqlCommand command = new SqlCommand( sqlQuery, sql);
            command.Parameters.AddWithValue("@email", Email.Text);
            SqlDataReader reader = command.ExecuteReader();
            if(reader.HasRows)
            {
                EmailFailed.Text = "This email is locked";
                reader.Close();
                return false;
            }
            reader.Close();

            // regex
        }
    }
}
