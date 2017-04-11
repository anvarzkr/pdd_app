using Newtonsoft.Json;
using PDD_APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace PDD_APP
{
    public class SessionController
    {

        public bool testStarted = false;
        public bool sessionStarted = false;
        public bool sessionTerminated = false;
        public bool connectionOK = false;
        public String response = "EMPTY RESPONSE";
        public Thread threadTerminate;
        public Thread threadWUAC;
        public Thread threadWFS;
        private SessionConnectingPage sessionConnectingPage;
        public String hostIpAdress = "";
        public int hostPort = 11000;
        public List<Thread> threadList;
        public static SessionController sessionControllerStatic;

        public SessionController(SessionConnectingPage scPage, String hostIpAdress, int hostPort)
        {
            threadList = new List<Thread>();
            sessionConnectingPage = scPage;
            this.hostIpAdress = hostIpAdress;
            this.hostPort = hostPort;
        }

        public bool testConnection() {
            try
            {
                new AsynchronousClient(this).StartClient(hostIpAdress, hostPort, AsynchronousClient.Command.CONNECTION_TEST);
            }
            catch (Exception e) {
                return false;
            }
            Console.WriteLine(response);
            return true;
        }

        public void startSession()
        {
            sessionControllerStatic = this;
            try
            {
                threadWUAC = new Thread(() => { new AsynchronousClient(this).StartClient(hostIpAdress, hostPort, AsynchronousClient.Command.WUAC); });
                threadWUAC.IsBackground = true;
                threadWUAC.Start();
                threadList.Add(threadWUAC);

                threadTerminate = new Thread(() =>
                {
                    try
                    {
                        new Thread(() => new AsynchronousClient(this).StartClient(hostIpAdress, hostPort, AsynchronousClient.Command.TERMINATE)).Start();
                        while (!sessionTerminated)
                        {
                            try
                            {
                                Thread.Sleep(1000);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Thread was stopped");
                                Console.WriteLine(ex.ToString());
                            }
                        }
                        closeThreads();
                        sessionConnectingPage.Dispatcher.BeginInvoke(new Action(delegate()
                        {
                            sessionConnectingPage.sessionClosedBlock.Visibility = System.Windows.Visibility.Visible;
                            sessionConnectingPage.sessionWUAC.Visibility = System.Windows.Visibility.Collapsed;
                            sessionConnectingPage.studentInfoBorder.Visibility = System.Windows.Visibility.Collapsed;
                            sessionConnectingPage.backButton.Visibility = System.Windows.Visibility.Visible;
                        }));
                        if (TestingPage.currentTest != null)
                        {
                            if (TestingPage.currentTest.timer != null)
                            {
                                TestingPage.currentTest.timer.Stop();
                                TestingPage.currentTest.timer.Dispose();
                            }
                            Utils.goToPage("sessionConnectingPage");
                        }
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex);
                        try
                        {
                            closeThreads();
                        }
                        catch (Exception exx) { 
                            
                        }
                    }

                });
                threadTerminate.IsBackground = true;
                threadTerminate.Start();
                threadList.Add(threadTerminate);

                Console.WriteLine("threadTerminate.Start();");
                
                Thread threadWSNS = new Thread(() =>
                {
                    Console.WriteLine("threadWSNS - Session waitin for start");
                    while (!sessionStarted)
                    {
                        // TODO UNHANDELED EXCEPTION HERE
                        try
                        {
                            Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Thread.Sleep(1000) - EXCEPTION");
                        }
                    }
                    //MessageBox.Show(response);
                    Console.WriteLine("STUDENT - session started");
                    Student student = JsonConvert.DeserializeObject<Student>(response.Split('%')[1]);
                    TestingPage.studentStatic = student;

                    ///STUDENT INFO GEVEN

                    Console.WriteLine(response.Split('%')[1]);

                    sessionConnectingPage.Dispatcher.BeginInvoke(new Action(delegate()
                    {
                        sessionConnectingPage.sessionWUAC.Visibility = System.Windows.Visibility.Collapsed;
                        sessionConnectingPage.studentInfoBorder.Visibility = System.Windows.Visibility.Visible;
                        //sessionConnectingPage.studentInfo.Visibility = System.Windows.Visibility.Visible;
                        sessionConnectingPage.studentInfo.Text = student.fname + " " + student.lname;
                        sessionConnectingPage.backButton.Visibility = System.Windows.Visibility.Collapsed;
                    }));

                    //threadWFS = new Thread(() => { new AsynchronousClient(this).StartClient(hostIpAdress, hostPort, AsynchronousClient.Command.WFS); });
                    //threadWFS.IsBackground = true;
                    //threadWFS.Start();
                    //threadList.Add(threadWFS);

                    while (!testStarted)
                    {
                        //new Thread(() => new AsynchronousClient(this).StartClient(hostIpAdress, hostPort, AsynchronousClient.Command.IS_TEST_STARTED_CHECK)).Start();
                        new AsynchronousClient(this).StartClient(hostIpAdress, hostPort, AsynchronousClient.Command.IS_TEST_STARTED_CHECK);
                        try { Thread.Sleep(5000); } catch (Exception ex) { Console.WriteLine(ex); }
                    }

                    //Thread threadWTNS = new Thread(() =>
                    //{

                    //sessionConnectingPage.Dispatcher.BeginInvoke(new Action(delegate()
                    //{
                    //    sessionConnectingPage.backButton.Visibility = System.Windows.Visibility.Visible;
                    //}));

                    Console.WriteLine("Test Started!");

                    String tasksIdsString = response.Split('%')[0];
                    String[] tasksIdsStringArray = tasksIdsString.Split('&');
                    Task[] tasks = new Task[tasksIdsStringArray.Length];

                    for (int i = 0; i < tasksIdsStringArray.Length; i++)
                    {
                        int id = 0;
                        try { id = Int32.Parse(tasksIdsStringArray[i]); }
                        catch (Exception e) { id = Task.getLastId(); }
                        tasks[i] = Task.get(id);
                    }

                    TestingPage.sessionController = this;
                    //TestingPage.studentStatic = student;
                    TestingPage.tasksStatic = tasks;
                    //MessageBox.Show(student.fname + " " + student.lname + " " + student.id);
                    Utils.goToPage("TestingPage");

                    //});
                    //threadWTNS.Start();
                    //threadWTNS.IsBackground = true;
                    //threadList.Add(threadWTNS);
                });
                threadWSNS.Start();
                threadWSNS.IsBackground = true;
                threadList.Add(threadWSNS);

                Console.WriteLine("threadWSNS.Start();");
            }
            catch (Exception TAE)
            {
                Console.WriteLine("Thread closed.");
                Console.WriteLine(TAE.ToString());
            }
        }

        public void closeThreads()
        {
            try
            {
                if (threadWUAC != null && threadWUAC.IsAlive)
                    threadWUAC.Interrupt();
                if (threadWFS != null && threadWFS.IsAlive)
                    threadWFS.Interrupt();
                if (threadTerminate != null && threadTerminate.IsAlive)
                    threadTerminate.Interrupt();
                foreach (Thread thread in threadList)
                {
                    if (thread != null && thread.IsAlive)
                        thread.Interrupt();
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Console.WriteLine("All Threads Closed.");
        }
    }
}
