﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Perspective perspective;
    public Health health;
    public PickingUpController pickingUpController;
    public GameStates gameStates;
    public Wandering mover;
    public SituationController situationController;
    public Controller controller;

    public Action currentAction;

    [System.Serializable]
    public class Action
    {
        [SerializeField]
        public string actionID;
        [SerializeField]
        public float probability;
        [SerializeField]
        public bool isBattleAction;

        public Action(string actionID, float probability, bool isBattleAction)
        {
            this.actionID = actionID;
            this.probability = probability;
            this.isBattleAction = isBattleAction;
        }
        [SerializeField]
        public string ActionID
        {
            get
            {
                return actionID;
            }
        }
        [SerializeField]
        public float Probability
        {
            get
            {
                return probability;
            }

            set
            {
                probability = value;
            }
        }
        [SerializeField]
        public bool IsBattleAction
        {
            get
            {
                return isBattleAction;
            }
        }
    }

   
    public List<Action> actions = new List<Action>();

    // Use this for initialization
    void Start()
    {
        perspective = GetComponent<Perspective>();
        health = GetComponent<Health>();
        pickingUpController = GetComponent<PickingUpController>();
        gameStates = GetComponent<GameStates>();
        mover = GetComponent<Wandering>();
        controller = GetComponent<Controller>();

        lastHP = health.initialHealth;

        actions.Add(new Action("Heal", 0.0f, false));
        actions.Add(new Action("PickupReward", 0.0f, false));
        actions.Add(new Action("Wander", 1.0f, false));
        actions.Add(new Action("EnterMirror", 0.0f, false));
        actions.Add(new Action("AttackToEnd", 0.0f, true));
    }

    private float timer = 5.0f;
    public float initialTimer = 5.0f;

    public bool sawSkeleton = false;
    public bool sawReward = false;

    public GameObject FindClosestAliveEnemy()
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.GetComponent<Health>().healthPoints >= 0 &&
                perspective.objectsInViewport.Contains(enemy))
            {
                Vector3 diff = enemy.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = enemy;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    public GameObject skeletonToAttack;

    public bool inBattle = false;

    public int lastHP = 0;

    public string currentActionID;

    public GameObject rewardInViewport;
    public GameObject mirrorInViewport;

    // Update is called once per frame
    void Update()
    {
         timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = initialTimer;
            ChangeActionToExecute();
        }

        if (currentAction == null)
        {
            return;
        }

        currentActionID = currentAction.ActionID;

        if (lastHP != health.storedHealth)
        {
            lastHP = health.storedHealth;

            actions.Find(obj => obj.ActionID == "Heal").Probability
                = ((float)health.initialHealth / (health.storedHealth + 1) - 1.0f);

                actions.Find(obj => obj.ActionID == "EnterMirror").Probability

                = ((float)health.initialHealth / (health.storedHealth + 1) - 1.0f);
        }

        if (rewardInViewport == null)
        {
            actions.Find(obj => obj.ActionID == "PickupReward").Probability
                = 0.0f;
        }

        if (mirrorInViewport == null || gameStates.potionEquiped)
        {
            actions.Find(obj => obj.ActionID == "EnterMirror").Probability
                = 0.0f;
        }
      

        if (currentAction.ActionID == "EnterMirror" && !mover.inBattle)
        {
            if (!gameStates.potionEquiped)
            {
                if (mirrorInViewport != null)
                {
                    mover.targetToMove = mirrorInViewport;
                }
            }

        }

        if (currentAction.ActionID == "PickupReward" && !mover.inBattle)
        {
            if (rewardInViewport != null && skeletonToAttack == null)
            {
                mover.targetToMove = rewardInViewport;
            }

        }

        if (currentAction.ActionID == "Wander" && !mover.inBattle)
        {
            mover.SetRandomWaypoint();
        }

        if (currentAction.ActionID == "Heal" && gameStates.potionEquiped && !mover.inBattle)
        {
            controller.KnightHeal();
        }

        if (currentAction.ActionID == "AttackToEnd" || mover.inBattle)
        {
            if (skeletonToAttack == null || !skeletonToAttack.activeInHierarchy)
            {
                actions.Find(obj => obj.ActionID == "AttackToEnd").Probability
                = 0.0f;
                mover.inBattle = false;
                skeletonToAttack = FindClosestAliveEnemy();
            }

            if (skeletonToAttack != null && skeletonToAttack.activeInHierarchy && gameStates.swordEquiped)
            {
                mover.inBattle = true;

                if (skeletonToAttack == null ||
                    skeletonToAttack.GetComponent<Health>().healthPoints <= 0)
                {
                    mover.inBattle = false;
                    skeletonToAttack = null;
                    return;
                }

                if (skeletonToAttack != null)
                {
                    if ((skeletonToAttack.transform.position -
                    transform.position).sqrMagnitude >= controller.distanceToHit)
                    {
                        mover.targetToMove = skeletonToAttack;
                    }
                    else
                    {
                        mover.agent.isStopped = true;
                        mover.agent.ResetPath();
                        mover.targetToMove = null;
                        controller.KnightAttack();
                    }
                }
            }
        }
    }

    public bool initialActionsExecuted = false;

    public void ChangeActionToExecute()
    {
        if ((!gameStates.swordEquiped || !gameStates.potionEquiped) && !initialActionsExecuted)
        {
            GameObject sword = GameObject.Find("Sword");
            GameObject potion = GameObject.Find("Potion");

            if (!gameStates.swordEquiped && sword != null)
            {
                mover.targetToMove = sword;
                return;
            }

            if (!gameStates.potionEquiped && potion != null)
            {
                mover.targetToMove = potion;
                return;
            }

            if(gameStates.potionEquiped && gameStates.swordEquiped)
            {
                initialActionsExecuted = true;
            }
        }

        currentAction = SectorChoose();
    }

    public Action SectorChoose()
    {
        float total = actions.Sum(item => item.Probability);
        float diceRoll = Random.Range(0.0f, total);

        float cumulative = 0.0f;
        foreach (Action action in actions)
        {
            cumulative += action.Probability;
            if (diceRoll < cumulative && action.Probability != 0.0f)
            {
                return action;
            }
        }
        return actions[0];
    }
}

