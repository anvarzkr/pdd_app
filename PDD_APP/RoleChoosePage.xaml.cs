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
    /// Логика взаимодействия для RoleChoosePage.xaml
    /// </summary>
    public partial class RoleChoosePage : Page
    {
        public RoleChoosePage()
        {
            InitializeComponent();

            if (SessionController.sessionControllerStatic != null) {
                SessionController.sessionControllerStatic.closeThreads();
            }

            mainGrid.Background = new SolidColorBrush(Utils.backgroundColor);
            backButton.Background = new SolidColorBrush(Utils.darkRedColor2);
            backButtonText.FontSize = Utils.fontSize - 4;
            menuAdminText.FontSize = Utils.fontSize - 4;
            menuStudentText.FontSize = Utils.fontSize - 4;

            menuAdminButton.Background = new SolidColorBrush(Utils.darkBlueColor2);
            menuStudentButton.Background = new SolidColorBrush(Utils.darkBlueColor2);
        }

        private void menuAdminButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Utils.goToPage("PasswordPage");
        }

        private void menuStudentButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Utils.goToPage("SessionConnectingPage");
        }

        private void menuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkBlueColor1);
        }

        private void menuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkBlueColor2);
        }

        private void backButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
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
