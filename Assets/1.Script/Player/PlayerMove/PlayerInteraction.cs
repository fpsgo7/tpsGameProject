﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어의 상호작용을 관리한다.
/// 벽 폭탄 설치
/// 아이템 상자 오픈
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    private float range=3;// 상호작용의 사정거리
    private RaycastHit hitInfo; // 충돌체 정보 저장
    private WaitForSeconds wfs = new WaitForSeconds(2f);
    private PlayerInput playerInput;
    //아이템 레이어를 지정하여 아이템 레이어 에만 반응하도록 레이어 마스크
    [SerializeField] private LayerMask layerMask;//레이어 마스크로 상호작용 대상으로 선정
    [SerializeField] private GameObject GunPivot;// 플레이어의 시야 높이를 대변함

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        //주의 레이케스트 발사 위치는 플레이어의 위치를 기준으로 하기 때문에 발바닥에서 발사된다 봐야한다. (수정필요)
        //닿은 대상이 범위내에 상호작용 대상일경우 true
        if(Physics.Raycast(GunPivot.transform.position, GunPivot.transform.TransformDirection(Vector3.forward),out hitInfo,range, layerMask))
        {
            Debug.Log(hitInfo.transform.gameObject.name);
            // 해당 상호작용 대상이 아이템 박스일 경우
            if(hitInfo.transform.CompareTag("ItemBox"))
            {
                //상호작용 이 가능해짐
                UIManager.Instance.ActiveItemBoxText();
                if (playerInput.IsInteraction)
                {
                    //안에 들어있는 아이템을 활성화 시키고 아이템 박스의 렌더러를 비활성화 하고 트리거를 true 로 하여 플레이어가 아이템을 먹을 수 있게함
                    hitInfo.transform.GetChild(0).gameObject.SetActive(true);
                    hitInfo.transform.GetComponent<MeshRenderer>().enabled = false;
                    hitInfo.transform.GetComponent<BoxCollider>().enabled = false;
                    UIManager.Instance.InactiveItemBoxText();
                }
            }
            if (hitInfo.transform.CompareTag("ExplosionWall"))
            {
                //상호작용 이 가능해짐
                UIManager.Instance.ActiveExplosionWallText();
                if (playerInput.IsInteraction)
                {
                    hitInfo.transform.tag = "Untagged";//테그 비활성화로 상호작용 ui 가 활성화되는 것을 막음
                    hitInfo.transform.GetChild(0).gameObject.SetActive(true);// 폭탄 오브젝트 활성화
                    UIManager.Instance.InactiveExplosionWallText();
                    StartCoroutine(Explosion(hitInfo.transform));
                }
            }
        }
        else
        {
            UIManager.Instance.InactiveItemBoxText();
            UIManager.Instance.InactiveExplosionWallText();
        }
        
    }

    private IEnumerator Explosion(Transform transform)
    {
        yield return wfs;
        transform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<BoxCollider>().isTrigger = true;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        ObjectToolManager.Instance.GetObject((int)ObjectList.explosion, this.transform.position, this.transform.position);
        SoundToolManager.Instance.PlayOneShotSound((int)SoundList.explosion, this.transform.position, 1f);
        Destroy(transform.gameObject,5f);
    }
}