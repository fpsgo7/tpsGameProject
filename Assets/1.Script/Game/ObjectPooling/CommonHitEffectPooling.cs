using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonHitEffectPooling : MonoBehaviour
{
    public static CommonHitEffectPooling Instance;

    [SerializeField]
    private GameObject poolingObject;
    private Queue<GameObject> poolingQueue = new Queue<GameObject>();
    private static WaitForSeconds wfs = new WaitForSeconds(0.9f);
    void Awake()
    {
        Instance = this;
        Initalize(20);
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
    public static GameObject GetObjet(Vector3 pos,Vector3 normal)
    {
        if (Instance.poolingQueue.Count > 0)
        {
            var particle = Instance.poolingQueue.Dequeue();//큐에서 하나 꺼내옴
            particle.transform.position = pos;
            particle.transform.rotation = Quaternion.LookRotation(normal);
            particle.SetActive(true);// 활성화하여 보여줌
            return particle;
        }
        else
        {
            var newParticle = Instance.CreateNewParticle();
            newParticle.transform.position = pos;
            newParticle.transform.rotation = Quaternion.LookRotation(normal);
            newParticle.SetActive(true);// 활성화하여 보여줌
            return newParticle;
        }
    }
    public static IEnumerator ReturnObject(GameObject particle)
    {
        yield return wfs;
        particle.gameObject.SetActive(false);
        particle.transform.SetParent(Instance.transform);
        Instance.poolingQueue.Enqueue(particle);//다시 큐에 넣음
    }
}
