using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject: MonoBehaviour {

    public string itemName; 

	// Use this for initialization
	void Start () {
        itemName = gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
