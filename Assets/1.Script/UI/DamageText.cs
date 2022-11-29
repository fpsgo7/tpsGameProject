using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float moveSpeed = 60;
    private float alphaSpeed = 0.7f;
    public float damage;
    Text text;
    Color textColor;
    void Start()
    {
        text = GetComponent<Text>();
        text.text = damage.ToString();
        textColor = text.color;
        Invoke("DestroyObject", 3);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        textColor.a = Mathf.Lerp(textColor.a, 0, Time.deltaTime * alphaSpeed);
        text.color = textColor;

    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
