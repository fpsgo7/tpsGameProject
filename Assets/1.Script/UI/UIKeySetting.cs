using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum KeyAction
{
    FIRE, ZOOMIN, GRENADE, JUMP, RELOAD, RESTOREHEALTH,
    SCOPEZOOMIN, INVENTORY, INTERACTION, RUN
}
public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> keys
    = new Dictionary<KeyAction, KeyCode>();
}
/// <summary>
/// 키 새팅 UI를 관리한다.
/// </summary>
public class UIKeySetting : MonoBehaviour
{
    private int key = -1;
    private bool isKeySetting = false;
    private KeySettingInfoManager keySettingInfoManager;
    [SerializeField] Text[] ButtonTxt;

    private void Awake()
    {
        //키세팅 메니져에 저장된 keySettingData값들을 가져와 키보드에 적용함
        keySettingInfoManager = GameObject.Find("InfoManager").GetComponent<KeySettingInfoManager>();
        
        KeySetting.keys.Add(KeyAction.FIRE, (KeyCode)keySettingInfoManager.keySettingData.FIRE);
        KeySetting.keys.Add(KeyAction.ZOOMIN, (KeyCode)keySettingInfoManager.keySettingData.ZOOMIN);
        KeySetting.keys.Add(KeyAction.GRENADE, (KeyCode)keySettingInfoManager.keySettingData.GRENADE);
        KeySetting.keys.Add(KeyAction.JUMP, (KeyCode)keySettingInfoManager.keySettingData.JUMP);
        KeySetting.keys.Add(KeyAction.RELOAD, (KeyCode)keySettingInfoManager.keySettingData.RELOAD);
        KeySetting.keys.Add(KeyAction.RESTOREHEALTH, (KeyCode)keySettingInfoManager.keySettingData.RESTOREHEALTH);
        KeySetting.keys.Add(KeyAction.SCOPEZOOMIN, (KeyCode)keySettingInfoManager.keySettingData.SCOPEZOOMIN);
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
        if (isKeySetting)
        {
            //Event 로 현재 실행되는 Evnet를 불러온다.
            Event keyEvent = Event.current;
            if (keyEvent.isKey)//현제 눌린키의 값을 상용
            {
                Debug.Log(keyEvent.keyCode);
                //가져온 키코드를 값에 넣는다.
                KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;
                key = -1;
                SetButtonText();// 키코드가 바뀌면 버튼의 택스트를 업데이트함
                SaveKeySetting();//변경된 키값들을 온라인이면 서버 오프라인이면 파일에 저장함
                isKeySetting = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                KeySetting.keys[(KeyAction)key] = KeyCode.Mouse0;
                key = -1;
                SetButtonText();// 키코드가 바뀌면 버튼의 택스트를 업데이트함
                SaveKeySetting();//변경된 키값들을 온라인이면 서버 오프라인이면 파일에 저장함
                isKeySetting = false;
            }else if (Input.GetMouseButtonDown(1))
            {
                KeySetting.keys[(KeyAction)key] = KeyCode.Mouse1;
                key = -1;
                SetButtonText();// 키코드가 바뀌면 버튼의 택스트를 업데이트함
                SaveKeySetting();//변경된 키값들을 온라인이면 서버 오프라인이면 파일에 저장함
                isKeySetting = false;
            }
            else if (Input.GetMouseButtonDown(2))
            {
                KeySetting.keys[(KeyAction)key] = KeyCode.Mouse2;
                key = -1;
                SetButtonText();// 키코드가 바뀌면 버튼의 택스트를 업데이트함
                SaveKeySetting();//변경된 키값들을 온라인이면 서버 오프라인이면 파일에 저장함
                isKeySetting = false;
            }
        }
    }
    //버튼을 눌러 대상을 선택하기위한 값을 적용함
    public void ChangeKey(int num)
    {
        isKeySetting = true;
        key = num;
    }

    public void SaveKeySetting()
    {
        keySettingInfoManager.keySettingData.FIRE = (int)KeySetting.keys[KeyAction.FIRE];
        keySettingInfoManager.keySettingData.ZOOMIN = (int)KeySetting.keys[KeyAction.ZOOMIN];
        keySettingInfoManager.keySettingData.GRENADE = (int)KeySetting.keys[KeyAction.GRENADE];
        keySettingInfoManager.keySettingData.JUMP = (int)KeySetting.keys[KeyAction.JUMP];
        keySettingInfoManager.keySettingData.RELOAD = (int)KeySetting.keys[KeyAction.RELOAD];
        keySettingInfoManager.keySettingData.RESTOREHEALTH = (int)KeySetting.keys[KeyAction.RESTOREHEALTH];
        keySettingInfoManager.keySettingData.SCOPEZOOMIN = (int)KeySetting.keys[KeyAction.SCOPEZOOMIN];
        keySettingInfoManager.keySettingData.INVENTORY = (int)KeySetting.keys[KeyAction.INVENTORY];
        keySettingInfoManager.keySettingData.INTERACTION = (int)KeySetting.keys[KeyAction.INTERACTION];
        keySettingInfoManager.keySettingData.RUN = (int)KeySetting.keys[KeyAction.RUN];
        //객체에 저장된 값들을 매계변수로 하여 저장 기능하는 함수 실행
        keySettingInfoManager.KeySettingSave(keySettingInfoManager.keySettingData);
    }

}
