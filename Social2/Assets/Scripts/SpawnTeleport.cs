using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SpawnTeleport : NetworkBehaviour
{

    public GameObject Platform_1;
    public GameObject Platform_2;
    public GameObject Generator;
    public GameObject GameManager;
    public GameObject UI_manager;

    GameObject p_1;
    GameObject p_2;
    GameObject gen;
    GameObject gameManager;
    GameObject ui_manager;


    public override void OnStartServer()
    {
        /*
        ui_manager = (GameObject)Instantiate(UI_manager);

        NetworkServer.Spawn(ui_manager);
        */
    }

}
