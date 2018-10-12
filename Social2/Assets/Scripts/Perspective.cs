using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : Sense
{
    public GameObject closestObject;
    public LayerMask objectsLayerMask;
    public Collider[] hits;
    public List<GameObject> objectsInViewport = new List<GameObject>();

    [Range(0, 180)]
    public int fieldOfView = 45;
    public int viewDistance = 100;

    public Transform playerTransform;
    public Vector3 rayDirection;
    public AIController aIController;

    private void Start()
    {
        aIController = GetComponent<AIController>();
    }

    protected override void Initialize()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= detectionRate)
        {
            DetectAspect();
        }
    }


    //Detect perspective field of view for the AI Character
    void DetectAspect()
    {
        /*Knight can forget what he saw*/
        //objectsInViewport.Clear();

        Ray ray = new Ray(transform.position, transform.forward);
        hits = Physics.OverlapSphere(transform.position, viewDistance, objectsLayerMask);

        Collider bestHit = null;
        float bestHitDistance = float.MaxValue;

        RaycastHit hitInfo;

        for (int i = 0; i < hits.Length; i++)
        {
            Vector3 directionToObject = hits[i].transform.position - transform.position;

            if (Vector3.Angle(transform.forward, directionToObject) < fieldOfView)
            {
                if (Physics.Raycast(transform.position,
                (hits[i].transform.position) - transform.position, out hitInfo) == true)
                {
                    // we hit SOMETHING
                    if (hitInfo.collider.gameObject.GetComponent<Aspect>() == null)
                    {
                        //Debug.Log("Cant see player " + hitInfo.collider.gameObject.name);
                        continue;

                    }
                    else
                    {
                        if (!objectsInViewport.Contains(hitInfo.collider.gameObject))
                        {
                            objectsInViewport.Add(hitInfo.collider.gameObject);
                            if (hitInfo.collider.gameObject.GetComponent<Aspect>().aspectType == Aspect.AspectTypes.ENEMY)
                            {
                                GetComponent<SoundController>().OnSkeletonSee();
                                if (GetComponent<GameStates>().swordEquiped)
                                {
                                    GetComponent<SituationController>().currentSituation
                                    = SituationController.Situation.ArmedSeeSkeleton;
                                }
                                else
                                {
                                    GetComponent<SituationController>().currentSituation
                                    = SituationController.Situation.UnarmedSeeSkeleton;
                                }

                            }

                            if (hitInfo.collider.gameObject.GetComponent<Aspect>().aspectType == Aspect.AspectTypes.REWARD)
                            {
                                GetComponent<SituationController>().currentSituation
                                    = SituationController.Situation.SeeTreasure;
                            }

                            if (hitInfo.collider.gameObject.GetComponent<Aspect>().aspectType == Aspect.AspectTypes.MIRROR)
                            {
                                aIController.actions.Find(v => v.ActionID == "EnterMirror").Probability
                                    = 1.0f;
                                aIController.mirrorInViewport = hitInfo.collider.gameObject;
                            }

                            if (hitInfo.collider.gameObject.GetComponent<Aspect>().aspectType == Aspect.AspectTypes.REWARD)
                            {
                                aIController.actions.Find(v => v.ActionID == "PickupReward").Probability
                                    = 1.0f;
                                aIController.rewardInViewport = hitInfo.collider.gameObject;

                            }

                        }

                    }

                    if (bestHit == null || directionToObject.sqrMagnitude < bestHitDistance)
                    {


                        bestHit = hits[i];
                        bestHitDistance = directionToObject.sqrMagnitude;


                    }
                }
            }

            /*
            foreach (GameObject item in objectsInViewport)
            {
                if (Physics.Raycast(transform.position,
                    (item.transform.position) - transform.position, out hitInfo) == false)
                {

                    {
                        objectsInViewport.Remove(item);
                    }
                }
            }
            */


            if (bestHit != null)
            {
                closestObject = bestHit.gameObject;
            }




            //Aspect aspect = hit.collider.GetComponent<Aspect>();
        }
    }
}
