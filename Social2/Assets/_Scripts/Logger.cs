using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SiSubs;

public class Logger : MonoBehaviour
{

    public static void LogAction(String action, GameObject actor, GameObject target)
    {
        Debug.Log(
               DateTime.Now.ToString("M/d/yyyy") + " "
               + System.DateTime.Now.ToString("HH:mm:ss") + ":"
               + System.DateTime.Now.Millisecond + "," +
                String.Format(",{0},{1},{2}",

               actor.gameObject.tag,

               action,

               target == null ? "" : target.gameObject.tag));
        
    }
}
