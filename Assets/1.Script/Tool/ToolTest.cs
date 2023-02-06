using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTest : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject explosion = EffectToolManager.Instance.EffectUse((int)EffectList.explosion,
               this.transform.position );
            SoundToolManager.Instance.PlayOneShotSound((int)SoundList.playerhitsound, transform.position, 1f);
        }
    }
}
