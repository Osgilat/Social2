using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderToMesh : MonoBehaviour {

    private SkinnedMeshRenderer meshRenderer;
    private new MeshCollider collider;

    private void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        collider = GetComponent<MeshCollider>();

        InvokeRepeating("UpdateCollider", 0, 0.15f);
    }
   
    public void UpdateCollider()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        collider.sharedMesh = null;
        collider.sharedMesh = colliderMesh;
    }

}
