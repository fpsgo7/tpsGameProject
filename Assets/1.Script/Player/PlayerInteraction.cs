using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private float range;//아이템 박스 열기위한 사정거리

    private RaycastHit hitInfo; // 충돌체 정보 저장
    //아이템 레이어를 지정하여 아이템 레이어 에만 반응하도록 레이어 마스크
    [SerializeField]
    private LayerMask layerMask;//레이어 마스크로 상호작용 대상으로 선정

    void Update()
    {
        //주의 레이케스트 발사 위치는 플레이어의 위치를 기준으로 하기 때문에 발바닥에서 발사된다 봐야한다. (수정필요)
        //닿은 대상이 범위내에 상호작용 대상일경우 true
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),out hitInfo,range, layerMask))
        {
            Debug.Log(hitInfo.transform.gameObject.name);
            // 해당 상호작용 대상이 아이템 박스일 경우
            if(hitInfo.transform.tag == "ItemBox")
            {
                //상호작용 이 가능해짐
                UIManager.Instance.OnItemBoxText();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //안에 들어있는 아이템을 활성화 시키고 아이템 박스의 렌더러를 비활성화 하고 트리거를 true 로 하여 플레이어가 아이템을 먹을 수 있게함
                    hitInfo.transform.GetChild(0).gameObject.SetActive(true);
                    hitInfo.transform.GetComponent<MeshRenderer>().enabled = false;
                    hitInfo.transform.GetComponent<BoxCollider>().isTrigger = true;
                    UIManager.Instance.OffItemBoxText();
                }
            }
            if (hitInfo.transform.tag == "ExplosionWall")
            {
                //상호작용 이 가능해짐
                UIManager.Instance.OnExplosionWallText();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hitInfo.transform.GetChild(0).gameObject.SetActive(true);// 폭탄 오브젝트 활성화
                    UIManager.Instance.OffExplosionWallText();
                    Invoke(nameof(Explosion), 2f);
                }
            }
        }
        else
        {
            UIManager.Instance.OffItemBoxText();
            UIManager.Instance.OffExplosionWallText();
        }
        
    }

    private void Explosion()
    {
        hitInfo.transform.GetComponent<MeshRenderer>().enabled = false;
        hitInfo.transform.GetComponent<BoxCollider>().isTrigger = true;
        hitInfo.transform.GetChild(0).gameObject.SetActive(false);
        hitInfo.transform.GetChild(1).gameObject.SetActive(true);
        EffectManager.Instance.ExplosionEffect(hitInfo.transform);
        Destroy(hitInfo.transform.gameObject,5f);
    }
}
