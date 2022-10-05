using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen : MonoBehaviour
{
    public bool doorRock = true;
    private float moveSpeed = 1f;
    private Vector3 doorV3;

    void Start()
    {
        doorV3 = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && doorRock == false)
        {
            Debug.Log(" 문열기 시작");
        }
    }

    public void DoorOpen()
    {
        StartCoroutine(DoorOpening());
    }

    public IEnumerator DoorOpening()
    {
        Debug.Log("문이 열립니다.");
        doorV3.x += moveSpeed;
        yield return new WaitForSeconds(0.1f);
    }
}
