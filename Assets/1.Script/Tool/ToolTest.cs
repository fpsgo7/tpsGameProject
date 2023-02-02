using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTest : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject explosion = EffectToolManager.Instance.EffectOneShot((int)EffectList.explosion,
               this.transform.position );
            //SoundToolManager.Instance.PlayOneShotEffect((int)stepSounds[index], transform.position, 0.2f);
        }
    }
}
