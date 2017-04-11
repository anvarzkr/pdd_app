using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PDD_APP.Models;
using System.Timers;

namespace PDD_APP
{
    /// <summary>
    /// Логика взаимодействия для TestingPage.xaml
    /// </summary>
    public partial class TestingPage : Page
    {
        public Border[] borderTabs;
        public Border[] timeCounter;
        public Border singleTimeCounter;
        public Border currentConfirmButton;
        public Student currentStudent;
        public Task[] tasks;
        public Test test;
        public static Student studentStatic;
        public static Task[] tasksStatic;
        public static SessionController sessionController;
        public static Test currentTest;
        public bool isHelp2Used = false;

        public TestingPage()
        {
            mistakesClarificationPage.testingPage = this;
            InitializeComponent();

            xamlInit();

            tasksStatic = new Task[] { Task.get(76) };
            tasksStatic = Task.getTestingTasks();
            studentStatic = new Student("Anvar", "Zakirov", 0);
            tasksStatic = Task.getTestingTasks();

            if (tasksStatic != null && studentStatic != null)
                init(tasksStatic, studentStatic);
        }

        private void xamlInit() 
        {
            mainGrid.Visibility = System.Windows.Visibility.Collapsed;
            info1.Background = new SolidColorBrush(Utils.lightBlueColor1);
            info1Border.BorderBrush = new SolidColorBrush(Utils.darkBlueColor2);
            info1Border.Height = Utils.windowHeight;
            info1Border.Width = Utils.windowWidth;
            info1_border1.Background = new SolidColorBrush(Utils.darkBlueColor2);
            info1_border2.Background = new SolidColorBrush(Utils.darkBlueColor3);
            startButton.Background = new SolidColorBrush(Utils.darkGreenColor2);

            info2.Background = new SolidColorBrush(Utils.lightBlueColor1);
            info2Border.BorderBrush = new SolidColorBrush(Utils.darkBlueColor2);
            info2Border.Height = Utils.windowHeight;
            info2Border.Width = Utils.windowWidth;
            info2_border1.Background = new SolidColorBrush(Utils.darkBlueColor2);
            info2_border2.Background = new SolidColorBrush(Utils.darkBlueColor3);
            nextButton.Background = new SolidColorBrush(Utils.darkGreenColor2);
        }

        private void init(Task[] tasks, Student student) 
        {
            this.tasks = tasks;
            this.currentStudent = student;

            test = new Test(this);
            test.tasks = tasks;
            test.start();
            currentTest = test;

            showTest();
        }

        private void showTest()
        {
            TestingPageConstructor tpc = new TestingPageConstructor(this);

            borderTabs = new Border[tasks.Length];
            timeCounter = new Border[tasks.Length];
            test.answered = new Test.ANSWER[tasks.Length];
            test.studentAnswer = new int[tasks.Length];
            for (int i = 0; i < test.answered.Length; i++)
                test.answered[i] = Test.ANSWER.NOT_ANSWERED;

            StackPanel tabControls = tpc.getTabControls(tasks.Length, false, true);
            tabControls.Orientation = Orientation.Horizontal;
            tabControls.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            headerGrid.Children.Add(tabControls);
            currentConfirmButton = tpc.getConfirmButton();
            Border timerBorder = tpc.getTimer("");
            Grid.SetColumn(currentConfirmButton, 1);
            Grid.SetColumn(timerBorder, 2);
            headerGrid.Children.Add(currentConfirmButton);
            headerGrid.Children.Add(timerBorder);
            mainGrid.Background = new SolidColorBrush(Utils.backgroundColor);
            tpc.changeTab(0);

            for (int i = 0; i < tasks.Length; i++) 
            {
                TabItem tabItem = TestingPageConstructor.getInvisibleTabItem();
                tabItem.Content = tpc.getTaskContent(tasks[i], i);
                tabControl.Items.Add(tabItem);
            }
        }

        private void startButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkGreenColor1);
        }

        private void startButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Utils.darkGreenColor2);
        }

        private void startButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (test.timer == null)
                test.start();

            info1.Visibility = System.Windows.Visibility.Collapsed;
            info2.Visibility = System.Windows.Visibility.Collapsed;
            mainGrid.Visibility = System.Windows.Visibility.Visible;
        }

    }
}