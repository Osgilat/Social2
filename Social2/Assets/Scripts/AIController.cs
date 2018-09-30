using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    public class AIController : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for

        public Animator anim;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            anim = GetComponent<Animator>();

	        agent.updateRotation = true;
	        agent.updatePosition = true;
        }

        public float agentVelocity;

        private void Update()
        {
            if(agent.velocity.magnitude >= 0)
            {
                agentVelocity = agent.velocity.magnitude;
                anim.SetFloat("Forward", agent.velocity.magnitude);
            }

            if (target != null)
            {
                agent.SetDestination(target.position);
            }



            if (agent.remainingDistance < agent.stoppingDistance)
            {
                agent.ResetPath();
                anim.SetFloat("Forward", 0.0f);
            }
            
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
