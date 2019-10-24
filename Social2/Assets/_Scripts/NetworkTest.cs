using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{

    IEnumerator Start()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("header-name", "header content");
        WWW www = new WWW("https://example.com", null, headers);
        yield return www;
        Debug.Log(www.text);
    }

}
