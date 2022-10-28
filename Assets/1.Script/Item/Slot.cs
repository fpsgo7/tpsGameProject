﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public enum ItemType  // 아이템 유형
    {
        Equipment,
        Weapon
    }
    public ItemType itemType;// 아이템 타입
    public enum WeaponType
    {// 무기 타입
        ShotGun,
        RifleGun,
        DMRGun
    }
    public WeaponType weaponType;
    public EquipmentItem equipmentItem;// 획득한 총기 아이템
    public PlayerShooter playerShooter;
    public Inventory inventory;
    public Image itemImage;// 아이탬의 이미지
    public Text itemNameText;
    public Text abilityText;

    public string itemName=string.Empty; // 아이템 이름
    public float damage;// 장비 대미지
    public float shield;// 장비 방어력

    // 아이템 이미지의 투명도 조절
    private void SetColor(float alphaColor)
    {
        Color color = itemImage.color;
        color.a = alphaColor;
        itemImage.color = color;
    }

    //인벤토리에 아이템 추가
    public void AddItem(EquipmentItem equipmentItem)
    {
        this.equipmentItem = equipmentItem;
        itemName = equipmentItem.itemName;
        itemImage.sprite = equipmentItem.itemImage;
        itemType = (ItemType)equipmentItem.itemType;
        if (itemType == ItemType.Equipment)
        {
            shield = equipmentItem.shield;
            abilityText.text = "방어력" + shield.ToString();
        }  
        if (itemType == ItemType.Weapon)
        {
            damage = equipmentItem.damage;
            abilityText.text ="데미지"+ damage.ToString();
            weaponType = (WeaponType)equipmentItem.weaponType;
        }
        itemNameText.text=itemName;
        

        SetColor(1);// 투명했던 이미지를 다시 보이게함
    }
    //아이템이 사라지면 초기화
    public void ClearSlot()
    {
        itemName = null;
        equipmentItem = null;
        itemImage.sprite = null;
        SetColor(0);
    }
    //클릭 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemName != null)
            {
                if (weaponType == WeaponType.RifleGun)
                    playerShooter.ChooseGun(0,damage);
                if (weaponType == WeaponType.DMRGun)
                    playerShooter.ChooseGun(1,damage);
                if (weaponType == WeaponType.ShotGun)
                    playerShooter.ChooseGun(2,damage);
                // 색깔을 설정하여 장착이 된것을 표시
                inventory.ClearColor();//전체색깔 초기화
                Color color=this.GetComponent<Image>().color;
                color.a = 0.5f;
                this.GetComponent<Image>().color = color;
                
            }
        }
    }
}
