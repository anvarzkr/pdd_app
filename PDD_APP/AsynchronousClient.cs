using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Newtonsoft.Json;
using PDD_APP.Models;
using System.Windows;

namespace PDD_APP
{
    // State object for receiving data from remote device.
    /*public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }*/

    public class AsynchronousClient
    {
        public enum Command
        {
            WUAC,
            WFS,
            TERMINATE,
            CONNECTION_TEST,
            SEND_STATS,
            HELP,
            IS_TEST_STARTED_CHECK
        }
        // The port number for the remote device.
        private const int port = 11000;

        // ManualResetEvent instances signal completion.
        private ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        // The response from the remote device.
        public String response = String.Empty;
        public String responseStudentAndTasks = String.Empty;
        private SessionController sessionController;
        private bool connectionFailed = false;

        public AsynchronousClient(SessionController _sessionCotroller) {
            sessionController = _sessionCotroller;
        }

        public void StartClient(String hostIpAdress, int hostPort, Command command)
        {
            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // The name of the
                // remote device is "host.contoso.com".
                Console.WriteLine(hostIpAdress);
                IPHostEntry ipHostInfo = Dns.Resolve(hostIpAdress);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                foreach (IPAddress ip in ipHostInfo.AddressList) {
                    Console.WriteLine(ip.ToString());
                }
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, hostPort);

                // Create a TCP/IP socket.
                Socket client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();
                if (connectionFailed)
                    throw new Exception();

                Console.WriteLine("Connected to: " + client.RemoteEndPoint.ToString());

                switch (command)
                {
                    case Command.WUAC:
                        sendWUAC(client);
                        break;
                    case Command.WFS:
                        sendWFS(client);
                        break;
                    case Command.TERMINATE:
                        sendTerminate(client);
                        break;
                    case Command.CONNECTION_TEST:
                        connectionTest(client);
                        break;
                    case Command.SEND_STATS:
                        sendStats(client);
                        break;
                    case Command.HELP:
                        help(client);
                        break;
                    case Command.IS_TEST_STARTED_CHECK:
                        isTestStartedCheck(client);
                        break;
                }

                // Release the socket.
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("In catch of AsynchronousClient");
                Console.WriteLine(e.ToString());
                if (connectionFailed)
                {
                    Console.WriteLine("Connection Failed");
                    //throw;
                }
            }
        }

        private void isTestStartedCheck(Socket client)
        {
            // Send test data to the remote device.
            Send(client, "CHECK");
            sendDone.WaitOne();

            // Receive the response from the remote device.
            Receive(client);
            receiveDone.WaitOne();

            if (!response.Contains("NO"))
            {
                //Console.WriteLine(response);
                Student student = JsonConvert.DeserializeObject<Student>(response);
                //MessageBox.Show(student.id + ": " + student.fname + " " + student.lname);
                TestingPage.studentStatic = student;
                Console.WriteLine("TEST IS STARTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                sessionController.testStarted = true;
            }

            // Write the response to the console.
            Console.WriteLine("Response received : {0}", response);
        }

        private void help(Socket client)
        {
            // Send test data to the remote device.
            Send(client, "HELP^" + TestingPage.studentStatic.id);
            sendDone.WaitOne();

            // Receive the response from the remote device.
            Receive(client);
            receiveDone.WaitOne();

            if (response.Contains("HELP"))
                MessageBox.Show("Вы позвали судью.");

            // Write the response to the console.
            Console.WriteLine("Response received : {0}", response);
        }

        private void sendStats(Socket client)
        {
            // Send test data to the remote device.
            String message = JsonConvert.SerializeObject(TestingPage.studentStatic) + "%" 
                + TestingPage.currentTest.timeCounterPassed + "&" 
                + TestingPage.currentTest.right_answered + "&" 
                + TestingPage.currentTest.tasks.Length;
            Send(client, "SENDSTATS^" + message);
            sendDone.WaitOne();

            // Receive the response from the remote device.
            Receive(client);
            receiveDone.WaitOne();

            // Write the response to the console.
            Console.WriteLine("Response received : {0}", response);
        }

        private void connectionTest(Socket client)
        {
            // Send test data to the remote device.
            Send(client, "CTEST");
            sendDone.WaitOne();

            Console.WriteLine("connectionTest");

            // Receive the response from the remote device.
            Receive(client);
            receiveDone.WaitOne();

            // Write the response to the console.
            Console.WriteLine("Response received : {0}", response);

            if (!response.Equals("NO"))
            {
                sessionController.connectionOK = true;
            }
        }

        private void sendWUAC(Socket client)
        {
            // Send test data to the remote device.
            Send(client, "WUAC");
            sendDone.WaitOne();

            Console.WriteLine("sendWUAC");

            // Receive the response from the remote device.
            Receive(client);
            receiveDone.WaitOne();

            // Write the response to the console.
            Console.WriteLine("Response received : {0}", response);

            if (!response.Equals("NO"))
            {
                sessionController.response = response;
                sessionController.sessionStarted = true;
            }
        }

        private void sendWFS(Socket client)
        {
            // Send test data to the remote device.
            Send(client, "WFS");
            sendDone.WaitOne();

            Console.WriteLine("sendWFS");

            // Receive the response from the remote device.
            Receive(client);
            receiveDone.WaitOne();

            // Write the response to the console.
            Console.WriteLine("Response received : {0}", response);

            if (response.Equals("WFSOK"))
            {
                sessionController.testStarted = true;
            }
        }

        private void sendTerminate(Socket client)
        {
            // Send test data to the remote device.
            Send(client, "TERMINATE");
            sendDone.WaitOne();

            Console.WriteLine("sendTerminate");

            // Receive the response from the remote device.
            Receive(client);
            receiveDone.WaitOne();

            // Write the response to the console.
            Console.WriteLine("Response received : {0}", response);

            if (response.Equals("TERMINATE"))
            {
                sessionController.sessionTerminated = true;
            }
        }

        private void ConnectCallback(IAsyncResult ar) 
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.
                connectDone.Set();
            }
            catch (Exception e)
            {
                connectionFailed = true;
                connectDone.Set();
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    //state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            //byte[] byteData = Encoding.ASCII.GetBytes(data + "<EOF>");
            byte[] byteData = Encoding.UTF8.GetBytes(data + "<EOF>");

            // Begin sending the data to the remote device.
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
