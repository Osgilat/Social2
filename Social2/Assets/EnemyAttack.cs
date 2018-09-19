using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
    Animator _animator;
    GameObject _player;

    public bool isNearPlayer = false;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
        {
            isNearPlayer = true;
            _animator.SetBool("IsNearPlayer", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
        {
            isNearPlayer = false;
            _animator.SetBool("IsNearPlayer", false);
        }
    }
}