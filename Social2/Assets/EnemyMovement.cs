using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent _nav;
    private Transform player;
    private Animator anim;

    public Vector3 initialPosition;

    public float forgetAboutPlayerDistance = 30;
    public float interactWithPlayerDistance = 15;


    void Start()
    {
        initialPosition = gameObject.transform.position;
        _nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    private float speed;
    private Vector3 lastPosition; 

    void Update()
    {
        speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude, 0.7f /*adjust this number in order to make interpolation quicker or slower*/);
        lastPosition = transform.position;

        anim.SetFloat("RemainingDistance", _nav.remainingDistance);

        /*if player at min distance and we are at max distance from initial position*/
        if (Vector3.Distance(transform.position, player.position) <= interactWithPlayerDistance &&
            Vector3.Distance(transform.position, initialPosition) < forgetAboutPlayerDistance)
        {
            /*if player next to us*/
            if (GetComponent<EnemyAttack>().isNearPlayer || player.GetComponent<Health>().healthPoints == 0)
            {
                _nav.ResetPath();
                return;
            }

            
            /*Stop following player if can't raycast*/
            // enemy is ahead of me and in my field of view
            RaycastHit hitInfo;

            // Vector3(0, 0.5f, 0) is the head offset for my capsules, you'll need to adjust accordingly
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0),
                (player.transform.position + new Vector3(0, 0.5f, 0)) - transform.position, out hitInfo) == true)
            {
                // we hit SOMETHING, not necessarily a player
                if (!(hitInfo.collider.gameObject.tag == "Player"))
                {
                    //Debug.Log("Cant see player " + hitInfo.collider.gameObject.name);
                    return;

                }
                    
            }
            

            _nav.SetDestination(player.position);
            transform.LookAt(player);
        }

        /*else if player more then on min distance and from us and we are more than on maxDistance from initial position*/
        else if(Vector3.Distance(transform.position, player.position) > interactWithPlayerDistance &&
            Vector3.Distance(transform.position, initialPosition) >= forgetAboutPlayerDistance)
        {
            /*Return to point*/
            _nav.SetDestination(initialPosition);
            transform.LookAt(initialPosition);
        }




    }
}