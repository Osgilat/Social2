using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTrigger : MonoBehaviour {

    public bool triggered = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.activeInHierarchy && 
            !triggered)
        {
            Logger.LogAction("MirrorEntered", other.gameObject, null);
            triggered = true;
            other.gameObject.GetComponent<GameStates>().TriggerMirror();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VignetteControl>().ChangeVignetteAtEnd();
            other.transform.position = other.gameObject.GetComponent<GameStates>().initialPlayerPosition;
            gameObject.SetActive(false);
        }
    }
}
