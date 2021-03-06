﻿using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
    Animator _animator;
    public GameObject player;

    public bool isNearPlayer = false;

    void Awake()
    {
        //_player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    public void DissolveGhost()
    {
        
        if (!GetComponent<Health>().canHarmPlayer)
        {
            Logger.LogAction("GhostDissappered", gameObject, null);
            if (player.GetComponent<GameStates>().swordEquiped)
            {
                player.GetComponent<SituationController>()
                .currentSituation = SituationController.Situation.GhostDissolvedWhenArmed;
            }
            else
            {
                player.GetComponent<SituationController>()
                .currentSituation = SituationController.Situation.GhostDissolvedWhenUnarmed;
            }
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<Aspect>().aspectType == Aspect.AspectTypes.PLAYER)
        {
            player = other.gameObject;

            player.GetComponent<Wandering>().inBattle = true;
            isNearPlayer = true;
            _animator.SetBool("IsNearPlayer", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Aspect>().aspectType == Aspect.AspectTypes.PLAYER)
        {
            player.GetComponent<Wandering>().inBattle = false;
            isNearPlayer = false;
            _animator.SetBool("IsNearPlayer", false);
        }
    }

    private void Update()
    {
        if(player != null && player.GetComponent<Health>().healthPoints == 0)
        {
            isNearPlayer = false;
            _animator.SetBool("IsNearPlayer", false);
        }
    }
}