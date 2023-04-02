﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 플레이어의 인벤토리로 장비 무기 슬롯들을 관리하며
/// 인벤토리 메니저에 정보를 넣거나 수정하게 시킨다.
/// </summary>
public class Inventory : MonoBehaviour
{
    //인벤토리 열기 닫기는 유아이 메니져에서 관리
    [SerializeField] private GameObject weaponSlotParent;// 무기 슬롯의 레이아웃
    [SerializeField] private GameObject equipmentSlotParent;// 장비 킷트의 레이아웃
    [SerializeField] private Slot[] weaponSlots;//무기 슬롯들
    [SerializeField] private Slot[] equipmentSlots;//장비 슬롯들
    [SerializeField] public List<Item> MyItemList;// 받아온 정보를 넣을 리스트 
    private string getJdata = string.Empty;
    private InventoryManager inventoryManager;
    EquipmentItem item = new EquipmentItem();
    int lastNum=0;// 아이템을 새로 획득하면 넣을 숫자의 이전값

    void Awake()
    {
        inventoryManager = GameObject.Find("InfoManager").GetComponent<InventoryManager>();
        weaponSlots = weaponSlotParent.GetComponentsInChildren<Slot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<Slot>();
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].gameObject.SetActive(false);
        }
    }
    //아이템 획득
    public bool AcquireItem(EquipmentItem item)
    {
        if(item.itemType == EquipmentItem.ItemType.Weapon)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i].itemName == string.Empty)
                {
                    //아이템 리스트 에 넣기
                    MyItemList.Add(new Item(++lastNum, item.itemType.ToString(), 
                        item.name, item.weaponType.ToString(),
                        item.damage.ToString(), item.shield.ToString()));
                    //json 데이터 파일 또는 서버 인벤토리에 넣기
                    inventoryManager.AddItemSave
                        (lastNum,item.itemType.ToString(),item.name,item.weaponType.ToString(),
                        item.damage.ToString(),item.shield.ToString());
                    weaponSlots[i].AddItem(lastNum, item);
                    weaponSlots[i].gameObject.SetActive(true);
                    return true;
                }
            }
        }
        if (item.itemType == EquipmentItem.ItemType.Equipment)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].itemName == string.Empty)
                {
                    MyItemList.Add(new Item(++lastNum, item.itemType.ToString(), item.name, item.weaponType.ToString(),
                        item.damage.ToString(), item.shield.ToString()));
                    inventoryManager.AddItemSave
                        (lastNum,item.itemType.ToString(), item.name, item.weaponType.ToString(),
                        item.damage.ToString(), item.shield.ToString());
                    equipmentSlots[i].AddItem(lastNum, item);
                    equipmentSlots[i].gameObject.SetActive(true);
                    return true;
                }
            }
        }
        return false;
    }

    public void StartAcquireItem()
    {
        MyItemList = inventoryManager.MyItemList;
        for (int j = 0; j < MyItemList.Count; j++)
        {
            lastNum = MyItemList[j].num;
            if (MyItemList[j].type.Equals(EquipmentItem.ItemType.Weapon.ToString()))
            {
                for (int i = 0; i < weaponSlots.Length; i++)
                {
                    if (weaponSlots[i].itemName == string.Empty)
                    {
                        item.itemName = MyItemList[j].name;
                        item.itemType = EquipmentItem.ItemType.Weapon;
                        if(MyItemList[j].weaponType.Equals(EquipmentItem.WeaponType.DMRGun.ToString()))
                            item.weaponType = EquipmentItem.WeaponType.DMRGun;
                        else if (MyItemList[j].weaponType.Equals(EquipmentItem.WeaponType.RifleGun.ToString()))
                            item.weaponType = EquipmentItem.WeaponType.RifleGun;
                        else if (MyItemList[j].weaponType.Equals(EquipmentItem.WeaponType.ShotGun.ToString()))
                            item.weaponType = EquipmentItem.WeaponType.ShotGun;
                        item.damage = int.Parse(MyItemList[j].damage);
                        item.shield = int.Parse(MyItemList[j].shield);
                        weaponSlots[i].AddItem(MyItemList[j].num,item);
                        weaponSlots[i].gameObject.SetActive(true);
                        break;
                    }
                }
            }
            if (MyItemList[j].type.Equals(EquipmentItem.ItemType.Equipment.ToString()))
            {
                for (int i = 0; i < equipmentSlots.Length; i++)
                {
                    if (equipmentSlots[i].itemName == string.Empty)
                    {
                        item.itemName = MyItemList[j].name;
                        item.itemType = EquipmentItem.ItemType.Equipment;
                        item.weaponType = EquipmentItem.WeaponType.none;
                        item.damage = int.Parse(MyItemList[j].damage);
                        item.shield = int.Parse(MyItemList[j].shield);
                        equipmentSlots[i].AddItem(MyItemList[j].num, item);
                        equipmentSlots[i].gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
    }
    //아이템 장착하면 나머지 장착색깔 비활성화
    public void ClearWeaponSlotColor()
    { 
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            Color color = weaponSlots[i].slotImage.color;
            color.a = 0;
            weaponSlots[i].slotImage.color = color;
            weaponSlots[i].equipSlot = false;
        }
    }
    //장비일경우
    public void ClearEquipmentSlotColor()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            Color color = equipmentSlots[i].slotImage.color;
            color.a = 0;
            equipmentSlots[i].slotImage.color = color;
            equipmentSlots[i].equipSlot = false;
        }
    }
    //아이템 선택하면 나머지 아이템 선택 이미지 비활 성화와 
    //또는 장비창에서 무기창으로 교체할 때도 작동한다.
    public void ClearSlotChoose()
    {
        // 선택된 이미지 비활성화
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i].chooseImage.SetActive(false);
            weaponSlots[i].chosenSlot = false;
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].chooseImage.SetActive(false);
            equipmentSlots[i].chosenSlot = false;
        }
    }
    //아이템이 선택된 상테에서 삭제버튼을 누르면 작동한다.
    public void DeleteSlotChoose()
    {
        
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i].chooseImage.activeSelf == true)
            {
                for (int j = 0; j < MyItemList.Count; j++)
                {
                    if (MyItemList[j].num == weaponSlots[i].num)
                    {
                        Item item = MyItemList[j];
                        int num = item.num;
                        MyItemList.Remove(item);
                        string jdata = JsonUtility.ToJson(new Serialization<Item>(MyItemList));
                        inventoryManager.DeleteItemSave(jdata,num);
                        break;
                    }
                }
                weaponSlots[i].ClearSlot();
            }
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].chooseImage.activeSelf == true)
            {
                for (int j = 0; j < MyItemList.Count; j++)
                {
                    if (MyItemList[j].num == equipmentSlots[i].num)
                    {
                        Item item = MyItemList[j];
                        int num = item.num;
                        MyItemList.Remove(item);
                        string jdata = JsonUtility.ToJson(new Serialization<Item>(MyItemList));
                        inventoryManager.DeleteItemSave(jdata,num);
                        break;
                    }
                }
                equipmentSlots[i].ClearSlot();
            }
        }
        
    }
    //게임을 시작하면 자동으로 아이템을 장착한다.
    public void StartWeaponItem(int num)
    {
        for (int j = 0; j < MyItemList.Count; j++)
        {
            if (MyItemList[j].type.Equals(EquipmentItem.ItemType.Weapon.ToString()))
            {
                for (int i = 0; i < weaponSlots.Length; i++)
                {
                    if (weaponSlots[i].num == num)
                    {
                        weaponSlots[i].EquipItemSlot();
                        break;
                    }
                }
            }
        }
    }
    public void StartEquipItem(int num)
    {
        
        for (int j = 0; j < MyItemList.Count; j++)
        {
            if (MyItemList[j].type.Equals(EquipmentItem.ItemType.Equipment.ToString()))
            {
                for (int i = 0; i < equipmentSlots.Length; i++)
                {
                    if (equipmentSlots[i].num == num)
                    {
                        equipmentSlots[i].EquipItemSlot();
                        break;
                    }
                }
            }
        }
    }
}