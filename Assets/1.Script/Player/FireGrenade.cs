using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrenade : MonoBehaviour
{
    public Transform firePos;
    public int grenadeAmmo =10;
    public float timeBetFire = 3f; // 수류탄 발사 간격
    private float lastFireTime; // 수류탄 마지막으로 발사한 시점
    private Animator playerAnimator; // 플레이어의 애니메이터
    private PlayerInput playerInput;
    private readonly int hashGrenade = Animator.StringToHash("Grenade");//Animator의 Grenade 트리거를 가져온다. 애니메이터 최적화
    public LateUpdateFollow lateUpdateFollow;//총잡는 부분 수적

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        grenadeAmmo = 10;
    }
    private void FixedUpdate()
    {
        if (playerInput.Grenade)
        {
            Fire();
        }
    }

    //발사 명령은 PlayerInput 에서 입력을 받아 PlayerShooter에서 실행된다.   
    public void Fire()
    {
        //수류탄 발사 딜레이와 발사 조건
        if (grenadeAmmo >= 1 &&Time.time >=lastFireTime +timeBetFire)
        {
            lateUpdateFollow.enabled = false;
            FireAnimation();
            --grenadeAmmo;
            UIManager.Instance.UpdateGrenadeText(grenadeAmmo);
            lastFireTime = Time.time;
        }
    }
    private void FireAnimation()
    {
        //PlayerShooter.instance.ikActive = false;//iKActive를 수정하여 총을 놓게함
        playerAnimator.SetTrigger(hashGrenade);
    }
    public void FireGrenadeProcess()//애니메이션 동작 이벤트에서 작동
    {
        var grenade = GrenadeObjectPooling.GetObjet();
        grenade.FireMan = gameObject;
        grenade.transform.position = firePos.transform.position;
        grenade.transform.rotation = firePos.transform.rotation;
        grenade.Shoot();
    }
    public void FireGrenadeEnd()//애니메이션 동작 이벤트에서 작동
    {
        lateUpdateFollow.enabled = true;
    }
    
    //수류탄 충전
    public void GetGrenade()
    {
        grenadeAmmo += 1;
        UIManager.Instance.UpdateGrenadeText(grenadeAmmo);
    }
}
