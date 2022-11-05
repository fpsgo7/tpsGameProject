using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //인벤토리 열기 닫기는 유아이 메니져에서 관리
    [SerializeField] private GameObject weaponSlotParent;// 무기 슬롯의 레이아웃
    [SerializeField] private GameObject equipmentSlotParent;// 장비 킷트의 레이아웃
    private Slot[] weaponSlots;//무기 슬롯들
    private Slot[] equipmentSlots;//장비 슬롯들

    void Start()
    {
        weaponSlots = weaponSlotParent.GetComponentsInChildren<Slot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<Slot>();
    }

    public void AcquireItem(EquipmentItem item)
    {
        if(item.itemType == EquipmentItem.ItemType.Weapon)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i].itemName == string.Empty)
                {
                    Debug.Log("아이템 획득 성공");
                    weaponSlots[i].AddItem(item);
                    return;
                }
            }
        }
        if (item.itemType == EquipmentItem.ItemType.Equipment)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].itemName == string.Empty)
                {
                    Debug.Log("아이템 획득 성공");
                    equipmentSlots[i].AddItem(item);
                    return;
                }
            }
        }
    }
    //아이템 장착하면 나머지 장착색깔 비활성화
    public void ClearWeaponSlotColor()
    { 
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            Color color = weaponSlots[i].GetComponent<Image>().color;
            color.a = 0;
            weaponSlots[i].GetComponent<Image>().color = color;
        }
    }
    //장비일경우
    public void ClearEquipmentSlotColor()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            Color color = equipmentSlots[i].GetComponent<Image>().color;
            color.a = 0;
            equipmentSlots[i].GetComponent<Image>().color = color;
        }
    }
}
