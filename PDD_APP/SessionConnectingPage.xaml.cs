using Newtonsoft.Json;
using PDD_APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для SessionConnectingPage.xaml
    /// </summary>
    public partial class SessionConnectingPage : Page
    {
        public static String hostIpAdress = "192.168.1.109"; 
        public static int hostPort = 11000;

        public SessionConnectingPage()
        {
            InitializeComponent();

            if (SessionController.sessionControllerStatic != null)
            {
                SessionController.sessionControllerStatic.closeThreads();
            }

            border1.Background = new SolidColorBrush(Utils.darkBlueColor2);
            border2.Background = new SolidColorBrush(Utils.darkBlueColor2);
            studentInfoBorder.Background = new SolidColorBrush(Utils.darkBlueColor2);
            studentInfo.FontSize = Utils.fontSize + 16;
            sessionStartButton.Background = new SolidColorBrush(Utils.darkBlueColor1);
            sessionStartText.FontSize = Utils.fontSize - 4;
            backButton.Background = new SolidColorBrush(Utils.darkRedColor2);
            backButtonText.FontSize = Utils.fontSize - 4;
            border1Text.FontSize = Utils.fontSize + 16;
            border2Text1.FontSize = Utils.fontSize + 16;
            border2Text2.FontSize = Utils.fontSize + 16;
        }

        private void sessionStartButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            hostIpAdress = Admin.getHostIp();
            //MessageBox.Show(hostIpAdress);
            SessionController sc = new SessionController(this, hostIpAdress, hostPort);
            if (sc.testConnection())
            {
                sessionClosedBlock.Visibility = System.Windows.Visibility.Collapsed;
                sessionWUAC.Visibility = System.Windows.Visibility.Visible;
                backButton.Visibility = Visibility.Collapsed;
                sc.startSession();
            }
        }

        private void sessionStartButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkBlueColor2);
        }

        private void sessionStartButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkBlueColor1);
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
