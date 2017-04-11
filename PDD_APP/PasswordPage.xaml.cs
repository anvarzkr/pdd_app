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

namespace PDD_APP
{
    /// <summary>
    /// Логика взаимодействия для PasswordPage.xaml
    /// </summary>
    public partial class PasswordPage : Page
    {
        public PasswordPage()
        {
            InitializeComponent();

            menuButton.Background = new SolidColorBrush(Utils.darkBlueColor2);
            passwordField.Background = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255));
            okButton.Background = new SolidColorBrush(Utils.darkBlueColor2);
            backButton.Background = new SolidColorBrush(Utils.darkRedColor2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void passwordField_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordField.Text = "";
        }

        private void passwordField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passwordField.Text == "")
                passwordField.Text = "Пароль";
        }

        private void okButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            String password = passwordField.Text;
            if (Utils.CalculateMD5Hash(password) == PDD_APP.Models.Admin.getPass())
            {
                Utils.goToPage("SessionPage");
            }
            else
            {
                passwordField.Text = "Пароль";
                MessageBox.Show("Вы ввели неверный пароль!");
            }
        }

        private void okButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkBlueColor1);
        }

        private void okButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkBlueColor2);
        }

        private void menuButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Utils.goToPage("SettingsPage");
        }

        private void menuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            menuButton.Background = new SolidColorBrush(Utils.darkBlueColor1);
        }

        private void menuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            menuButton.Background = new SolidColorBrush(Utils.darkBlueColor2);
        }

        private void backButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Utils.goToPage("RoleChoosePage");
        }

        private void backButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkRedColor1);
        }

        private void backButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkRedColor2);
        }

    }
}
