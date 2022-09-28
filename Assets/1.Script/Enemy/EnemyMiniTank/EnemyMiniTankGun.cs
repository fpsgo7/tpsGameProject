using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMiniTankGun : MonoBehaviour
{
    [SerializeField] Transform m_tfGunBody = null;// 총 몸체 위치
    [SerializeField] float m_spinSpeed = 0f;// 포탑 회전 속도
    [SerializeField] float m_fireRate = 0f;// 터렛의 연사속도
    float m_currentFireRate;// 실제 사용할 연사속도

    [SerializeField] Transform target = null;

    void Update()
    {
        if (target == null)
        {
            m_currentFireRate = m_fireRate;
            m_tfGunBody.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);// 적이 없는 동안은 계속 회전하게함
        }
        else
        {
            Quaternion t_lookRotation = Quaternion.LookRotation(target.position);
            //RotateTowards  a 지점에서 b 지점까지의 c의 스피드로 회전
            Vector3 t_euler = Quaternion.RotateTowards(m_tfGunBody.rotation,
                t_lookRotation,
                m_spinSpeed * Time.deltaTime).eulerAngles;

            m_tfGunBody.rotation = Quaternion.Euler(0, t_euler.y, 0);//오일러 값이 y 축만 반영되게 수정한뒤 쿼터니언으로 변환

            //터렛이 조준해야할 최종 방향 필요
            Quaternion t_fireRotation = Quaternion.Euler(0, t_lookRotation.eulerAngles.y, 0);
            if (Quaternion.Angle(m_tfGunBody.rotation, t_fireRotation) < 5f)
            {
                m_currentFireRate -= Time.deltaTime;
                if (m_currentFireRate <= 0)
                {
                    m_currentFireRate = m_fireRate;
                    Debug.Log(target.transform.position.x+"");
                    Debug.Log("발싸");
                }
            }
        }
    }

    public void SameTarget(Transform target)
    {
        this.target = target;
    }
}
