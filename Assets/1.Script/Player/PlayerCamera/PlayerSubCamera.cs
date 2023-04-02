using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 인벤토리를 볼때 캐릭터를 보기위한 카메라
/// 
/// </summary>
public class PlayerSubCamera : MonoBehaviour
{
    public GameObject ForrowCamInventory;

    public void ActiveForrowCamInventory()
    {
        ForrowCamInventory.SetActive(true);
    }

    public void InactiveForrowCamInventory()
    {
        ForrowCamInventory.SetActive(false);
    }
}
