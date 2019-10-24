using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField]
    private string enemy;

    private bool playOnce = true;
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != enemy)
        {
            return;
        }

        Animator otherAnimator = other.GetComponentInParent<Animator>();
        int animLayer = 0;

        if (otherAnimator.GetCurrentAnimatorStateInfo(animLayer).IsName("Stomp") && playOnce)
        {
            playOnce = false;
            GameObject.FindGameObjectWithTag("GameManager")
                    .GetComponent<GameManager>().SnakeDied();
            anim.SetBool("isDead", true);

        }
    }

}
