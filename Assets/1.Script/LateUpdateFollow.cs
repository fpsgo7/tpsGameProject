using UnityEngine;

public class LateUpdateFollow : MonoBehaviour
{
    public Transform targetToFollow;//따라갈 대상

    private void LateUpdate()
    {
        transform.position = targetToFollow.position;
        transform.rotation = targetToFollow.rotation;
    }
}