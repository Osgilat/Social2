using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : Sense
{
    public GameObject closestObject;

    public LayerMask objectsLayerMask;

    [Range(0, 180)]
    public int fieldOfView = 45;
    public int viewDistance = 100;

    public Transform playerTransform;
    public Vector3 rayDirection;

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

    public Collider[] hits;

    public List<GameObject> objectsInViewport = new List<GameObject>();

    //Detect perspective field of view for the AI Character
    void DetectAspect()
    {
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
