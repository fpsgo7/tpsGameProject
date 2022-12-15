using System.Collections;
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
        DMRGun,
        none
    }
    public WeaponType weaponType;

    public EquipmentItem equipmentItem;// 획득한 총기 아이템
    public PlayerShooter playerShooter;
    public PlayerHealth playerHealth;
    public Inventory inventory;
    public GameObject chooseImage;//아이템 선택 이미지
    public Sprite[] itemImages;// 아이탬의 이미지들
    public Image itemImage;// 아이탬의 이미지
    public Text itemNameText;
    public Text abilityText;
    public Image slotImage;

    public int num;// 아이템 번호
    public string itemName=string.Empty; // 아이템 이름
    public float damage;// 장비 대미지
    public float shield;// 장비 방어력
    public bool chosenSlot=false;// 슬롯 선택상태값
    public bool equipSlot = false;//슬록 장착 값

    // 아이템 이미지의 투명도 조절
    private void SetColor(float alphaColor)
    {
        Color color = itemImage.color;
        color.a = alphaColor;
        itemImage.color = color;
    }

    //인벤토리에 아이템 추가
    public void AddItem(int num,EquipmentItem equipmentItem )
    {
        this.equipmentItem = equipmentItem;
        this.num=num;
        itemName = equipmentItem.itemName;
        itemType = (ItemType)equipmentItem.itemType;
        if (itemType == ItemType.Equipment)
        {
            shield = equipmentItem.shield;
            abilityText.text = "방어력" + shield.ToString();
            itemImage.sprite = itemImages[3];
        }  
        if (itemType == ItemType.Weapon)
        {
            damage = equipmentItem.damage;
            abilityText.text ="데미지"+ damage.ToString();
            weaponType = (WeaponType)equipmentItem.weaponType;
            if(weaponType == WeaponType.RifleGun)
                itemImage.sprite = itemImages[0];
            if(weaponType == WeaponType.ShotGun)
                itemImage.sprite = itemImages[1];
            if(weaponType == WeaponType.DMRGun)
                itemImage.sprite = itemImages[2];
        }
        itemNameText.text=itemName;
        SetColor(1);// 투명했던 이미지를 다시 보이게함
    }
    //아이템이 사라지면 초기화
    public void ClearSlot()
    {
        equipmentItem = null;
        itemName = string.Empty;
        shield = 0;
        damage = 0;
        abilityText.text = null;
        itemNameText.text = null;
        itemImage.sprite = null;
        SetColor(0);
        this.gameObject.SetActive(false);
    }
    //클릭 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //클릭을 받으면 아이템이 선택이 된것을 표시한다.
            if(itemName != null && chosenSlot == false && equipSlot == false)
            {
                inventory.ClearSlotChoose();
                chosenSlot = true;
                chooseImage.SetActive(true);
            }
            else// 선택된 것을 비활성화 
            {
                inventory.ClearSlotChoose();
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right 
            && chosenSlot == false && equipSlot == false)
        {
            EquipItemSlot();   
        }
    }

    public void EquipItemSlot()
    {
        if (itemName != null)
        {
            // 색깔을 설정하여 장착이 된것을 표시
            if (itemType == ItemType.Weapon)
            {
                inventory.ClearWeaponSlotColor();//전체색깔 초기화
                PlayerInfoManager.Instance.ChangePlayerWeapon(num);

                //무기 교체및 대미지 변환
                if (weaponType == WeaponType.RifleGun)
                    playerShooter.ChooseGun(0, damage);
                if (weaponType == WeaponType.DMRGun)
                    playerShooter.ChooseGun(1, damage);
                if (weaponType == WeaponType.ShotGun)
                    playerShooter.ChooseGun(2, damage);
            }
            if (itemType == ItemType.Equipment)
            {
                inventory.ClearEquipmentSlotColor();//전체색깔 초기화
                PlayerInfoManager.Instance.ChangePlayerEquipment(num);

                //장비에 따른 체력 변화
                playerHealth.EqipmentWear(shield);
            }

            Color color = slotImage.color;
            color.a = 0.5f;
            slotImage.color = color;
            equipSlot = true;
        }
    }
}
