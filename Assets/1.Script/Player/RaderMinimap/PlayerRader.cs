using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRader : MonoBehaviour
{
    public GameObject raderPrefab;

    public float switchDistance;//테스트를 위해 public 사용
    public Transform helpTransform;
    [SerializeField] private List<GameObject> raderObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> borderObjects = new List<GameObject>();
    private RaycastHit[] rayHits;

    void Update()
    {

        for (int i = 0; i<raderObjects.Count; i++)
        {
            if (Vector3.Distance(raderObjects[i].transform.position, transform.position) > switchDistance)
            {
                helpTransform.LookAt(raderObjects[i].transform);
                borderObjects[i].transform.position = transform.position + switchDistance * helpTransform.forward;
                borderObjects[i].layer = LayerMask.NameToLayer("RaderVisibleEnemy");
                raderObjects[i].layer = LayerMask.NameToLayer("RaderInvisibleEnemy");
            }
            else
            {
                borderObjects[i].layer = LayerMask.NameToLayer("RaderInvisibleEnemy");
                raderObjects[i].layer = LayerMask.NameToLayer("RaderVisibleEnemy");
            }
        }
    }
    public void GetTrackedObjects(GameObject trackedObject)
    {
        GameObject k = Instantiate(raderPrefab, trackedObject.transform.position, Quaternion.identity) as GameObject;
        k.transform.SetParent(trackedObject.transform);
        raderObjects.Add(k);
        GameObject j = Instantiate(raderPrefab, trackedObject.transform.position, Quaternion.identity) as GameObject;
        j.transform.SetParent(trackedObject.transform);
        borderObjects.Add(j);
    }

    public void RemoveTrackedObject(GameObject trackedObject)
    {
        for (int i = 0; i < raderObjects.Count; i++)
        {
            if (raderObjects[i].transform.parent.gameObject.Equals(trackedObject))
            {
                raderObjects.RemoveAt(i);
                borderObjects.RemoveAt(i);
            }
        }
        //raderObjects.Remove(gameObject);
        //borderObjects.Remove(gameObject);
    }
    //void createRaderObjects()
    //{
    //    for (int i = 0; i < rayHits.Length; i++)
    //    {
    //        if (rayHits[i].transform.GetComponent<LivingEntity>())
    //        {
    //            GameObject k = Instantiate(raderPrefab, rayHits[i].transform.position, Quaternion.identity) as GameObject;
    //            k.transform.SetParent(rayHits[i].transform);
    //            raderObjects.Add(k);
    //            GameObject j = Instantiate(raderPrefab, rayHits[i].transform.position, Quaternion.identity) as GameObject;
    //            j.transform.SetParent(rayHits[i].transform);
    //            borderObjects.Add(j);
    //        }
    //    }
    //}
}
