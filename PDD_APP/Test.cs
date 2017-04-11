using PDD_APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace PDD_APP
{
    public class Test
    {
        public enum ANSWER {
            RIGHT,
            WRONG,
            NOT_ANSWERED
        };
        public ANSWER[] answered;
        public int[] studentAnswer;
        public int right_answered = 0;
        public int answeredCount = 0;
        public int timeCounterPassed = 0;
        public Timer timer;
        public Task[] tasks;
        private TestingPage testingPage;
        public int TEST_TIME = 1200;

        public Test(TestingPage testingPage) {
            this.testingPage = testingPage;
        }

        public void start() 
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        public void sendStats() 
        {
            new AsynchronousClient(TestingPage.sessionController).StartClient(SessionConnectingPage.hostIpAdress, SessionConnectingPage.hostPort, AsynchronousClient.Command.SEND_STATS);
        }

        public void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MainWindow.mainFrame.Dispatcher.BeginInvoke(new Action(delegate
            {
                timeCounterPassed += 1;
                for (int i = 0; i < answered.Length; i++)
                {
                    TextBlock textBlock = (TextBlock)testingPage.timeCounter[i].Child;
                    textBlock.Text = Utils.getTimeFromInt(TEST_TIME - testingPage.test.timeCounterPassed);
                }
                if (timeCounterPassed >= TEST_TIME) 
                {
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                    //mistakesClarificationPage.testingPage = testingPage;
                    Utils.goToPage("mistakesClarificationPage");
                }
            }));
        }
    }
}
