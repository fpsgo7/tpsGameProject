using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPooling : MonoBehaviour
{
    public static DamageTextPooling Instance;
    [SerializeField] private GameObject poolingObject;
    private Queue<GameObject> poolingQueue = new Queue<GameObject>();
    void Awake()
    {
        Instance = this;
        Initalize(30);
    }
    // 오브젝트 생성
    private GameObject CreateNewParticle()
    {
        var newParticle = Instantiate(poolingObject, transform);
        newParticle.SetActive(false);
        return newParticle;
    }
    // 실제 오브젝트 생성 실행을휘한 함수
    private void Initalize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            poolingQueue.Enqueue(CreateNewParticle());
        }
    }
    //오브젝트를 풀링하기
    public GameObject GetObjet(GameObject pos , float damage)
    {
        if (Instance.poolingQueue.Count > 0)
        {
            var gameObject = Instance.poolingQueue.Dequeue();//큐에서 하나 꺼내옴
            gameObject = Instance.poolingQueue.Dequeue();//큐에서 하나 꺼내옴
            gameObject.transform.position = pos.transform.position;
            gameObject.GetComponent<DamageText>().damage = damage;
            gameObject.transform.SetParent(pos.transform);
            gameObject.SetActive(true);// 활성화하여 보여줌
            return gameObject;
        }
        else
        {
            var newGameObject = Instance.CreateNewParticle();
            newGameObject.transform.position = pos.transform.position;
            newGameObject.GetComponent<DamageText>().damage = damage;
            newGameObject.transform.SetParent(pos.transform);
            newGameObject.SetActive(true);// 활성화하여 보여줌
            return newGameObject;
        }
    }
    public IEnumerator ReturnObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(3f);
        gameObject.gameObject.SetActive(false);
        gameObject.transform.SetParent(Instance.transform);
        Instance.poolingQueue.Enqueue(gameObject);//다시 큐에 넣음
    }
}
