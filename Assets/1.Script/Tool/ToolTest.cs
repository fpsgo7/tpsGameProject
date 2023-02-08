using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTest : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            EffectToolManager.Instance.GetEffect(1, transform.position, transform.position);
            SoundToolManager.Instance.PlayOneShotSound((int)SoundList.playerhitsound, transform.position, 1f);
        }
    }
}
