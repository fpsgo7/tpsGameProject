using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMiniTankGun : MonoBehaviour
{
    [SerializeField] Transform tfGunBody = null;// 총 몸체 위치
    [SerializeField] float spinSpeed = 0f;// 포탑 회전 속도
    [SerializeField] float fireRate = 0f;// 터렛의 연사속도
    float currentFireRate;// 실제 사용할 연사속도

    [SerializeField] Transform target = null;
    [SerializeField] EnemyTurretGun enemyTurretGun1 = null;
    [SerializeField] EnemyTurretGun enemyTurretGun2 = null;

    void Update()
    {
        if (target == null)
        {
            currentFireRate = fireRate;
            tfGunBody.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);// 적이 없는 동안은 계속 회전하게함
        }
        else
        {
            //쿼터니언 을 이용하여 플레이어를 바라보게 한다.(A벡터 -B 벡터 ) =B 좌표에서 A 좌표로 가는 벡터를 나타낸것이다.
            Quaternion rot = Quaternion.LookRotation(target.position - transform.position);
            //보간함수Slerp를 이용해서 점진적으로 회전시킨다 Slerp는 a각도에서 b각도 사이를 시간 t에 따라 점진적으로 반환하는 함수 이다.
            tfGunBody.rotation = Quaternion.Slerp(tfGunBody.rotation, rot, Time.deltaTime * spinSpeed);
            enemyTurretGun1.Fire();
            enemyTurretGun2.Fire();
        }
    }

    public void SameTarget(Transform target)
    {
        this.target = target;
    }
}
