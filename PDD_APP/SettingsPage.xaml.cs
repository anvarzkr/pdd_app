using PDD_APP.Models;
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
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            backButton.Background = new SolidColorBrush(Utils.darkRedColor2);
            backButtonText.FontSize = Utils.fontSize - 4;
            okButtonText.FontSize = Utils.fontSize - 4;
            passwordField.FontSize = Utils.fontSize - 4;
        }

        private void passwordField_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordField.Text = "";
        }

        private void passwordField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passwordField.Text == "")
                passwordField.Text = "Ip-Адрес главного компьютера";
        }

        private void okButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            String ipAdress = passwordField.Text;
            Admin.setHostIp(ipAdress);
            MessageBox.Show("IP-Адресс главного компьютера изменен на: " + ipAdress);
            Utils.goToPage("RoleChoosePage");
        }

        private void okButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkBlueColor1);
        }

        private void okButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkBlueColor2);
        }

        private void backButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Utils.goToPage("PasswordPage");
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
