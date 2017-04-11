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
    /// Логика взаимодействия для SessionPrintingPage.xaml
    /// </summary>
    public partial class SessionPrintingPage : Page
    {
        private int sessionIdSelected;
        public SessionPrintingPage()
        {
            InitializeComponent();

        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            Utils.printStats(Session.get(sessionIdSelected));
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainFrame.GoBack();
        }

        private void sessionsListBox_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Session session in Session.getAll()) {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Tag = session.id;
                TextBlock textBlock = new TextBlock();
                textBlock.Text = session.id + " | " + Utils.getDate(session.timeMilliseconds);
                listBoxItem.Content = textBlock;
                sessionsListBox.Items.Add(listBoxItem);
            }
        }

        private void sessionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = ((ListBox)sender);
            ListBoxItem lbi = (ListBoxItem)lb.SelectedItem;
            testsListBox.Items.Clear();
            sessionIdSelected = (int)lbi.Tag;
            foreach (TestDB test in TestDB.getAll(sessionIdSelected))
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Tag = test.id;
                TextBlock textBlock = new TextBlock();
                Student student = Student.get(test.studentId);
                if (student == null) {
                    student = new Student("?", "?", 0);
                }
                textBlock.Text = student.id + " | " + student.fname + " " + student.lname + ": " + test.rAns + "/" + test.ansCount + " | " + Utils.getTimeFromInt(test.time);
                listBoxItem.Content = textBlock;
                testsListBox.Items.Add(listBoxItem);
            }
        }
    }
}
