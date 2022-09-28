﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform m_tfGunBody = null;// 총 몸체 위치
    [SerializeField] float m_range = 0f;// 사정거리
    [SerializeField] LayerMask m_layerMask = 0;//레이어 마스크 
    [SerializeField] float m_spinSpeed = 0f;// 포탑 회전 속도
    [SerializeField] float m_fireRate = 0f;// 터렛의 연사속도
    float m_currentFireRate;// 실제 사용할 연사속도

    Transform m_tfTarget = null;
    
    void SearchEnemy()// 사정거리내에 맞출 대상을 검출한다.
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_range, m_layerMask);//(자신의 위치, 사정거리,대상)
        Transform t_shortestTarget = null;

        if (t_cols.Length > 0)
        {
            float t_shortestDistance = Mathf.Infinity;
            //SqrMagnitude => 제곱 반환(실제 거리 * 실제거리)
            //Distance => 루트 연산후 반호나 (실제거리)
            // 가장 가까운 적을 얻어내기위한 반복문이다.
            foreach(Collider t_colTarget in t_cols)
            {
                float t_distance = Vector3.SqrMagnitude(transform.position - t_colTarget.transform.position);
                if(t_shortestDistance > t_distance)
                {
                    t_shortestDistance = t_distance;
                    t_shortestTarget = t_colTarget.transform;
                }
            }
        }

        m_tfTarget = t_shortestTarget;//가장 가까운 타겟을 넣음
        
    }
    
    void Start()
    {
        InvokeRepeating("SearchEnemy", 0f, 0.5f);// 0.5초마다 적 탐지를 함
    }
    
    void Update()
    {
        if(m_tfTarget == null)
        {
            m_currentFireRate = m_fireRate;
            m_tfGunBody.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);// 적이 없는 동안은 계속 회전하게함
        }
        else
        {
            Quaternion t_lookRotation = Quaternion.LookRotation(m_tfTarget.position);
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
                if(m_currentFireRate <= 0)
                {
                    m_currentFireRate = m_fireRate;
                    Debug.Log("발싸");
                }
            }
        }
    }
}