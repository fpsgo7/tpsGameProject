using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float moveSpeed = 10;
    public Text damageText;
    void Awake()
    {
        damageText = GetComponent<Text>();
    }
    
    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
    }

    public void SetDamageText(float damage)
    {
        Debug.Log(damage);
        damageText.text = damage.ToString();
    }
}
