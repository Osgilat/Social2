using UnityEngine;

public class CameraLock : MonoBehaviour
{

    [SerializeField]
    private GameObject targetBone;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = targetBone.transform.position;
    }

}
