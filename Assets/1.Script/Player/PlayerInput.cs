using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //다른 스크립트와 연결하기
    private PlayerHealth playerHealth;
    //입력감지를 위해 사용하는 변수
    public string fireButtonName = "Fire1";
    public string zoomInButtonName = "ZoomIn";// 우클릭
    public string jumpButtonName = "Jump";
    public string moveHorizontalAxisName = "Horizontal";
    public string moveVerticalAxisName = "Vertical";
    public string reloadButtonName = "Reload";
    public string restoreHealthButtonName = "RestoreHealth";//v 키

    //실제로 입력된 값들을 저장할 프로퍼티
    //값을 읽을때는 public 형이라 밬에서 읽기 쉽지만
    //값설정은 private 로 막아뒀다.
    public Vector2 moveInput { get; private set; }//방향을 위해 사용
    public bool fire { get; private set; }
    public bool zoomIn { get; private set; }
    public bool reload { get; private set; }
    public bool jump { get; private set; }
    public bool restoreHealth { get; private set; }

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (Input.GetButtonUp(restoreHealthButtonName))
            restoreHealth = false;

        //게임 오버가 되면 유저의 입력을 무시하는 코드를 실행한다.
        if (GameManager.Instance != null
            && GameManager.Instance.isGameover || playerHealth.restoreHealthProceeding == true)
        {
            moveInput = Vector2.zero;
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
        zoomIn = Input.GetButton(zoomInButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        if (Input.GetButtonDown(restoreHealthButtonName))
            restoreHealth = true;

    }
}