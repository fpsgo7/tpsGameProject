using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어의 입력을 전체적으로 관리한다.
/// </summary>

public class PlayerInput : MonoBehaviour
{
    private const string moveHorizontalAxisName = "Horizontal";
    private const string moveVerticalAxisName = "Vertical";
    private const string ESCButtonName = "Cancel";//esc 키
    public Vector2 moveInput { get; private set; }//방향을 위해 사용
    public bool IsFire { get; private set; }
    public bool IsZoomIn { get; private set; }
    public bool IsScopeZoomIn { get; private set; }
    public bool IsInteraction { get; private set; }

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;
    private PlayerInteraction playerInteraction;
    private FireGrenade fireGrenade;
    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerShooter = GetComponent<PlayerShooter>();
        fireGrenade = GetComponent<FireGrenade>();
    }

    private void Update()
    {
        //체력 회복 관리
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.RESTOREHEALTH]))
            playerHealth.isRestoringHealth = true;
        if (Input.GetKeyUp(KeySetting.keys[KeyAction.RESTOREHEALTH]))
            playerHealth.isRestoringHealth = false;
        //메뉴 온오프 
        if (Input.GetButtonDown(ESCButtonName))
        {
            UIMenu.Instance.SetMenuOnOff();
        }
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.INVENTORY]))
        {
            UIMenu.Instance.SetInventoryOnOff();
        }
        //게임 오버가 되거나 체력 회복 중인 경우와 메뉴를 보는 동안 정지한다.
        if (GameManager.Instance != null
            && GameManager.Instance.isGameover || playerHealth.isRestoreHealthProceeding == true
            || UIMenu.Instance.isMenuUI == true)
        {
            moveInput = Vector2.zero;
            IsFire = false;
            IsZoomIn = false;
            IsInteraction = false;
            IsScopeZoomIn = false;
            return;
        }
        // 점프중인 경우
        if (playerMovement.isJumpState == true)
        {
            return;
        }
        //뛰기가 눌러지는 경우
        if (playerMovement.isRunState == true)
        {
            moveInput = Vector2.up;
            if (Input.GetKeyDown(KeySetting.keys[KeyAction.JUMP]))
                playerMovement.Jump();
            if (Input.GetKeyUp(KeySetting.keys[KeyAction.RUN]))
                playerMovement.RunEnd();
            return;
        }
        //moveInput 에 입력된 값을 할당한다. new Vector2(수직방향입력,수평방향입력)
        moveInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));
        IsFire = Input.GetKey(KeySetting.keys[KeyAction.FIRE]);
        IsInteraction = Input.GetKeyDown(KeySetting.keys[KeyAction.INTERACTION]);
        IsScopeZoomIn = Input.GetKey(KeySetting.keys[KeyAction.SCOPEZOOMIN]);
        IsZoomIn = Input.GetKey(KeySetting.keys[KeyAction.ZOOMIN]);
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.JUMP]))
            playerMovement.Jump();
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.GRENADE]))
            fireGrenade.Fire();
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.RELOAD]))
            playerShooter.Reload();
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.RUN]))
            playerMovement.RunStart();
    }
}
