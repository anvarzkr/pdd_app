using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using PDD_APP.Models;
using Newtonsoft.Json;

namespace PDD_APP
{
    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }


    public class AsynchronousSocketListener
    {
        private List<Socket> connections = new List<Socket>();
        private List<Socket> connectionsToTerminate = new List<Socket>();
        private bool isAllConnected = false;
        public Thread WUACThread;
        // Thread signal.
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public Dictionary<String, Student> studentListSocket = new Dictionary<String, Student>();

        public Socket listener;
        public SessionPage sp;

        private static Object studentLocker = new Object();

        public AsynchronousSocketListener(SessionPage sp)
        {
            this.sp = sp;
        }

        public void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            lock (connections)
            {
                connections.Clear();
            }
            lock (connectionsToTerminate)
            {
                connectionsToTerminate.Clear();
            }
            isAllConnected = false;
            studentListSocket = new Dictionary<String, Student>();

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.
            listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                //WUACThread = new Thread(waitUntilAllConnected);
                //WUACThread.Start();

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                try
                {
                    listener.Close();
                    listener.Dispose();
                }
                catch (Exception ex) {
                    Console.WriteLine("Listener Closing Problem");
                }
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Signal the main thread to continue.
                allDone.Set();

                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                //connections.Add(handler);
                Console.WriteLine("Connected: " + handler.RemoteEndPoint.ToString());

                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e) {
                Console.WriteLine("AcceptCallback Exception");
            }
        }

        public void ReadCallback(IAsyncResult ar)
        {
            try
            {
                String content = String.Empty;

                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.
                    //state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = state.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        // All the data has been read from the 
                        // client. Display it on the console.
                        Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                            content.Length, content);
                        // Echo the data back to the client.

                        if (content.Contains("WUAC"))
                        {
                            Console.WriteLine("WUAC start");
                            lock (connections)
                            {
                                connections.Add(handler);
                            }
                            //while (!isAllConnected || !sp.sessionStarted)
                            //{
                            //    try
                            //    {
                            //        Thread.Sleep(1000);
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Console.WriteLine("Thread was stopped");
                            //        Console.WriteLine(ex.ToString());
                            //    }
                            //}
                            //Console.WriteLine("WUAC all connected");
                            //try
                            //{
                            //    WUACThread.Interrupt();
                            //}
                            //catch (Exception e) {
                            //    Console.WriteLine("WUACThread.Interrupt();");
                            //}
                            Student student;
                            sp.Dispatcher.BeginInvoke(new Action(delegate ()
                            {
                                sp.startSessionButton.IsEnabled = false;
                                sp.startTestButton.IsEnabled = true;
                            }));
                            lock (studentLocker)
                            {
                                Console.WriteLine("STUDENT LOCKER ------------------------------<<<<<<<<<<<<<<<<<<");
                                if (sp.studentList.Count > 0)
                                {
                                    student = sp.studentList.First.Value;
                                    Console.WriteLine("WUAC graphics");
                                    sp.studentList.RemoveFirst();
                                }
                                else {
                                    student = new Student("?", "?", 0);
                                }
                                studentListSocket.Add(handler.RemoteEndPoint.ToString().Split(':')[0], student);
                                //Student studentToCheck;
                                //studentListSocket.TryGetValue(handler.RemoteEndPoint.AddressFamily.ToString(), out studentToCheck);
                                //Console.WriteLine(studentToCheck.id + ": " + studentToCheck.fname + " and " + studentToCheck.lname);
                                //sp.Dispatcher.BeginInvoke(new Action(delegate ()
                                //{
                                //    Console.WriteLine("WUAC graphics");
                                //    sp.studentList.RemoveFirst();
                                //    sp.startSessionButton.IsEnabled = false;
                                //    sp.startTestButton.IsEnabled = true;
                                //}));
                            }
                            Console.WriteLine("WUAC sending..");
                            //String studentJSON = "{\"id\":" + student.id + ",\"fname\":\"" + student.fname + "\",\"lname\":\"" + student.lname + "\",\"score\":0}";
                            
                            if (sp.sessionStarted)
                                Send(handler, (getTasksId() + "%" + JsonConvert.SerializeObject(student)));
                            
                            //MessageBox.Show("Задания отправлены");
                            //Send(handler, (getTasksId() + "%" + studentJSON));
                            Console.WriteLine("WUAC sent..");
                        }
                        else if (content.Contains("WFS"))
                        {
                            Console.WriteLine("Wait For Start start");
                            while (!sp.testStarted)
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
                            Console.WriteLine("Wait For Start test started");
                            Send(handler, "WFSOK");
                        }
                        else if (content.Contains("TERMINATE"))
                        {
                            lock (connectionsToTerminate)
                            {
                                connectionsToTerminate.Add(handler);
                            }
                            Console.WriteLine("Waiting to terminate");
                            while (!sp.sessionTerminated)
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
                            Console.WriteLine("Session started to terminating");
                            sp.Dispatcher.BeginInvoke(new Action(delegate()
                            {
                                sp.helpBlock.Text = "";
                                sp.helpStackPanel.Visibility = Visibility.Collapsed;
                            }));
                            Send(handler, "TERMINATE");
                            try
                            {
                                Thread.Sleep(3000);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Thread was stopped");
                                Console.WriteLine(ex.ToString());
                            }
                            closeSessions();
                        }
                        else if (content.Contains("CTEST"))
                        {
                            Send(handler, "OK");
                        }
                        else if (content.Contains("SENDSTATS"))
                        {
                            content = content.Split('^')[1];
                            //Console.WriteLine(content);
                            String studentJson = content.Split('%')[0];
                            //Console.WriteLine(studentJson);
                            Student student = JsonConvert.DeserializeObject<Student>(studentJson);
                            //Console.WriteLine(content.Split('%')[1].Split('&')[0]);
                            //Console.WriteLine(content.Split('%')[1].Split('&')[1]);
                            //Console.WriteLine(content.Split('%')[1].Split('&')[2]);
                            int time = Int32.Parse(content.Split('%')[1].Split('&')[0]);
                            int rAns = Int32.Parse(content.Split('%')[1].Split('&')[1]);
                            int ansCount = Int32.Parse(content.Split('%')[1].Split('&')[2].Replace("<EOF>", ""));
                            /*sp.Dispatcher.BeginInvoke(new Action(delegate()
                            {
                                sp.stats.Text += student.id + " | " + student.fname + " " + student.lname + ": " + rAns + "/20 " + Utils.getTimeFromInt(time) + Environment.NewLine;
                            }));*/
                            Student studentToCheck;
                            if (!studentListSocket.TryGetValue(handler.RemoteEndPoint.ToString().Split(':')[0], out studentToCheck))
                            {
                                studentToCheck = new Student("?", "?", 0);
                            }
                            new TestDB(sp.session.id, time, studentToCheck.id, rAns, ansCount).add();
                            Send(handler, "OK");
                        } else if (content.Contains("HELP"))
                        {
                            String studentIdString = content.Split('^')[1].Replace("<EOF>", "");
                            int studentId = Int32.Parse(studentIdString);
                            Student student = Student.get(studentId);
                            sp.Dispatcher.BeginInvoke(new Action(delegate()
                            {
                                sp.helpBlock.Text += student.id + " | " + student.fname + " " + student.lname + Environment.NewLine;
                                sp.helpStackPanel.Visibility = Visibility.Visible;
                            }));
                            Send(handler, "HELP");
                        }
                        else if (content.Contains("CHECK"))
                        {
                            String response = "NO";

                            Student studentToCheck;

                            if (sp.testStarted)
                            {
                                if (!studentListSocket.TryGetValue(handler.RemoteEndPoint.ToString().Split(':')[0], out studentToCheck))
                                {
                                    studentToCheck = new Student("?", "?", 0);
                                }

                                response = JsonConvert.SerializeObject(studentToCheck);

                            }

                            Send(handler, response);
                        }
                    }
                    else
                    {
                        // Not all data received. Get more.
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("ReadCallback Exception");
            }
        }

        public void Send(Socket handler, String data)
        {
            try
            {

                // Convert the string data to byte data using ASCII encoding.
                //byte[] byteData = Encoding.ASCII.GetBytes(data);
                byte[] byteData = Encoding.UTF8.GetBytes(data);

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                        new AsyncCallback(SendCallback), handler);
            }
            catch (Exception e) {
                Console.WriteLine("Send method EXCEPTION ========================");
                Console.WriteLine(e.ToString());
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Receive);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private String getTasksJson() 
        {
            Task[] tasksArray = Task.getRandomTasks(15);
            List<Task> taskList = tasksArray.ToList();
            String json = JsonConvert.SerializeObject(taskList);
            return json;
        }
        private String getTasksId() 
        {
            //MessageBox.Show("Даю задания участникам");
            String returnString = "";
            //for (int i = 0; i < 20; i++)
            //{
            //    returnString += 1 + ((i != 19) ? "&" : "");
            //}
            //Console.WriteLine("I AM GETTING TASKS FOR PARTICIPANT");

            Random random = new Random();

            for (int i = 0; i < 20; i++)
            {
                int rand = (random.Next(5) + 1) + i * 5;
                returnString += rand + ((i != 19) ? "&" : "");
            }

            //Task[] tasksArray = Task.getTestingTasks();
            ////Task[] tasksArray = Task.getRandomTasks(20);
            //for (int i = 0; i < tasksArray.Length; i++) 
            //{
            //    //Console.WriteLine(i);
            //    //Console.WriteLine(tasksArray[i].id);
            //    returnString += tasksArray[i].id + ((i != tasksArray.Length - 1) ? "&" : "");
            //}
            //MessageBox.Show("Дал задания участникам");
            return returnString;
        }

        private void waitUntilAllConnected() {
            try
            {
                List<Socket> connectionsToRemove = new List<Socket>();
                while (!isAllConnected)
                {
                    int connCount = 0;
                    lock (connections)
                    {
                        foreach (Socket conn in connections)
                        {
                            if (!isSocketConnected(conn))
                                connectionsToRemove.Add(conn);
                            else
                                connCount++;
                        }
                    
                        connectionsToRemove.ForEach(conn => connections.Remove(conn));
                    }
                    if (connCount == sp.computerCount)
                        isAllConnected = true;
                    else
                        isAllConnected = false;
                    Console.WriteLine(connCount + "/" + sp.computerCount);
                    sp.studentsCount.Text = connCount + "/" + sp.computerCount;
                    try
                    {
                        Thread.Sleep(3000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Thread was stopped");
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("WUAC Thread interrupted. ((((((((((((((((((((((((((((((((((((((((((((");
                Console.WriteLine(ex);
            }
        }

        public void closeSessions() 
        {
            try {
                lock (connections)
                {
                    foreach (Socket conn in connections)
                    {
                        try
                        {
                            conn.Shutdown(SocketShutdown.Both);
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception in CLOSING SESSION LOOP");
                        }
                    }
                }
                lock (connectionsToTerminate)
                {
                    foreach (Socket conn in connectionsToTerminate)
                    {
                        try
                        {
                            conn.Shutdown(SocketShutdown.Both);
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception in CLOSING SESSION LOOP");
                        }
                    }
                }
                //foreach (Socket conn in connectionsToTerminate)
                //{
                //    if (isSocketConnected(conn))
                //    {
                //        conn.Shutdown(SocketShutdown.Both);
                //        conn.Close();
                //    }
                //}
                //foreach (Socket conn in connections)
                //{
                //    if (isSocketConnected(conn))
                //    {
                //        conn.Shutdown(SocketShutdown.Both);
                //        conn.Close();
                //    }
                //}
                listener.Close();
                listener.Dispose();
                sp.sessionTerminated = false;
                sp.sessionStarted = false;
                sp.testStarted = false;
            }
            catch (Exception ex) {
                Console.WriteLine("Exception in CLOSING SESSION");
            }
        }

        private bool isSocketConnected(Socket socket) 
        {
            if (socket == null)
                return false;

            return !(!socket.Connected || (socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)));
        }
    }
}
