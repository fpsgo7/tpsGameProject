using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float moveSpeed = 60;
    public float damage;
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        text.text = damage.ToString();
        Invoke("DestroyObject", 3);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
