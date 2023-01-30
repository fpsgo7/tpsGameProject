using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 특정 오브젝트가 플레이어의 메인카메라를 보도록 회전 시켜주는 스크립트
/// </summary>
public class Billboard : MonoBehaviour {

    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
