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
            triggered = true;
            other.gameObject.GetComponent<Health>().TriggerMirror();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VignetteControl>().ChangeVignetteAtEnd();
            other.transform.position = other.gameObject.GetComponent<Health>().initialPlayerPosition;
            gameObject.SetActive(false);
        }
    }
}
