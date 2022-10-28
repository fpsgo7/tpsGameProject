using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //인벤토리 열기 닫기는 유아이 메니져에서 관리
    [SerializeField] private GameObject slotParent;// 아이템 슬롯의 레이아웃
    private Slot[] slots;//아이템 슬롯들


    void Start()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }

    public void AcquireItem(EquipmentItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemName == string.Empty)
            {
                Debug.Log("아이템 획득 성공");
                slots[i].AddItem(item);
                return;
            }
        }
    }
    //아이템 장착하면 나머지 장착색깔 비활성화
    public void ClearColor()
    { 
        for (int i = 0; i < slots.Length; i++)
        {
            Color color = slots[i].GetComponent<Image>().color;
            color.a = 0;
            slots[i].GetComponent<Image>().color = color;
        }
    }
}
