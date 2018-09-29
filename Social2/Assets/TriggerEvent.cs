using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour {

    public UnityEvent eventToInvoke;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && other.gameObject.activeInHierarchy)
        {
            eventToInvoke.Invoke();
        }
    }
}
