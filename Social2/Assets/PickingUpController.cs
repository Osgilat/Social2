using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingUpController : MonoBehaviour {

    public GameObject maul;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Maul":
                maul.SetActive(true);
                Destroy(other.gameObject);
                break;
            default:
                break;
        }


    }

}
