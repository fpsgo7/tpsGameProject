using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void grenadeExplosion()
    {
        Invoke(nameof(DestroyBullet), 5f);
    }

    private void DestroyBullet()
    {
        GrenadeExplosionObjectPooling.ReturnObject(this);
    }
}
