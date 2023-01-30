using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 장비나 , 무기 아이템과
/// 충돌하면 해당 아이템을 먹는 기능을 수행한다.
/// </summary>
public class EquipmentItem : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    private bool itemGetSucess;
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
    public string itemName; // 아이템 이름
    public int damage;// 장비 대미지
    public int shield;// 장비 방어력

    private void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemGetSucess = inventory.AcquireItem(this))
            {
                Destroy(gameObject.transform.parent.transform.gameObject);
                //Destroy(gameObject);
            }
        }
        else if (other.CompareTag("otherPlayer"))
        {
            Destroy(gameObject.transform.parent.transform.gameObject);
        }
    }
}
