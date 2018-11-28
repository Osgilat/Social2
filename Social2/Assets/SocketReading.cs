using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class SocketReading
{
    public class PacketMessage : MessageBase
    {
        public string messageType;
        public string payload;
    }

    /// <summary>
    /// Point this to your own handler to process messages
    /// </summary>
    public Action<string> HandleMessage;

    private Thread ListenerThread = null;
    private bool KeepListening = true;

    public SocketReading()
    {
        /*
        HandleMessage = (p) =>
        {
            Debug.Log("Did not handle message: " + p);
        };
        */

        ListenerThread = new Thread(ListenWorker);
        ListenerThread.Start();
    }

    public bool streamFlag = false;

    private void ListenWorker()
    {
        KeepListening = true;
        var dataBuffer = new StringBuilder();
        var receiveBuffer = new byte[0x10000]; // Read 64KB at a time

        // Set up a local socket for listening
        using (var localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            // Set up an endpoint and start listening
            var localEndpoint = new IPEndPoint(IPAddress.Any, 55001);
            localSocket.Bind(localEndpoint);
            localSocket.Listen(10);
            Debug.Log("Socket Standby....");

            while (KeepListening)
            {

                // Clear input buffer (Assumption: messages are always string data)
                dataBuffer.Remove(0, dataBuffer.Length);

                // This call will block until we get a message. Using Async methods
                // will have better performance, but this is simpler
                var remoteSocket = localSocket.Accept();
                Debug.Log("Socket Connected.");

                // Connect to the remote client and receive the message as text
                var remoteEndpoint = (IPEndPoint)remoteSocket.RemoteEndPoint;
                var receiveStream = new NetworkStream(remoteSocket);

                 while (receiveStream.Read(receiveBuffer, 0, receiveBuffer.Length) > 0)
                {

                    var data = Encoding.Default.GetString(receiveBuffer);
                    dataBuffer.Append(data);
                    break;
                }

                // Here we assume the remote client is sending us JSON data that describes
                // a PacketMessage object.  Deserialize the Json and call our custom handler
                //var message = JsonUtility.FromJson<PacketMessage>(dataBuffer.ToString());
                HandleMessage(dataBuffer.ToString());

                /*
                try
                {
                    // Clear input buffer (Assumption: messages are always string data)
                    dataBuffer.Remove(0, dataBuffer.Length);

                    // This call will block until we get a message. Using Async methods
                    // will have better performance, but this is simpler
                    var remoteSocket = localSocket.Accept();
                    Debug.Log("Socket Connected.");

                    // Connect to the remote client and receive the message as text
                    var remoteEndpoint = (IPEndPoint)remoteSocket.RemoteEndPoint;
                    var receiveStream = new NetworkStream(remoteSocket);
                    
                    while (receiveStream.Read(receiveBuffer, 0, receiveBuffer.Length) > 0)
                    {

                        var data = Encoding.Default.GetString(receiveBuffer);
                        dataBuffer.Append(data);
                        break;
                    }

                    // Here we assume the remote client is sending us JSON data that describes
                    // a PacketMessage object.  Deserialize the Json and call our custom handler
                    //var message = JsonUtility.FromJson<PacketMessage>(dataBuffer.ToString());
                    HandleMessage(dataBuffer.ToString());
                }
                catch (Exception e)
                {
                    // report errors and keep listening.
                    Debug.Log("Network Error: " + e.Message);

                    // Sleep 5 seconds so that we don't flood the output with errors
                    Thread.Sleep(5000);
                }
                
            */
            }
        }
    }
}