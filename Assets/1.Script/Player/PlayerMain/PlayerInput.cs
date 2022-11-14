using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //다른 스크립트와 연결하기
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    //입력감지를 위해 사용하는 변수 const 사용으로 상수화 시켜 최적화
    public const string fireButtonName = "Fire1";
    public const string FireGrenadeButtonName = "Grenade";//g 키
    public const string zoomInButtonName = "ZoomIn";// 우클릭
    public const string jumpButtonName = "Jump";
    public const string moveHorizontalAxisName = "Horizontal";
    public const string moveVerticalAxisName = "Vertical";
    public const string reloadButtonName = "Reload";
    public const string restoreHealthButtonName = "RestoreHealth";//v 키
    public const string scopeZoomInButtonName = "ScopeZoomIn";//tab 키
    private const string ESCButtonName = "Cancel";//esc 키
    private const string InventoryButtonName = "Inventory";//m 키

    //실제로 입력된 값들을 저장할 프로퍼티
    //값을 읽을때는 public 형이라 밬에서 읽기 쉽지만
    //값설정은 private 로 막아뒀다.
    public Vector2 moveInput { get; private set; }//방향을 위해 사용
    public bool fire { get; private set; }
    public bool Grenade { get; private set; }
    public bool zoomIn { get; private set; }
    public bool reload { get; private set; }
    public bool jump { get; private set; }
    public bool restoreHealth { get; private set; }
    public bool scopeZoomIn { get; private set; }

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        //체력 회복 관리
        if (Input.GetButtonDown(restoreHealthButtonName))
            restoreHealth = true;
        if (Input.GetButtonUp(restoreHealthButtonName))
            restoreHealth = false;

        //메뉴 온오프 
        if (Input.GetButtonDown(ESCButtonName))
        {
            //Debug.Log("메뉴 키입력");
            UIManager.Instance.MenuOnOff();
        }
        if (Input.GetButtonDown(InventoryButtonName))
        {
            UIManager.Instance.InventoryOnOff();
        }

        //게임 오버가 되거나 체력 회복 중인 경우와 메뉴를 보는 동안 정지한다.
        if (GameManager.Instance != null
            && GameManager.Instance.isGameover || playerHealth.restoreHealthProceeding == true
            || UIManager.Instance.menuUIOpen == true
            )
        {
            moveInput = Vector2.zero;
            fire = false;
            zoomIn = false;
            reload = false;
            jump = false;
            Grenade = false;
            return;
        }
        // 점프중인 경우
        if(playerMovement.jumpState == true)
        {
            fire = false;
            zoomIn = false;
            reload = false;
            jump = false;
            return;
        }

        //moveInput 에 입력된 값을 할당한다. new Vector2(수직방향입력,수평방향입력)
        moveInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));
        //move Input 값이 1보다 크면 그값을 1로 교환해주어 속도가 잘못되는 현상을 방지한다.
        if (moveInput.sqrMagnitude > 1) moveInput = moveInput.normalized;//정규화 활용
        // 해당 파라미터에 입력을 받아 true 와 false 로 바꿔 조종하는데 쓴다.
        jump = Input.GetButtonDown(jumpButtonName);
        fire = Input.GetButton(fireButtonName);
        Grenade = Input.GetButtonDown(FireGrenadeButtonName);
        zoomIn = Input.GetButton(zoomInButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        //스코프 조작
        scopeZoomIn = Input.GetButton(scopeZoomInButtonName);

       
    }
}