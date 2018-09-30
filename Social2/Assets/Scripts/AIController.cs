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

        public void DrinkPotion()
        {
            if (GetComponent<PickingUpController>().potion.activeInHierarchy)
            {
                anim.Play("Drinking");
            }
        }

        public void AttackOnce()
        {
            if (GetComponent<PickingUpController>().sword.activeInHierarchy)
            {
                int n = UnityEngine.Random.Range(1, 3);

                if (Vector3.Distance(FindClosestEnemy().transform.position, transform.position) < distanceToHit)
                {

                    FindClosestEnemy().GetComponent<Health>().DecreaseHealth();
                    Debug.Log("PlayerDamaged");
                }
                anim.Play("Slash" + n);

            }
        }

        public float distanceToHit = 3.0f;

        public GameObject FindClosestEnemy()
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in gos)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
            return closest;
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
