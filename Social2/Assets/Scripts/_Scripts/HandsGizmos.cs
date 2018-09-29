using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsGizmos : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetComponent<SphereCollider>().center, GetComponent<SphereCollider>().radius);
    }
}
