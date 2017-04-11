using PDD_APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
    /// Логика взаимодействия для SessionPage.xaml
    /// </summary>
    public partial class SessionPage : Page
    {
        public bool testStarted = false;
        public bool sessionStarted = false;
        public bool sessionTerminated = false;
        public int computerCount = 0;
        public Session session;
        private Thread serverThread;
        private AsynchronousSocketListener asl;
        public LinkedList<Student> studentList = new LinkedList<Student>();

        public SessionPage()
        {
            InitializeComponent();
            
            asl = new AsynchronousSocketListener(this);

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            terminateSession();
        }

        private void terminateSession() {
            if (asl.WUACThread != null)
                asl.WUACThread.Interrupt();
            if (serverThread != null)
                serverThread.Interrupt();
        }

        private void startSessionButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem lbi in studentListBox.SelectedItems) 
            {
                int id = (int)lbi.Tag;
                studentList.AddLast(Student.get(id));
            }
            computerCount = studentListBox.SelectedItems.Count;

            serverThread = new Thread(asl.StartListening);
            serverThread.Start();

            sessionStarted = true;
            startSessionButton.IsEnabled = false;
            terminateSessionButton.IsEnabled = true;
        }

        private void startTestButton_Click(object sender, RoutedEventArgs e)
        {
            //foreach (Session s in Session.getAll())
            //    Console.WriteLine(s.id.ToString());
            session = new Session(Utils.getCurrentTimeInMilliseconds());
            session.add();
            //foreach (Session s in Session.getAll())
            //    Console.WriteLine(s.id.ToString());
            testStarted = true;
            startSessionButton.IsEnabled = false;
            startTestButton.IsEnabled = false;
        }

        private void terminateSessionButton_Click(object sender, RoutedEventArgs e)
        {
            studentList.Clear();
            sessionTerminated = true;
            testStarted = false;
            sessionStarted = false;
            startSessionButton.IsEnabled = true;
            startTestButton.IsEnabled = false;
            terminateSessionButton.IsEnabled = false;
            Thread.Sleep(3000);
            asl.closeSessions();
            terminateSession();
        }

        private void printSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sessionStarted || testStarted) {
                MessageBox.Show("Невозможно выполнить данное действие пока запущена сессия тестирования!");
                return;
            }
            Utils.goToPage("SessionPrintingPage");
        }

        private void toMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (sessionStarted || testStarted)
            {
                MessageBox.Show("Невозможно выполнить данное действие пока запущена сессия тестирования!");
                return;
            }
            Utils.goToPage("RoleChoosePage");
        }

        private void studentList_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Student student in Student.getAll())
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Tag = student.id;
                TextBlock textBlock = new TextBlock();
                textBlock.Text = student.id + " | " + student.fname + " " + student.lname;
                listBoxItem.Content = textBlock;
                studentListBox.Items.Add(listBoxItem);
            }
        }

        private void studentAdd_Click(object sender, RoutedEventArgs e)
        {
            String fname = studentAddfNameBox.Text;
            String lname = studentAddlNameBox.Text;
            String studentIdString = studentAddIdBox.Text;
            int studentId = 0;

            if (fname.Equals("")) {
                MessageBox.Show("Поле с именем должно быть заполнено!");
                return;
            }
            if (lname.Equals(""))
            {
                MessageBox.Show("Поле с фамилией должно быть заполнено!");
                return;
            }

            bool tryparsed = false;

            if (int.TryParse(studentIdString, out studentId) == true)
            {
                if (Student.get(studentId) != null) {
                    MessageBox.Show("Участник с данных ID уже существует!");
                    return;
                }
                Student.add(new Student(fname, lname, 0), studentId);
                tryparsed = true;
            }
            else {
                Student.add(new Student(fname, lname, 0));
            }

            //Student.add(new Student(fname, lname, 0));

            Student student;

            if (tryparsed)
            {
                student = Student.get(studentId);
            }
            else {
                student = Student.get(Student.getLastId());
            }

            

            ListBoxItem listBoxItem = new ListBoxItem();
            listBoxItem.Tag = student.id;
            TextBlock textBlock = new TextBlock();
            textBlock.Text = student.id + " | " + student.fname + " " + student.lname;
            listBoxItem.Content = textBlock;
            studentListBox.Items.Add(listBoxItem);

            studentAddfNameBox.Text = "";
            studentAddlNameBox.Text = "";
            studentAddIdBox.Text = "";
        }

        private void removeStudents_Click(object sender, RoutedEventArgs e)
        {
            if (sessionStarted || testStarted) {
                MessageBox.Show("Нельзя удалять участников, пока запущена сессия тестирования!");
                return;
            }
            int count = 0;

            foreach (ListBoxItem lbi in studentListBox.SelectedItems)
                count++;

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить " + count + " выбранных участников? Действие необратимо!", "УДАЛЕНИЕ УЧАСТНИКОВ", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                List<ListBoxItem> listBoxItemsToRemove = new List<ListBoxItem>();
                foreach (ListBoxItem lbi in studentListBox.SelectedItems)
                {
                    int id = (int)lbi.Tag;
                    Student.remove(id);
                    listBoxItemsToRemove.Add(lbi);
                }
                foreach (ListBoxItem lbi in listBoxItemsToRemove)
                    studentListBox.Items.Remove(lbi);
            }

        }

        private void removeAllStudents_Click(object sender, RoutedEventArgs e) {
            if (sessionStarted || testStarted)
            {
                MessageBox.Show("Нельзя удалять участников, пока запущена сессия тестирования!");
                return;
            }

            int count = 0;

            foreach (ListBoxItem lbi in studentListBox.Items)
                count++;

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить " + count + " выбранных участников? Действие необратимо!", "УДАЛЕНИЕ УЧАСТНИКОВ", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                List<ListBoxItem> listBoxItemsToRemove = new List<ListBoxItem>();
                foreach (ListBoxItem lbi in studentListBox.Items)
                {
                    int id = (int)lbi.Tag;
                    Student.remove(id);
                    listBoxItemsToRemove.Add(lbi);
                }
                foreach (ListBoxItem lbi in listBoxItemsToRemove)
                    studentListBox.Items.Remove(lbi);
            }

            try
            {
                DBManager.execute("delete from students");
                DBManager.execute("delete from sqlite_sequence where name = 'students'");
            }
            catch (Exception ex) {
                Console.WriteLine(e.ToString());
            }
        }
    }
    
}
