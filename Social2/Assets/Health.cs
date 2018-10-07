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

    private void Start()
    {
        isPlayer = (gameObject.name == "Paladin");

        initialHealth = healthPoints;
        storedHealth = healthPoints;
        
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

                GetComponent<Animator>().enabled = true;
                GetComponent<Animator>().SetTrigger("Die");
            }
        }

        
    }

    public bool canHarmPlayer = false;

    public void PlayerHit()
    {
        if (canHarmPlayer)
        {
            Logger.LogAction("Attacked", gameObject, GameObject.FindGameObjectWithTag("Player"));
            GameObject.FindGameObjectWithTag("Player").
                GetComponent<Health>().DecreaseHealth();
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
        if (!potion.activeInHierarchy)
        {
            return;
        }

        Logger.LogAction("PotionUsed", gameObject, null);
        ResetHealth();
        potion.SetActive(false);
        potion.transform.parent = initialParentTransform;
        potion.transform.localPosition = initialPotionPosition;
    }

}
