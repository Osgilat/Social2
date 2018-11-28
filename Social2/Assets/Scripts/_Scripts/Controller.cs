using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{


    [SerializeField]
    private string enemy;
    [SerializeField]
    private VignetteControl vignetteControl;

    private float inputH;
    private float inputV;
    private Animator anim;
    public CharacterController characterController;
    private Health health;
    public GameStates gameStates;

    SituationController situationController;

    // Use this for initialization
    void Start()
    {
        gameStates = GetComponent<GameStates>();
        health = GetComponent<Health>();
        situationController = GetComponent<SituationController>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != enemy)
        {
            return;
        }


        Animator otherAnimator = other.GetComponentInParent<Animator>();
        int animLayer = 0;

        if (!otherAnimator.GetCurrentAnimatorStateInfo(animLayer).IsName("Idle")
            && !otherAnimator.GetCurrentAnimatorStateInfo(animLayer).IsName("Dead"))
        {
            vignetteControl.ChangeVignetteAtRuntime();
        }


        float timeToWait = otherAnimator.GetCurrentAnimatorStateInfo(animLayer).length;

        other.enabled = false;
        //Against double hit
        StartCoroutine(LateCall(other, timeToWait));

    }

    IEnumerator LateCall(Collider other, float timeToWait)
    {

        yield return new WaitForSeconds(timeToWait);

        other.enabled = true;

    }

    public float speed = 4.0f;
    public float rotationSpeed = 80;
    public float rotation = 0f;
    float gravity = 8;

    Vector3 moveDir = Vector3.zero;

    // Update is called once per frame
    void Update()
    {

        if (!gameStates.enabledAI)
        {
            anim.SetFloat("Turn", inputH);
            anim.SetFloat("Forward", inputV);
        }
        

        if (Input.GetKeyDown(KeyCode.X) && !gameStates.enabledAI)
        {

            KnightHeal();
        }

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Slash1") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Slash2") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Drinking") && !gameStates.enabledAI)
        {

            MoveCharacter();

            if (Input.GetMouseButtonDown(0) &&
            GetComponent<PickingUpController>().sword.activeInHierarchy)
            {
                KnightAttack();

            }
        }


    }

    public void KnightHeal()
    {
        if (health.storedHealth == health.initialHealth)
        {
            return;
        }

        if (GetComponent<PickingUpController>().potion.activeInHierarchy)
        {
            anim.Play("Drinking");
        }
    }


        public void KnightAttack()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Slash1") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Slash2") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Drinking"))
        {
            int n = Random.Range(1, 3);

            if (Vector3.Distance(FindClosestEnemy().transform.position, transform.position) < distanceToHit)
            {
                situationController.currentSituation = SituationController.Situation.AttackingEnemy;
                FindClosestEnemy().GetComponent<Health>().DecreaseHealth();
                Logger.LogAction("Attacked", gameObject, FindClosestEnemy());

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

    private void MoveCharacter()
    {
        if (gameStates.enabledAI)
        {
            return;
        }

        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        if (inputV > 0)
        {
            moveDir = new Vector3(0, 0, 1);
            moveDir *= speed;
            moveDir = transform.TransformDirection(moveDir);
        }
        else
        {
            moveDir = Vector3.zero;
        }

        rotation += inputH * rotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotation, 0);

        characterController.Move(moveDir * Time.deltaTime);

    }
}
