using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Thoth.Models;
using Thoth.Static_Resources;

namespace Thoth
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
            UserBase.LoadUsers();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            //Authenticate user
            User user = UserBase.FindUser(usernameTextbox.Text, passwordTextbox.Text);
            if(user != null)
            {
                //decrypt studd using this base
                MessageBox.Show($"Welcome {user.username}");
            }
        }

        private void RegisterUser_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("users")) Directory.CreateDirectory("users");
            new RegisterWindow().ShowDialog();
        }
    }
}
