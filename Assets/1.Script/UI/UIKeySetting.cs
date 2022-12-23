using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum KeyAction
{
    FIRE, ZOOMIN, GRENADE, JUMP, RELOAD, RESTOREHEALTH,
    SCOPEZOOMIN, CANCEL, INVENTORY, INTERACTION, RUN
}
public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> keys
    = new Dictionary<KeyAction, KeyCode>();
}
public class UIKeySetting : MonoBehaviour
{
    private int key = -1;
    private KeySettingInfoManager keySettingInfoManager;
    [SerializeField] Text[] ButtonTxt;
    KeySettingData keySettingData = new KeySettingData();

    private void Awake()
    {
        keySettingInfoManager = GameObject.Find("JsonManager").GetComponent<KeySettingInfoManager>();
        
        KeySetting.keys.Add(KeyAction.FIRE, (KeyCode)keySettingInfoManager.keySettingData.FIRE);
        KeySetting.keys.Add(KeyAction.ZOOMIN, (KeyCode)keySettingInfoManager.keySettingData.ZOOMIN);
        KeySetting.keys.Add(KeyAction.GRENADE, (KeyCode)keySettingInfoManager.keySettingData.GRENADE);
        KeySetting.keys.Add(KeyAction.JUMP, (KeyCode)keySettingInfoManager.keySettingData.JUMP);
        KeySetting.keys.Add(KeyAction.RELOAD, (KeyCode)keySettingInfoManager.keySettingData.RELOAD);
        KeySetting.keys.Add(KeyAction.RESTOREHEALTH, (KeyCode)keySettingInfoManager.keySettingData.RESTOREHEALTH);
        KeySetting.keys.Add(KeyAction.SCOPEZOOMIN, (KeyCode)keySettingInfoManager.keySettingData.SCOPEZOOMIN);
        KeySetting.keys.Add(KeyAction.CANCEL, (KeyCode)keySettingInfoManager.keySettingData.CANCEL);
        KeySetting.keys.Add(KeyAction.INVENTORY, (KeyCode)keySettingInfoManager.keySettingData.INVENTORY);
        KeySetting.keys.Add(KeyAction.INTERACTION, (KeyCode)keySettingInfoManager.keySettingData.INTERACTION);
        KeySetting.keys.Add(KeyAction.RUN, (KeyCode)keySettingInfoManager.keySettingData.RUN);
    }

    private void Start()
    {
        for (int i = 0; i<ButtonTxt.Length; i++)
        {
            ButtonTxt[i].text = KeySetting.keys[(KeyAction)i].ToString();
        }
    }

    private void SetButtonText()
    {
        for (int i = 0; i < ButtonTxt.Length; i++)
        {
            ButtonTxt[i].text = KeySetting.keys[(KeyAction)i].ToString();
        }
    }

    //GUI 키 입력등의 이벤트가 발생할 때 호출된다.
    private void OnGUI()
    {
        //Event 로 현재 실행되는 Evnet를 불러온다.
        Event keyEvent = Event.current;
        if (keyEvent.isKey)//현제 눌린키의 값을 상용
        {
            //가져온 키코드를 값에 넣는다.
            KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;
            key = -1;
        }
        SetButtonText();// 키코드가 바뀌면 버튼의 택스트를 업데이트함
        SetKeySetting();
    }
    //버튼을 눌러 대상을 선택하기위한 값을 적용함
    public void ChangeKey(int num)
    {
        key = num;
    }

    public void SetKeySetting()
    {
        Debug.Log("키를 설정합니다.");
        keySettingData.FIRE = (int)KeySetting.keys[KeyAction.FIRE];
        keySettingData.ZOOMIN = (int)KeySetting.keys[KeyAction.ZOOMIN];
        keySettingData.GRENADE = (int)KeySetting.keys[KeyAction.GRENADE];
        keySettingData.JUMP = (int)KeySetting.keys[KeyAction.JUMP];
        keySettingData.RELOAD = (int)KeySetting.keys[KeyAction.RELOAD];
        keySettingData.RESTOREHEALTH = (int)KeySetting.keys[KeyAction.RESTOREHEALTH];
        keySettingData.SCOPEZOOMIN = (int)KeySetting.keys[KeyAction.SCOPEZOOMIN];
        keySettingData.CANCEL = (int)KeySetting.keys[KeyAction.CANCEL];
        keySettingData.INVENTORY = (int)KeySetting.keys[KeyAction.INVENTORY];
        keySettingData.INTERACTION = (int)KeySetting.keys[KeyAction.INTERACTION];
        keySettingData.RUN = (int)KeySetting.keys[KeyAction.RUN];
        keySettingInfoManager.KeySettingSave(keySettingData);
    }

}
