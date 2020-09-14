using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : MetroWindow
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            User user = UserBase.FindUser(usernameTextbox.Text, passwordTextbox.Text);
            if (user == null)
            {
                //decrypt studd using this base
                if(passwordTextbox.Text != confirmPasswordTextbox.Text)
                {
                    MessageBox.Show("Passwords do not match");
                }
                else
                {
                    User newUser = new User(usernameTextbox.Text, passwordTextbox.Text);
                    UserBase.AddUser(newUser);

                    MessageBox.Show($"Account for {newUser.username} has succesfully been created");
                }
            }
            else
            {
                MessageBox.Show("This user can not be created. Maybe try using a different username");
            }
        }
    }
}
