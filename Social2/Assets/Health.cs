using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int healthPoints;

    public void DecreaseHealth()
    {
        healthPoints--;
    }

    public void IncrementHealth()
    {
        healthPoints++;
    }

    public bool reloadLevel = false;

    public int storedHealth;
    public int initialHealth;

    public bool isPlayer = false;

    public Animator anim;

    public Vector3 initialPlayerPosition;
    public GameObject initialPlayer;

    private void Start()
    {
        initialHealth = healthPoints;
        storedHealth = healthPoints;
        isPlayer = (gameObject.name == "Paladin");
        anim = GetComponent<Animator>();


        if (isPlayer)
        {
            initialPlayerPosition = gameObject.transform.position;
            initialPlayer = gameObject;
        }
        
    }

    public void ResetHealth()
    {
        healthPoints = initialHealth;
    }

    private void Update()
    {

        if (isPlayer)
        {
            if(storedHealth != healthPoints)
            {
                storedHealth = healthPoints;
                if(healthPoints <= 0)
                {
                    StartCoroutine(ReloadGame(true));
                }
                GetComponent<ColorGradingControl>().ChangeSaturationAtRuntime(
                    (float)healthPoints / (float)initialHealth + 0.2f); 

            }
        }

        if (healthPoints <= 0)
        {
            
            foreach (MonoBehaviour c in GetComponents<MonoBehaviour>())
            {
                c.enabled = false;
            }

            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().SetTrigger("Die");
        }
    }

    public bool canHarmPlayer = false;
    public void PlayerHit()
    {
        if (canHarmPlayer)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().DecreaseHealth();
        }
    }

    public GameObject addedSkeleton;
    public GameObject swordToRemove;
    public bool swordEquiped = false;

    public void TriggerMirror()
    {
        if (isPlayer)
        {
            Debug.Log("Triggered mirror");
            StartCoroutine(ReloadGame(false));
        }
    }

    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
           // if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    IEnumerator ReloadGame(bool isDied)
    {
        
        yield return new WaitForSeconds(5);
        Application.LoadLevel(Application.loadedLevel);

        yield return new WaitForSeconds(3);

        if (!isDied)
        {
            addedSkeleton = FindInActiveObjectByName("Skeleton (3)");
            swordToRemove = FindInActiveObjectByName("Sword");

            addedSkeleton.SetActive(true);
            if (swordEquiped)
            {
                swordToRemove.SetActive(false);
            }

            
        } else
        {
            healthPoints = initialHealth;
            GetComponent<PickingUpController>().UnequipAll();

            foreach (MonoBehaviour c in GetComponents<MonoBehaviour>())
            {
                c.enabled = true;
            }
        }

        

        anim.Play("Respawn");
        gameObject.transform.position = initialPlayerPosition;
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

        ResetHealth();
        potion.SetActive(false);
        potion.transform.parent = initialParentTransform;
        potion.transform.localPosition = initialPotionPosition;
    }

}
