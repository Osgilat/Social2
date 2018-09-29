using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSync : MonoBehaviour {


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
