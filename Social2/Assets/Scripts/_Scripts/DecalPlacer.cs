using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalPlacer : MonoBehaviour
{

    public bool canSpawnDecal = false;

    [SerializeField]
    private GameObject decalPrefab;


    private void Update()
    {
        if (canSpawnDecal)
        {
            RaycastHit hitInfo;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hitInfo, 10, 1 << LayerMask.NameToLayer("SkinMesh")))
            {
                if (hitInfo.transform.gameObject.name.Equals("Body"))
                {
                    canSpawnDecal = false;
                    SpawnDecal(hitInfo);
                }
            }
        }
    }

    private void SpawnDecal(RaycastHit hitInfo)
    {
        var decal = Instantiate(decalPrefab);

        decal.transform.forward = hitInfo.normal;
        decal.transform.position = hitInfo.point;

        decal.transform.parent = FindClosestBone(hitInfo.transform.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().bones, decal.transform.position);
    }

    Transform FindClosestBone(Transform[] bones, Vector3 decalPos)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (Transform potentialTarget in bones)
        {
            Vector3 directionToTarget = potentialTarget.position - decalPos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
