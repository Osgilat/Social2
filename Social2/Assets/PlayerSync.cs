using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSync : MonoBehaviour {

    public bool initialisation = true;

    /*
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    */

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length > 1 &&
            initialisation)
        {
            Destroy(gameObject);
        } 

        initialisation = false;
        DontDestroyOnLoad(gameObject);
    }


}