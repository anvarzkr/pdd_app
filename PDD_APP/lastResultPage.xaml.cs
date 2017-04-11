using PDD_APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для lastResultPage.xaml
    /// </summary>
    public partial class lastResultPage : Page
    {
        public static Student student;
        public lastResultPage()
        {
            InitializeComponent();

            mainGrid.Background = new SolidColorBrush(Utils.backgroundColor);
            resultTextBorder.Background = new SolidColorBrush(Utils.darkBlueColor2);
            resultText.FontSize = Utils.fontSize + 16;
            fnameBlock.FontSize = Utils.fontSize + 1;
            lnameBlock.FontSize = Utils.fontSize + 1;
            idBlock.FontSize = Utils.fontSize + 1;
            border1Text.FontSize = Utils.fontSize - 4;
            border2Text.FontSize = Utils.fontSize - 4;
            border3Text.FontSize = Utils.fontSize - 4;
            timeBlock.FontSize = Utils.fontSize - 4;
            rAnsBlock.FontSize = Utils.fontSize - 4;
            wAnsBlock.FontSize = Utils.fontSize - 4;

            student = TestingPage.studentStatic;

            fnameBlock.Text = student.fname;
            fnameBlockBorder.Background = new SolidColorBrush(Utils.darkBlueColor2);
            lnameBlock.Text = student.lname;
            lnameBlockBorder.Background = new SolidColorBrush(Utils.darkBlueColor2);
            idBlock.Text = student.id.ToString();
            idBlockBorder.Background = new SolidColorBrush(Utils.darkBlueColor2);

            border1.Background = new SolidColorBrush(Utils.darkGreenColor2);
            border2.Background = new SolidColorBrush(Utils.darkGreenColor2);
            border3.Background = new SolidColorBrush(Utils.darkGreenColor2);

            border11.Background = new SolidColorBrush(Utils.darkGreenColor2);
            border12.Background = new SolidColorBrush(Utils.darkGreenColor2);
            border13.Background = new SolidColorBrush(Utils.darkGreenColor2);

            resendButton.Background = new SolidColorBrush(Utils.darkRedColor2);
            menuButton.Background = new SolidColorBrush(Utils.darkBlueColor2);
            helpButton.Background = new SolidColorBrush(Utils.darkBlueColor2);

            timeBlock.Text = Utils.getTimeFromInt(mistakesClarificationPage.testingPage.test.timeCounterPassed);
            rAnsBlock.Text = mistakesClarificationPage.testingPage.test.right_answered.ToString();
            wAnsBlock.Text = (mistakesClarificationPage.testingPage.test.tasks.Length - mistakesClarificationPage.testingPage.test.right_answered).ToString();

            new Thread(()=>{Thread.Sleep(100); MessageBox.Show("Нажмите кнопку \"Отправить результаты\" для закрепления результатов.");}).Start();
        }

        private void resendButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TestingPage.currentTest.sendStats();
                MessageBox.Show("Результаты успешно отправлены. Спасибо.");
                resendButton.Visibility = System.Windows.Visibility.Collapsed;
                menuButton.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось отправить результаты на главный компьютер из-за проблемы с соединением. Нажмите кнопку \"Отправить Результаты\", чтобы отправить данные снова.");
            }
        }

        private void helpButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new Thread(() => { new AsynchronousClient(SessionController.sessionControllerStatic).StartClient(SessionController.sessionControllerStatic.hostIpAdress, SessionController.sessionControllerStatic.hostPort, AsynchronousClient.Command.HELP); }).Start();
        }

        private void resendButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkRedColor1);
        }

        private void resendButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkRedColor2);
        }

        private void menuButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SessionController.sessionControllerStatic.closeThreads();
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Utils.goToPage("RoleChoosePage");
        }

        private void menuButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkBlueColor1);
        }

        private void menuButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkBlueColor2);
        }
    }
}
