using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePersonViewCamera : MonoBehaviour
{

    public float rotSpeed = 200;

    private float mx;
    private float my;

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        mx += h * rotSpeed * Time.deltaTime;
        my += v * rotSpeed * Time.deltaTime;

        if(my >= 90)
        {
            my = 90;
        }else if(my <= -90)
        {
            my = -90;
        }
        transform.eulerAngles = new Vector3(-my, mx, 0);

    }
}
