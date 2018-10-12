using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int healthPoints;

    public int DecreaseHealth()
    {
        return --healthPoints;
    }

    public void IncrementHealth()
    {
        healthPoints++;
    }

   

    public int storedHealth;
    public int initialHealth;

    public bool isPlayer = false;

    SituationController situationController;
    GameStates gameStates;

    private void Start()
    {
        isPlayer = (gameObject.name == "Paladin");

        initialHealth = healthPoints;
        storedHealth = healthPoints;

        situationController = GetComponent<SituationController>();
        gameStates = GetComponent<GameStates>();
    }

    public void ResetHealth()
    {
        healthPoints = initialHealth;
    }

    private void Update()
    {
        if (isPlayer)
        {
            if (storedHealth != healthPoints)
            {
                storedHealth = healthPoints;
                if (healthPoints <= 0)
                {
                    StartCoroutine(GetComponent<GameStates>().ReloadGame(true));

                }
                GetComponent<ColorGradingControl>().ChangeSaturationAtRuntime(
                    (float)healthPoints / (float)initialHealth + 0.2f);

            }
        }
        else
        {
            if(healthPoints <= 0)
            {
                foreach (MonoBehaviour c in GetComponents<MonoBehaviour>())
                {
                    c.enabled = false;
                }

                GameObject.FindGameObjectWithTag("Player").GetComponent<SituationController>()
                .currentSituation = SituationController.Situation.KilledEnemy;

                GetComponent<Animator>().enabled = true;
                GetComponent<Animator>().SetTrigger("Die");
            }
        }

        
    }

    public bool canHarmPlayer = false;
    public GameObject player;

    public void PlayerHit()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (canHarmPlayer)
        {
            Logger.LogAction("Attacked", gameObject, player);
            player.GetComponent<Health>().DecreaseHealth();
            if(player.GetComponent<Health>().storedHealth > 1)
            {
                player.GetComponent<SituationController>()
                .currentSituation = SituationController.Situation.HurtByEnemy;
            } else
            {
                player.GetComponent<SituationController>()
                .currentSituation = SituationController.Situation.KnightKilled;
            }
            
        } else
        {
            if (player.GetComponent<GameStates>().swordEquiped)
            {
                player.GetComponent<SituationController>()
                .currentSituation = SituationController.Situation.GhostDissolvedWhenArmed;
            }
            else
            {
                player.GetComponent<SituationController>()
                .currentSituation = SituationController.Situation.GhostDissolvedWhenArmed;
            }

        }
    }

  

    public GameObject potion;
    public GameObject handForPotion;
    public Transform initialParentTransform;
    public Vector3 initialPotionPosition;

    public void PotionEquip()
    {
        if (!potion.activeInHierarchy)
        {
            return;
        }

        initialParentTransform = potion.transform.parent;
        initialPotionPosition = potion.transform.localPosition;
        potion.transform.parent = handForPotion.transform;
        potion.transform.localPosition = Vector3.zero;

    }

    public void PotionDrink()
    {
        if (!potion.activeInHierarchy || storedHealth == initialHealth)
        {
            return;
        }

        Logger.LogAction("PotionUsed", gameObject, null);

        ResetHealth();

        situationController.currentSituation = SituationController.Situation.DrinkedPotion;
        gameStates.potionEquiped = false;
        potion.SetActive(false);
        potion.transform.parent = initialParentTransform;
        potion.transform.localPosition = initialPotionPosition;
    }

}
