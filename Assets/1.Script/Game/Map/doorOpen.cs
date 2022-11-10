using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen : MonoBehaviour
{
    public bool doorRock = true;
    private float moveSpeed = 0.01f;
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
            StartCoroutine(DoorOpening());
            GameManager.Instance.NextMap();
        }
    }

    public IEnumerator DoorOpening()
    {
        var wfs = new WaitForSeconds(0.01f);
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
