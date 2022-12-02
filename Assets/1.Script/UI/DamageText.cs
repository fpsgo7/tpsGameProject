using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float moveSpeed = 10;
    public Text damageText;
    public bool isDamageTextActive=false;
    private void Awake()
    {
        damageText = GetComponent<Text>();
        damageText.color = Color.yellow;
    }

    private void OnEnable()
    {
        isDamageTextActive = true;
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
    }

    public void SetDamageText(float damage,bool isHeadShot)
    {
        Debug.Log("헤드샷"+isHeadShot);
        if (isHeadShot == true)
        {
            damageText.color = Color.red;
        }
        damageText.text = damage.ToString();
    }

    public void InActiveDamageText()
    {
        damageText.color = Color.yellow;
        isDamageTextActive = false;
        this.gameObject.SetActive(false);
    }
}
