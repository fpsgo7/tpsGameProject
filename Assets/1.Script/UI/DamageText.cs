using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float moveSpeed = 10;
    public float damage;
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        text.text = damage.ToString();
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
    }
}
