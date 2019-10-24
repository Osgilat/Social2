using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSync : MonoBehaviour {
    
    public bool initialisation = true;


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