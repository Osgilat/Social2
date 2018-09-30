using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wandering : MonoBehaviour {

	public float wanderRadius;
	public float wanderTimer;

	private Transform target;
	private NavMeshAgent agent;
	private float timer;


    public Animator anim;
    // Use this for initialization
    void OnEnable () {
		agent = GetComponent<NavMeshAgent> ();

        anim = GetComponent<Animator>();
        timer = wanderTimer;
	}

    public float agentVelocity;

    // Update is called once per frame
    void Update () {
        if (agent.velocity.magnitude >= 0)
        {
            agentVelocity = agent.velocity.magnitude;
            anim.SetFloat("Forward", agent.velocity.magnitude);
        }

        timer += Time.deltaTime;

		if (timer >= wanderTimer) {
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
