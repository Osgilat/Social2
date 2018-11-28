using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SiSubs;
using System.Text;

public class Logger : MonoBehaviour
{

    static Byte[] sendBytes;

    public static void LogAction(String action, GameObject actor, GameObject target)
    {
        string logMessage = DateTime.Now.ToString("M/d/yyyy") + " "
               + System.DateTime.Now.ToString("HH:mm:ss") + ":"
               + System.DateTime.Now.Millisecond + "," +
                String.Format(",{0},{1},{2}",

               actor.gameObject.tag,

               action,

               target == null ? "" : target.gameObject.tag);

        Debug.Log(logMessage);

        sendBytes = Encoding.UTF8.GetBytes(logMessage);
        if (MatlabSocketWriting.instance != null)
        {
            MatlabSocketWriting.instance.mySocket.GetStream().Write(sendBytes, 0, sendBytes.Length);
        }

    }
}
