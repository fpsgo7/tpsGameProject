using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  사용안하는 스크립트
/// </summary>
public class doorOpen : MonoBehaviour
{
    public bool doorRock = true;
    private float moveSpeed = 0.01f;
    private Vector3 doorV3;
    WaitForSeconds wfs = new WaitForSeconds(0.01f);

    void Start()
    {
        doorV3 = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && doorRock == false)
        {
            Debug.Log(" 문열기 시작");
            StartCoroutine(DoorOpening());
           
        }
    }

    public IEnumerator DoorOpening()
    {
        var i = 0;
        while (i<750)
        {
            i++;
            doorV3.x += moveSpeed;
            transform.position = doorV3;
            yield return wfs;
        }

    }
}
