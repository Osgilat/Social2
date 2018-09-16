using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : NetworkBehaviour
{

    //PlayerID for identification between sessions
    [SyncVar]
    public string playerID = "noPlayerID";
    public static GameObject localPlayerGameObject;
    public Material localPlayerMaterial;
    public GameObject objectToPaint;

    public GameObject playerToTarget1 = null;
    public GameObject playerToTarget2 = null;

    void Awake()
    {
        localPlayerGameObject = gameObject;
    }

    // Use this for initialization
    void Start ()
    {

        string temp = gameObject.name.Remove(11);
        playerID = temp.Replace("Mod_", " ");

        localPlayerGameObject = gameObject;

    }

    public override void OnStartLocalPlayer()
    {
        localPlayerGameObject = gameObject;
        objectToPaint.GetComponent<MeshRenderer>().material = localPlayerMaterial;
    }

    public bool IsTrueLocalPlayer()
    {
        
        if (gameObject.GetComponent<Text>().text !=
            localPlayerGameObject.GetComponent<Text>().text)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
