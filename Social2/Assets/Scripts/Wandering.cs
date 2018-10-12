using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wandering : MonoBehaviour {

	public float wanderRadius;
	public float wanderTimer;
	private Transform target;
	public NavMeshAgent agent;
	private float timer;
    public Animator anim;
    public float agentVelocity;
    public Health health;

    public bool inBattle = false;

    public GameObject targetToMove;

    // Use this for initialization
    void OnEnable () {
		agent = GetComponent<NavMeshAgent> ();
        anim = GetComponent<Animator>();
        timer = wanderTimer;
        health = GetComponent<Health>();
	}

    // Update is called once per frame
    void Update () {

        if(health.storedHealth <= 0)
        {
            return;
        }

        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }

        if (agent.velocity.magnitude >= 0)
        {
            agentVelocity = agent.velocity.magnitude;
            anim.SetFloat("Forward", agent.velocity.magnitude);
        }

        if(targetToMove != null)
        {
            agent.SetDestination(targetToMove.transform.position);

            // Check if we've reached the destination
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        // Done
                        targetToMove = null;
                    }
                }
            }

            return;
        }

        /*
        timer += Time.deltaTime;

        if (timer >= wanderTimer && !inBattle)
        {
            SetRandomWaypoint();
        }
        */
	}

    public void SetRandomWaypoint()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer && !inBattle)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

	public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
		Vector3 randDirection = Random.insideUnitSphere * dist;

		randDirection += origin;

		NavMeshHit navHit;

		NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);

		return navHit.position;
	}
}
