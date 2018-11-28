using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : MonoBehaviour {

    public GameObject addedSkeleton;
    public GameObject swordToRemove;

    public bool swordEquiped = false;
    public bool potionEquiped = false;

    public bool reloadLevel = false;

    public bool isPlayer = false;

    public Animator anim;

    public Vector3 initialPlayerPosition;
    public GameObject initialPlayer;
    public Health healthComponent;

    SituationController situationController;

    private void Start()
    {
        
        isPlayer = (gameObject.name == "Paladin");
        anim = GetComponent<Animator>();
        healthComponent = GetComponent<Health>();
        situationController = GetComponent<SituationController>();

        if (isPlayer)
        {
            initialPlayerPosition = gameObject.transform.position;
            initialPlayer = gameObject;
        }
    }

    public void TriggerMirror()
    {
        if (isPlayer)
        {
            situationController.currentSituation = SituationController.Situation.TriggeredMirror;
            GetComponent<AIController>().mirrorInViewport = null;
            StartCoroutine(ReloadGame(false));
        }
    }

    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }



    public IEnumerator ReloadGame(bool isDied)
    {
        if (isDied)
        {
            Logger.LogAction("PlayerDied", gameObject, null);

            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().SetTrigger("Die");
        }

        yield return new WaitForSeconds(4);
        
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

        }
        else
        {
            healthComponent.ResetHealth();

            GetComponent<PickingUpController>().UnequipAll();
            ReinitializeComponents();

        }

        situationController.currentSituation = SituationController.Situation.RoundBegin;
        anim.Play("Respawn");
        //gameObject.transform.position = initialPlayerPosition;
        gameObject.transform.position = GameObject.Find("SpawnPoint").transform.position;
        GetComponent<AIController>().initialActionsExecuted = false;
        GetComponent<Perspective>().objectsInViewport.Clear();
    }

    public bool enabledAI = false;

    private void ReinitializeComponents()
    {
        foreach (MonoBehaviour c in GetComponents<MonoBehaviour>())
        {
            if (c.GetType().ToString()
                == "Wandering" && !enabledAI)

            {
                c.enabled = false;
            }
            else
            {
                c.enabled = true;
            }
        }
    }

}
