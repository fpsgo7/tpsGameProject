using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
