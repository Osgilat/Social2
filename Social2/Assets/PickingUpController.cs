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

    

    SituationController situationController;
    SoundController soundController;
    GameStates gameStates;

    private void Start()
    {
        situationController = GetComponent<SituationController>();
        soundController = GetComponent<SoundController>();
        gameStates = GetComponent<GameStates>();
    }

    public void UnequipAll()
    {
        sword.SetActive(false);
        shield.SetActive(false);
        potion.SetActive(false);
        gameStates.swordEquiped = false;
        gameStates.potionEquiped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Sword":
                Logger.LogAction("SwordEquip", gameObject, null);
                sword.SetActive(true);
                shield.SetActive(true);

                gameStates.swordEquiped = true;
                soundController.OnSwordPickup();
                situationController.currentSituation = SituationController.Situation.SwordPickup; 
                other.gameObject.SetActive(false);
                break;
            case "Potion":
                Logger.LogAction("PotionEquip", gameObject, null);
                potion.SetActive(true);
                gameStates.potionEquiped = true;
                soundController.OnPotionPickup();
                situationController.currentSituation = SituationController.Situation.PotionPickup;
                Destroy(other.gameObject);
                break;
            case "RealTreasure":
                Logger.LogAction("FoundTreasure", gameObject, null);
                other.gameObject.GetComponent<Chest>().openedChest.SetActive(true);
                other.gameObject.GetComponent<Chest>().closedChest.SetActive(false);
                situationController.currentSituation = SituationController.Situation.AchievedTrueTreasure;
                StartCoroutine(WinGame());

                break;
            case "FakeTreasure":
                Logger.LogAction("FoundFakeTreasure", gameObject, null);
                other.gameObject.GetComponent<Chest>().openedChest.SetActive(true);
                other.gameObject.GetComponent<Chest>().closedChest.SetActive(false);
                situationController.currentSituation = SituationController.Situation.AchievedFakeTreasure;
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
        situationController.currentSituation = SituationController.Situation.WinnedRound;
        soundController.OnWon();
        yield return new WaitForSeconds(5);
        gameObject.transform.position = GameObject.Find("SpawnPoint").transform.position; ;
        gameObject.GetComponent<Health>().healthPoints = 0;
        yield return new WaitForSeconds(15);
        HideText();
        //victoryClip.SetActive(false);
    }
}
