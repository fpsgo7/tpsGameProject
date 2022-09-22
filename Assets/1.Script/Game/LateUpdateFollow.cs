using UnityEngine;

public class LateUpdateFollow : MonoBehaviour
{
    public Transform targetToFollow;//따라갈 대상
    public Transform targetToFollowZoom;// 줌상태에 따라갈 대상
    public Transform target;
    public PlayerShooter playerShooter;
    private void Start()
    {
       target = targetToFollow;
      
    }
    private void LateUpdate()
    {
            transform.position = target.position;
            transform.rotation = target.rotation;
    }
    public void ZoomInFollow()
    {
        target = targetToFollowZoom;
    }
    public void ZoomOutFollow()
    {
        target = targetToFollow;
    }
}