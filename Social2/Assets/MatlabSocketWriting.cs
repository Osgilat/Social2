using System.Collections;
using System.Net;
using System.Net.Sockets;
using System;
using System.IO;
using UnityEngine;
using System.Text;

public class MatlabSocketWriting : MonoBehaviour
{
    // Use this for initialization
    internal Boolean socketReady = false;
    public TcpClient mySocket;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;
    String Host = "localhost";
    Int32 Port = 55000;

    public static MatlabSocketWriting instance;

    
  
    void Start()
    {
        instance = this;
        SetupSocket();
        Debug.Log("Socket initialized");
    }

    public bool triggerSocket = false;
    // Update is called once per frame
    void Update()
    {
        if (triggerSocket)
        {
            triggerSocket = false;

            Byte[] sendBytes = Encoding.UTF8.GetBytes("Testing unity sockets");
            mySocket.GetStream().Write(sendBytes, 0, sendBytes.Length);
        }
    }
    public void SetupSocket()
    {
        try
        {
            mySocket = new TcpClient(Host, Port);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
            socketReady = true;
            
            Debug.Log("Socket success");
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
        }
    }
}