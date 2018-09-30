using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : MonoBehaviour {
    public Vector3 targetPosition;

    public float movementSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    public float targetPositionTolerance = 3.0f;
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    public Animator anim;
    private NavMeshAgent agent;

    void Start()
    {
        minX = -45.0f;
        maxX = 45.0f;

        minZ = -45.0f;
        maxZ = 45.0f;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        //Get Wander Position
        GetNextPosition();
    }

    void GetNextPosition()
    {
        targetPosition = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));
    }



    public float agentVelocity;

    void Update()
    {
        if (agent.velocity.magnitude >= 0)
        {
            agentVelocity = agent.velocity.magnitude;
            anim.SetFloat("Forward", agent.velocity.magnitude);
        }

        if (Vector3.Distance(targetPosition, transform.position) <=
            targetPositionTolerance)
        {
            GetNextPosition();
        }
        /*
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
            */
        agent.SetDestination(targetPosition);
        
    }
}
