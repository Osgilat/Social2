using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButcherController : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float attackRadius = 1.5f;
    [SerializeField]
    private string enemy;

    private int kicksToDie = 4;
    private int rotationSpeed = 2;
    private bool lockedAnimation = false;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (kicksToDie <= 0)
        {
            return;
        }

        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget < attackRadius)
        {
            int n = Random.Range(1, 4);

            if (!lockedAnimation)
                StartCoroutine(PlayAndWaitForAnim("Attack(" + n + ")"));
        }

    }

    IEnumerator PlayAndWaitForAnim(string stateName)
    {
        int animLayer = 0;


        anim.Play(stateName);

        lockedAnimation = true;

        //Wait until Animator is done playing
        while (anim.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) &&
            anim.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
        {
            //Wait every frame until animation has finished
            yield return null;
        }

        lockedAnimation = false;
    }

    static bool playOnce = true;

    // void OnTriggerEnter(Collider other)
    void OnTriggerStay(Collider other)
    {
       
        if (other.gameObject.tag != enemy || !playOnce || kicksToDie < 0)
        {
            return;
        }

        Animator otherAnimator = other.GetComponentInParent<Animator>();
        int animLayer = 0;

        if (playOnce)
        {
           
            if (otherAnimator.GetCurrentAnimatorStateInfo(animLayer).IsName("Punch 1") ||
            otherAnimator.GetCurrentAnimatorStateInfo(animLayer).IsName("Punch 2"))
            {
                playOnce = false;

                kicksToDie--;

                other.gameObject.GetComponent<DecalPlacer>().canSpawnDecal = true;

                if (kicksToDie <= 0)
                {
                    GameObject.FindGameObjectWithTag("GameManager")
                        .GetComponent<GameManager>().ButcherDied();
                    anim.SetBool("isDead", true);
                }

                float timeToWait = otherAnimator.GetCurrentAnimatorStateInfo(animLayer).length;


                foreach (var handCollider in GameObject.FindGameObjectsWithTag("Hand"))
                {
                    handCollider.GetComponent<SphereCollider>().enabled = false;
                }

                //Against double hit
                StartCoroutine(LateCall(other, timeToWait));

            }
        }
        

        

    }

    IEnumerator LateCall(Collider other, float timeToWait)
    {

        yield return new WaitForSeconds(timeToWait);
        
        foreach (var handCollider in GameObject.FindGameObjectsWithTag("Hand"))
        {
            handCollider.GetComponent<SphereCollider>().enabled = true;
        }
        
        playOnce = true;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

}
