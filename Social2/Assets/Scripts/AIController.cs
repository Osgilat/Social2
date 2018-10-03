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
        public float distanceToHit = 3.0f;
        Vector3 moveDir = Vector3.zero;
        float rotation = 0.0f;
        public float rotationSpeed = 80;
        public CharacterController characterController;

        /* BICA AI */
        public float mood;
        public float intensity;
        public float opportunity;
        public float dominance;
        public float agentVelocity;


        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            characterController = GetComponent<CharacterController>();
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            anim = GetComponent<Animator>();
	        agent.updateRotation = true;
	        agent.updatePosition = true;
            agent.stoppingDistance = 0.0002f;


            /* BICA AI */
            mood = 4.83f;
            intensity = 4.83f;
            opportunity = 7.08f;
            dominance = 5.17f;
            agent.speed = CalculateSpeed();
        }

        private void Update()
        {
            System.Random rnd = new System.Random();

            if (agent.velocity.magnitude >= 0)
            {
                agentVelocity = agent.velocity.magnitude;
                anim.SetFloat("Forward", agent.velocity.magnitude);
            }

            if (target != null)
            {
                agent.SetDestination(target.position);
            }

            if (agent.remainingDistance != 0.0f && agent.remainingDistance < agent.stoppingDistance)
            {
                target = null;
                agent.ResetPath();
                anim.SetFloat("Forward", 0.0f);
            }
            //    agent.ResetPath();
            //    anim.SetFloat("Forward", 0.0f);
            //}*/

            //if (GetComponent<PickingUpController>().sword.activeInHierarchy)
            //{
            //    target = null;
                //agent.ResetPath();
                //anim.SetFloat("Forward", 0.0f);
            //}



            /* BICA AI */
            agent.speed = CalculateSpeed();
            if (agent.velocity.magnitude <= 0.000001f)
            {
                float H;
                H = rnd.Next() % 100;
                MoveCharacter(0.0f, H / 300.0f);

                foreach (GameObject go in GetComponent<Perspective>().objectsInViewport)
                {
                    if (go.tag != null && go.tag != "not")
                    {
                        if (go.tag != null && go.tag == "Enemy" && go.GetComponent<Health>().healthPoints > 0.0001f && go.active != false && target == null)
                        {
                            //System.Random rnd = new System.Random();
                            UpdateParameters(4.5f, 8.08f, 4.83f, 3.67f);
                            if (rnd.Next() % Math.Floor(mood + opportunity + dominance * 3.0) > 15)
                            {
                                SetTarget(go.transform);
                            }
                            else
                            {
                                //go.tag = "not";
                            }
                        }
                        else if ((go.tag == null || go.tag != "Enemy") && target == null)
                        {
                            SetTarget(go.transform);
                        }
                    }
                }
            }

            DrinkPotion();
            AttackOnce();

        }

        public void DrinkPotion()
        {
            if (GetComponent<PickingUpController>().potion.activeInHierarchy)
            {
                UpdateParameters(6.17f, 4.92f, 6.33f, 5.92f);
                anim.Play("Drinking");
            }
        }

        public void AttackOnce()
        {
            if (GetComponent<PickingUpController>().sword.activeInHierarchy)
            {
                GameObject go = FindClosestEnemy();
                if (Vector3.Distance(go.transform.position, transform.position) < distanceToHit &&
                    go.GetComponent<Health>().healthPoints > 0.0f &&
                    !anim.GetCurrentAnimatorStateInfo(0).IsName("Slash1"))
                {

                    if(FindClosestEnemy().GetComponent<Health>().DecreaseHealth() <= 0.0001f)
                    {
                        UpdateParameters(6.75f, 5.33f, 6.75f, 6.00f);
                        target = null;
                        agent.ResetPath();
                        anim.SetFloat("Forward", 0.0f);
                    }
                    Debug.Log("PlayerDamaged");
                    anim.Play("Slash1");
                }
            }
        }


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

        public float CalculateSpeed()
        {
            return (mood * 1.0f + intensity * 3.0f + opportunity * 1.0f + dominance * 0.5f) / 10.0f;
        }

        public void UpdateParameters(float mood, float intensity, float opportunity, float dominance)
        {
            this.mood        = (this.mood        + mood        * 4.0f) / 5.0f;
            this.intensity   = (this.intensity   + intensity   * 2.0f) / 3.0f;
            this.opportunity = (this.opportunity + opportunity * 2.0f) / 3.0f;
            this.dominance   = (this.dominance   + dominance   * 2.0f) / 3.0f;
        }

        private void MoveCharacter(float V, float H)
        {
            float speed = CalculateSpeed();
            if (V > 0)
            {
                moveDir = new Vector3(0, 0, 1);
                moveDir *= speed;
                moveDir = transform.TransformDirection(moveDir);
            }
            else
            {
                moveDir = Vector3.zero;
            }

            rotation += H * rotationSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, rotation, 0);

            characterController.Move(moveDir * Time.deltaTime);

        }
    }
}
