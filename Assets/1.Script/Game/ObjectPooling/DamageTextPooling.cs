using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPooling : MonoBehaviour
{
    public static DamageTextPooling Instance;
    [SerializeField] private GameObject poolingObject;
    private Queue<DamageText> poolingQueue = new Queue<DamageText>();
    private static WaitForSeconds WaitReturnText = new WaitForSeconds(3f);
    void Awake()
    {
        Instance = this;
        Initalize(30);
    }
    // 오브젝트 생성
    private DamageText CreateNewObject()
    {
        DamageText newDamageText = Instantiate(poolingObject, transform).GetComponent<DamageText>();
        newDamageText.gameObject.SetActive(false);
        return newDamageText;
    }
    // 실제 오브젝트 생성 실행을휘한 함수
    private void Initalize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            poolingQueue.Enqueue(CreateNewObject());
        }
    }
    //오브젝트를 풀링하기
    public DamageText GetObject(GameObject pos , float damage,bool isHeadShot)
    {
        if (Instance.poolingQueue.Count > 0)
        {
            DamageText damageText = Instance.poolingQueue.Dequeue();//큐에서 하나 꺼내옴
            damageText.transform.position = pos.transform.position;
            damageText.SetDamageText(damage,isHeadShot);
            damageText.transform.SetParent(pos.transform);
            damageText.gameObject.SetActive(true);// 활성화하여 보여줌
            StartCoroutine(ReturnObject(damageText));//일정시간뒤 비활성화 하게함
            return damageText;
        }
        else
        {
            Debug.Log("오브젝트 풀링 실패 새로 생성합니다.");
            DamageText newDamageText = Instance.CreateNewObject();
            newDamageText.transform.position = pos.transform.position;
            newDamageText.SetDamageText(damage,isHeadShot);
            newDamageText.transform.SetParent(pos.transform);
            newDamageText.gameObject.SetActive(true);// 활성화하여 보여줌
            StartCoroutine(ReturnObject(newDamageText));
            return newDamageText;
        }
    }
    public IEnumerator ReturnObject(DamageText damageText)
    {
        yield return WaitReturnText;
        damageText.InActiveDamageText();
        damageText.transform.SetParent(Instance.transform);
        Instance.poolingQueue.Enqueue(damageText);//다시 큐에 넣음
    }
}
