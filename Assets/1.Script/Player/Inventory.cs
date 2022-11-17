using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //인벤토리 열기 닫기는 유아이 메니져에서 관리
    [SerializeField] private GameObject weaponSlotParent;// 무기 슬롯의 레이아웃
    [SerializeField] private GameObject equipmentSlotParent;// 장비 킷트의 레이아웃
    [SerializeField] private Slot[] weaponSlots;//무기 슬롯들
    [SerializeField] private Slot[] equipmentSlots;//장비 슬롯들
    [SerializeField] public List<Item> MyItemList;// 받아온 정보를 넣을 리스트 
    private string getJdata = string.Empty;
    EquipmentItem item = new EquipmentItem();

    void Awake()
    {
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

    public bool AcquireItem(EquipmentItem item)
    {
        if(item.itemType == EquipmentItem.ItemType.Weapon)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (weaponSlots[i].itemName == string.Empty)
                {
                    JsonInventoryManager.Instance.AddItemSave
                        (item.itemType.ToString(),item.name,item.weaponType.ToString(),
                        item.damage.ToString(),item.shield.ToString(),false);
                    weaponSlots[i].AddItem(item);
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
                    JsonInventoryManager.Instance.AddItemSave
                        (item.itemType.ToString(), item.name, item.weaponType.ToString(),
                        item.damage.ToString(), item.shield.ToString(), false);
                    equipmentSlots[i].AddItem(item);
                    equipmentSlots[i].gameObject.SetActive(true);
                    return true;
                }
            }
        }
        return false;
    }

    public void StartAcquireItem()
    {
        getJdata = JsonInventoryManager.Instance.jdata;
        if (getJdata == string.Empty)
            return;
        MyItemList = JsonUtility.FromJson<Serialization<Item>>(getJdata).target;
        for (int j = 0; j < MyItemList.Count; j++)
        {
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
                        weaponSlots[i].AddItem(item);
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
                        Debug.Log(j + " " + item.itemName);
                        Debug.Log(j + " " + item.itemType);
                        Debug.Log(j + " " + item.weaponType);
                        Debug.Log(j + " " + item.damage);
                        Debug.Log(j + " " + item.shield);
                        equipmentSlots[i].AddItem(item);
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
    //아이템 선택하면 나머지 아이템 선택 이미지 비활 성화와 
    //또는 장비창에서 무기창으로 교체할 때도 작동한다.
    public void ClearSlotChooseImage()
    {
        // 선택된 이미지 비활성화
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i].chooseImage.SetActive(false);
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].chooseImage.SetActive(false);
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
                    if (MyItemList[j].name == weaponSlots[i].itemName)
                    {
                        Item item = MyItemList[j];
                        MyItemList.Remove(item);
                        string jdata = JsonUtility.ToJson(new Serialization<Item>(MyItemList));
                        JsonInventoryManager.Instance.DeleteItemSave(jdata);
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
                    if (MyItemList[j].name == equipmentSlots[i].itemName)
                    {
                        Item item = MyItemList[j];
                        MyItemList.Remove(item);
                        string jdata = JsonUtility.ToJson(new Serialization<Item>(MyItemList));
                        JsonInventoryManager.Instance.DeleteItemSave(jdata);
                        break;
                    }
                }
                equipmentSlots[i].ClearSlot();
            }
        }
        
    }
}
