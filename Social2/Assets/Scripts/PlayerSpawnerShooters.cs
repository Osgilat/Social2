using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerSpawnerShooters : NetworkManager {

	[SerializeField] public GameObject playerA;
    public Brain brain;

    List<GameObject> meshes = new List<GameObject> ();

    //Used to store chars for players
    List<string> chars = new List<string>(); 

	void Start()
	{
        NetworkManager.singleton.StartHost();
        meshes.Add(playerA);
    }

	//subclass for sending network messages
	public class NetworkMessage : MessageBase {
		public int chosenClass;
	}


    
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {
		NetworkMessage message = extraMessageReader.ReadMessage< NetworkMessage>();
        
		//Store players in a scene in array
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        if (meshes.Count != 0)
        {

            int randCharIndex = Random.Range(0, meshes.Count);

            //Used for a local player representation
            GameObject localPlayer = null;


            Transform spawn = NetworkManager.singleton.GetStartPosition();

            localPlayer = Instantiate(meshes[randCharIndex], spawn.position, spawn.rotation) as GameObject;


            meshes.RemoveAt(randCharIndex);

            NetworkServer.AddPlayerForConnection(conn, localPlayer, playerControllerId);

        }

    }
    
	public override void OnClientConnect(NetworkConnection conn) {
		NetworkMessage test = new NetworkMessage();
		ClientScene.AddPlayer(conn, 0, test);
	}
}
