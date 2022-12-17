﻿using System;
using UnityEngine;
using UnityEngine.UI;
//LivingEntity 는 체력을 데미지를 받는 기능을 사용하여 IDamageable을 상속받는다.
//ApplyDamage 함수를 사용하기위하여 상속받는다.
public class LivingEntity : MonoBehaviour 
{

    public float maxHealth;//최대체력
    public float startingPlayerHealth;//플레이어 초기체력
    public float Health { get; protected set; }//현제 체력
    public bool IsDead { get; protected set; }//죽음 상태값
    //OnDeath 라는 이름으로 Action 타입의 이벤트가 있다.
    //외부에서 접근에서 할당하여 사용할 수 있다.
    public event Action OnDeath;
    //공격 중복 방지
    private const float minTimeBetDamaged = 0.1f;//공격과 공격사이의 대기시간
    private float lastDamagedTime;//마지막 데미지 시간
    public Slider enemyHealthSlider;
    private Image enemyHealthSliderImage;
    public LivingEntity livingEntity;
    //공격을 당하는 상태를 관리
    protected bool IsInvulnerabe
    {
        get
        {
            //걸린 시간이 마지막 데미지 시간  + 공격과 공격사이의 대기시간 보다 클경우 대미지를 입을수 있게 도운다.
            if (Time.time >= lastDamagedTime + minTimeBetDamaged) return false;

            return true;
        }
    }
    protected virtual void Awake()
    {
        enemyHealthSliderImage = enemyHealthSlider.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        livingEntity = GetComponent<LivingEntity>();
    }
    //virtual 로 자식 스크립트에서 활용할 수잇다.
    protected virtual void OnEnable()
    {
        IsDead = false;
        startingPlayerHealth = maxHealth;
        Health = maxHealth;
    }
    //데미지 관련
    public virtual bool IsApplyDamage(DamageMessage damageMessage)//매계변수로 DamageMessage 스크립트의 내용을 받는다.
    {
        //IsInvulnerabe 가 true 즉 무적상태이거나 damageMessage.damager
        //공격한자가 여가 대미지를 받는 대상과 갇거나 죽은경우 return 시켜 데미지기능을 막는다.
        if (IsInvulnerabe || damageMessage.damagerLivingEntity == livingEntity || IsDead)
            return false;
        //대미지를 입었으므로 시간을 넣어 업데이터한다.
        lastDamagedTime = Time.time;
        //layer은 숫자번째로 취급되어 숫자를찾아 조건을 넣는다.
        if (this.gameObject.layer == 9)
        {
            UIManager.Instance.ShowDamageText((int)damageMessage.amount,damageMessage.isHeadShot);
        }
        Health -= damageMessage.amount;//체력을 감소 시킴
        if(enemyHealthSlider != null)
        {
            enemyHealthSlider.value = Health;
        }
        //만약 체력이 0보다 작거나 같으면 죽는 함수 실행
        if (Health <= 0)
            Die(0);
        //이후 공격이 성공했다는 신호를 보냄
        return true;
    }
    //폭파 데미지
    public virtual bool IsApplyDamage(int damage , LivingEntity damager)//매계변수로 DamageMessage 스크립트의 내용을 받는다.
    {
        //대미지를 입었으므로 시간을 넣어 업데이터한다.
        //lastDamagedTime = Time.time;
        Health -= damage;
        //layer은 숫자번째로 취급되어 숫자를찾아 조건을 넣는다.
        if (this.gameObject.layer == 9)
        {
            UIManager.Instance.ShowDamageText(damage,false);
        }
        if (enemyHealthSlider != null)
        {
            enemyHealthSlider.value = Health;
        }
        //만약 체력이 0보다 작거나 같으면 죽는 함수 실행
        if (Health <= 0)
            Die(1);
        //이후 공격이 성공했다는 신호를 보냄
        return true;
    }
    //체력 회복
    public virtual void RestoreHealth()
    {
        //사망상태인 경우 체력 회복을 못한다.
        if (IsDead)
            return;
        //체력 회복
        Health = maxHealth;
    }
    //죽는경우
    public virtual void Die(int die)
    {
        //OnDeath 이벤트를 실행
        //외부에 스크립트의 event가 실행된다.
        if (OnDeath != null)
            OnDeath();
        //죽음은 true
        IsDead = true;
    }

    //적 AI의 초기 스펙을 결정하는 셋업 메서드
    public void EnemySetup(int intensity)
    {
        var newStartHealth=0f;
        //체력 설정 후 체력바 색깔 설정
        switch (intensity)
        {
            case 0:
                enemyHealthSliderImage.color = Color.red;
                newStartHealth = maxHealth;
                break;
            case 1:
                enemyHealthSliderImage.color = Color.blue;
                newStartHealth = maxHealth * 2;
                break;
            case 2:
                enemyHealthSliderImage.color = Color.yellow;
                newStartHealth = maxHealth * 3;
                break;
        }
        // 체력 설정
        this.maxHealth = newStartHealth;
        this.Health = maxHealth;
        //체력바에 값 적용
        if (enemyHealthSlider != null)
        {
            enemyHealthSlider.maxValue = Health;
            enemyHealthSlider.value = Health;
        }
    }
}