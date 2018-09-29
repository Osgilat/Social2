using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingUpController : MonoBehaviour
{

    public GameObject sword;
    public GameObject shield;
    public GameObject potion;
    public GameObject victoryClip;

    public void UnequipAll()
    {
        sword.SetActive(false);
        shield.SetActive(false);
        potion.SetActive(false);
        GetComponent<Health>().swordEquiped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Sword":
                sword.SetActive(true);
                shield.SetActive(true);
                GetComponent<Health>().swordEquiped = true;
                other.gameObject.SetActive(false);
                break;
            case "Potion":
                potion.SetActive(true);
                Destroy(other.gameObject);
                break;
            case "RealTreasure":
                other.gameObject.GetComponent<Chest>().openedChest.SetActive(true);
                other.gameObject.GetComponent<Chest>().closedChest.SetActive(false);
                StartCoroutine(WinGame());

                break;
            case "FakeTreasure":
                other.gameObject.GetComponent<Chest>().openedChest.SetActive(true);
                other.gameObject.GetComponent<Chest>().closedChest.SetActive(false);
                break;
            default:
                break;
        }


    }


    IEnumerator WinGame()
    {

        yield return new WaitForSeconds(5);
        victoryClip.SetActive(true);
        yield return new WaitForSeconds(10);
        gameObject.GetComponent<Health>().healthPoints = 0;
        yield return new WaitForSeconds(10);
        victoryClip.SetActive(false);
        
    }
}
