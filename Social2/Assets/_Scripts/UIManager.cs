using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager: NetworkBehaviour {

    //References to buttons
    public GameObject askButton;
    public GameObject kickButton;

    //References to HUD 
    public GameObject announcementText;
    public GameObject canvas;

    // Use this for initialization
    void Start ()
    {
        Invoke("InstantiateButtons", 2f);
    }

    
    //Function for buttons and announcement
    void InstantiateButtons()
    {
   

        //Instantiate canvas and get references to it's buttons and announcement text
        Transform thisCanvas = Instantiate(canvas).transform;
        askButton = thisCanvas.GetChild(0).gameObject;
        kickButton = thisCanvas.GetChild(1).gameObject;

        //Set announcement active for a first round
        announcementText.SetActive(true);
        
    }

    public static Hashtable buttons;


}
