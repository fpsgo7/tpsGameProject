using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPooling : MonoBehaviour
{
    public static DamageTextPooling Instance;
    [SerializeField] private GameObject poolingObject;
    private Queue<GameObject> poolingQueue = new Queue<GameObject>();
    private static WaitForSeconds wfs = new WaitForSeconds(3f);
    void Awake()
    {
        Instance = this;
        Initalize(30);
    }
    // 오브젝트 생성
    private GameObject CreateNewObject()
    {
        var newGameObject = Instantiate(poolingObject, transform);
        newGameObject.SetActive(false);
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
    public static GameObject GetObjet(GameObject pos , float damage)
    {
        if (Instance.poolingQueue.Count > 0)
        {
            var gameObject = Instance.poolingQueue.Dequeue();//큐에서 하나 꺼내옴
            gameObject.transform.position = pos.transform.position;
            gameObject.GetComponent<DamageText>().damage = damage;
            gameObject.transform.SetParent(pos.transform);
            gameObject.SetActive(true);// 활성화하여 보여줌
            return gameObject;
        }
        else
        {
            Debug.Log("오브젝트 풀링 실패 새로 생성합니다.");
            var newGameObject = Instance.CreateNewObject();
            newGameObject.transform.position = pos.transform.position;
            newGameObject.GetComponent<DamageText>().damage = damage;
            newGameObject.transform.SetParent(pos.transform);
            newGameObject.SetActive(true);// 활성화하여 보여줌
            return newGameObject;
        }
    }
    public static IEnumerator ReturnObject(GameObject gameObject)
    {
        yield return wfs;
        gameObject.gameObject.SetActive(false);
        gameObject.transform.SetParent(Instance.transform);
        Instance.poolingQueue.Enqueue(gameObject);//다시 큐에 넣음
    }
}
