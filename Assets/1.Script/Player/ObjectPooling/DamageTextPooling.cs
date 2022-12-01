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
        var newGameObject = Instantiate(poolingObject, transform).GetComponent<DamageText>();
        newGameObject.gameObject.SetActive(false);
        return newGameObject;
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
    public DamageText GetObject(GameObject pos , float damage)
    {
        if (Instance.poolingQueue.Count > 0)
        {
            DamageText gameObject = Instance.poolingQueue.Dequeue();//큐에서 하나 꺼내옴
            gameObject.transform.position = pos.transform.position;
            gameObject.SetDamageText(damage);
            gameObject.transform.SetParent(pos.transform);
            gameObject.gameObject.SetActive(true);// 활성화하여 보여줌
            StartCoroutine(ReturnObject(gameObject));//일정시간뒤 비활성화 하게함
            return gameObject;
        }
        else
        {
            Debug.Log("오브젝트 풀링 실패 새로 생성합니다.");
            DamageText newGameObject = Instance.CreateNewObject();
            newGameObject.transform.position = pos.transform.position;
            newGameObject.SetDamageText(damage);
            newGameObject.transform.SetParent(pos.transform);
            newGameObject.gameObject.SetActive(true);// 활성화하여 보여줌
            StartCoroutine(ReturnObject(newGameObject));
            return newGameObject;
        }
    }
    public IEnumerator ReturnObject(DamageText gameObject)
    {
        yield return WaitReturnText;
        gameObject.InActiveDamageText();
        gameObject.transform.SetParent(Instance.transform);
        Instance.poolingQueue.Enqueue(gameObject);//다시 큐에 넣음
    }
}
