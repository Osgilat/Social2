using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnucklesPickUp : MonoBehaviour
{
    [SerializeField]
    private GameObject item, equippedItem1, equippedItem2,
        butcher, thisLight, redLight;


    private void OnTriggerEnter(Collider other)
    {
        Destroy(item);
        thisLight.SetActive(false);

        redLight.SetActive(true);
        butcher.SetActive(true);

        equippedItem1.SetActive(true);
        equippedItem2.SetActive(true);
    }

}
