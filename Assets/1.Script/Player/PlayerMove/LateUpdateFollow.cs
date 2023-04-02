using UnityEngine;
/// <summary>
/// 무기가 플레이어의 팔을 따라가기 위한 스크립트 
/// </summary>
public class LateUpdateFollow : MonoBehaviour
{
    public Transform targetToFollow;//따라갈 대상
    public Transform target;
    private void Start()
    {
        //Debug.Log(this.gameObject.name);
        GameObject.FindWithTag("Player").GetComponent<PlayerShooter>().lateUpdateFollow = this.GetComponent<LateUpdateFollow>();
        GameObject.FindWithTag("Player").GetComponent<FireGrenade>().lateUpdateFollow = this.GetComponent<LateUpdateFollow>();
        if (this.gameObject.name == "RifleGun(Clone)")
        {
            targetToFollow = GameObject.Find("RightHandAttach_byRifle").transform;
        }
        if (this.gameObject.name == "DmrGun(Clone)")
        {
            targetToFollow = GameObject.Find("RightHandAttach_byDmr").transform;
        }
        if (this.gameObject.name == "ShotGun(Clone)")
        {
            targetToFollow = GameObject.Find("RightHandAttach_byShotGun").transform;
        }
        target = targetToFollow;
       
    }
    private void LateUpdate()// 업데이트 문 필수
    {
            transform.position = target.position;
            transform.rotation = target.rotation;
    }
}