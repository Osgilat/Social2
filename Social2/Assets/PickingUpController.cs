using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        GetComponent<GameStates>().swordEquiped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Sword":
                Logger.LogAction("SwordEquip", gameObject, null);
                sword.SetActive(true);
                shield.SetActive(true);
                GetComponent<GameStates>().swordEquiped = true;
                GetComponent<SoundController>().OnSwordPickup();
                other.gameObject.SetActive(false);
                break;
            case "Potion":
                Logger.LogAction("PotionEquip", gameObject, null);
                potion.SetActive(true);
                GetComponent<SoundController>().OnPotionPickup();
                Destroy(other.gameObject);
                break;
            case "RealTreasure":
                Logger.LogAction("FoundTreasure", gameObject, null);
                other.gameObject.GetComponent<Chest>().openedChest.SetActive(true);
                other.gameObject.GetComponent<Chest>().closedChest.SetActive(false);
                StartCoroutine(WinGame());

                break;
            case "FakeTreasure":
                Logger.LogAction("FoundFakeTreasure", gameObject, null);
                other.gameObject.GetComponent<Chest>().openedChest.SetActive(true);
                other.gameObject.GetComponent<Chest>().closedChest.SetActive(false);
                break;
            default:
                break;
        }


    }

    Color transparent = new Color(0,0,0,0);

    public void ShowText(string text)
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<Image>().color = Color.black;
        GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<Text>().text = text;
    }

    public void HideText()
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<Image>().color = transparent;
        GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<Text>().text = "";
    }

    IEnumerator WinGame()
    {
        yield return new WaitForSeconds(5);
        //victoryClip.SetActive(true);
        ShowText("YOU WON");
        GetComponent<SoundController>().OnWon();
        yield return new WaitForSeconds(5);
        gameObject.transform.position = gameObject.GetComponent<GameStates>().initialPlayerPosition;
        gameObject.GetComponent<Health>().healthPoints = 0;
        yield return new WaitForSeconds(15);
        HideText();
        //victoryClip.SetActive(false);
    }
}
