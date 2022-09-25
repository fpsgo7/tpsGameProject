using System;
using UnityEngine;
//LivingEntity 는 체력을 데미지를 받는 기능을 사용하여 IDamageable을 상속받는다.
//ApplyDamage 함수를 사용하기위하여 상속받는다.
public class LivingEntity : MonoBehaviour, IDamageable
{

    public float startingHealth = 100f;//초기체력
    public float health { get; protected set; }//현제 체력
    public bool dead { get; protected set; }//죽음 상태값
    //OnDeath 라는 이름으로 Action 타입의 이벤트가 있다.
    //외부에서 접근에서 할당하여 사용할 수 있다.
    public event Action OnDeath;
    //공격 중복 방지
    private const float minTimeBetDamaged = 0.1f;//공격과 공격사이의 대기시간
    private float lastDamagedTime;//마지막 데미지 시간
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
    //virtual 로 자식 스크립트에서 활용할 수잇다.
    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }
    //데미지 관련
    public virtual bool ApplyDamage(DamageMessage damageMessage)//매계변수로 DamageMessage 스크립트의 내용을 받는다.
    {
        //IsInvulnerabe 가 true 즉 무적상태이거나 damageMessage.damager
        //공격한자가 여가 대미지를 받는 대상과 갇거나 죽은경우 return 시켜 데미지기능을 막는다.
        if (IsInvulnerabe || damageMessage.damager == gameObject || dead)
            return false;
        //대미지를 입었으므로 시간을 넣어 업데이터한다.
        lastDamagedTime = Time.time;
        health -= damageMessage.amount;//체력을 감소 시킴
        //만약 체력이 0보다 작거나 같으면 죽는 함수 실행
        if (health <= 0)
            Die();
        //이후 공격이 성공했다는 신호를 보냄
        return true;
    }
    //폭파 데미지
    public virtual bool ApplyDamage(int damage)//매계변수로 DamageMessage 스크립트의 내용을 받는다.
    {
        //대미지를 입었으므로 시간을 넣어 업데이터한다.
        lastDamagedTime = Time.time;
        health -= damage;
        //만약 체력이 0보다 작거나 같으면 죽는 함수 실행
        if (health <= 0)
            Die();
        //이후 공격이 성공했다는 신호를 보냄
        return true;
    }
    //체력 회복
    public virtual void RestoreHealth()
    {
        //사망상태인 경우 체력 회복을 못한다.
        if (dead)
            return;
        //체력 회복
        health = startingHealth;
    }
    //죽는경우
    public virtual void Die()
    {
        //OnDeath 이벤트를 실행
        //외부에 스크립트의 event가 실행된다.
        if (OnDeath != null)
            OnDeath();
        //죽음은 true
        dead = true;
    }
}