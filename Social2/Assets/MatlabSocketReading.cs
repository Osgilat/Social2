using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using System.IO;


public class MatlabSocketReading : MonoBehaviour
{
    private bool mRunning;


    string msg = "";
    Thread mThread;
    TcpListener tcp_Listener = null;
    private string lastMsg;

    void Start()
    {
        mRunning = true;
        ThreadStart ts = new ThreadStart(SayHello);
        mThread = new Thread(ts);
        mThread.Start();
        Debug.Log("Thread done...");
    }

    public void stopListening()
    {
        mRunning = false;
    }

    public AIController aIController;

    void SayHello()
    {
        try
        {
            tcp_Listener = new TcpListener(55001);
            tcp_Listener.Start();
            print("Server Start");
            while (true)
            {
                // check if new connections are pending, if not, be nice and sleep 100ms
                if (!tcp_Listener.Pending())
                {
                    Thread.Sleep(100);
                }
                else
                {
                    TcpClient client = tcp_Listener.AcceptTcpClient();
                    NetworkStream ns = client.GetStream();
                    StreamReader reader = new StreamReader(ns);
                    msg = reader.ReadLine();

                    aIController.ChangeActionToExecute(msg);
                    
                    reader.Close();
                    client.Close();
                    
                }
            }
        }
        catch (ThreadAbortException)
        {
            print("exception");
        }
        finally
        {
            
            mRunning = false;
            tcp_Listener.Stop();
            
        }
    }

    void OnApplicationQuit()
    {
        // stop listening thread
        stopListening();
        // wait fpr listening thread to terminate (max. 500ms)
        mThread.Join(500);
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), lastMsg);
    }

}