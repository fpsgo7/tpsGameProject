using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //플레이어 컴포넌트 연결 변수
    private CharacterController characterController;
    private PlayerInput playerInput;
    private PlayerShooter playerShooter;
    private PlayerHealth playerHealth;
    private Animator animator;
    //카메라를 기준으로 움직이기에 필요한 카메라 변수
    private Camera followCam;
    //애니메이션 최적화를 위한 해쉬값
    public readonly int hashJump = Animator.StringToHash("Jump");
    public readonly int hashVerticalMove = Animator.StringToHash("Vertical Move");
    public readonly int hashHorizontalMove = Animator.StringToHash("Horizontal Move");
    //플레이어 값
    [HideInInspector] public float speed;//속도
    [HideInInspector] public float runSpeed;// 일반속도
    [HideInInspector] public float walkSpeed;// 걷는속도
    [HideInInspector] public float jumpSpeed;// 구르는 동안의 속도
    [HideInInspector] public float jumpStopSpeed;// 점프 중 느려야 하는 구간 속도
    [Range(0.01f, 1f)] private float airControlPercent = 0.1f;//공중 속도
    //스무스의 지연 값
    private float speedSmoothTime = 0.1f;
    private float turnSmoothTime = 0.1f;
    //값의 변화 속도 damping 관련
    private float speedSmoothVelocity;
    private float turnSmoothVelocity;
    //플레이어의 y 축 방향 속도
    private float currentVelocityY;
    //점프 딜레이 
    private float waitingForJump = 4f;//점프 딜레이
    private float lastJumpTime; //마지막 점프시간
    [HideInInspector] public bool isJumpState = false;// 점프 상태

    //지면상의 현제 속도를 표현한다. 람다식 활용
    public float currentSpeed =>
        new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;//x축 과 z 축의 값을 백터 형식으로 구한다.
    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerShooter = GetComponent<PlayerShooter>();
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        followCam = Camera.main;
        speed = 3f;
        jumpSpeed = 6.1f;
        walkSpeed = 2.5f;
        runSpeed = 5f;
        jumpStopSpeed = 0.1f;
    }

    private void FixedUpdate()//업데이트 문 필요
    {
        //회전 관련 함수를 실행하게함
        if (currentSpeed > 0.2f || playerInput.IsFire || playerShooter.aimState == PlayerShooter.AimState.HipFire) 
            Rotate();
        //움직임 함수를 실행하게함
        Move(playerInput.moveInput);
        //점프함수를 실행하게함
        if (playerInput.IsJump && Time.time >= lastJumpTime + waitingForJump)
            Jump();
    }

    private void Update()
    {
        UpdateAnimation(playerInput.moveInput);//playerInput의 moveInput 값을 보내준다.
    }

    public void Move(Vector2 moveInput)
    {
        float targetSpeed = speed * moveInput.magnitude;//속도 결정
        Vector3 moveDiection 
            = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);//방향 결정
        currentVelocityY += Time.deltaTime * Physics.gravity.y;// 중력 역할을함  Physics.gravity.y 에는 중력값으로 -9.8이 들어가 있다.

        //지연시간
        float smoothTime 
            = characterController.isGrounded ? speedSmoothTime : speedSmoothTime / airControlPercent;//땅에 있는지 체크한후 스무스 스피드 값을 넣는다.

        //목표 속도=Mathf.SmoothDamp(기본 속도, 목표속도,ref 값의 변화량, 지연시간)
        targetSpeed
            = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, smoothTime);

        //압뒤 좌우 속도 구한후 위아레 속도를 합한다.
        Vector3 velocity 
            = moveDiection * targetSpeed + Vector3.up * currentVelocityY;

        characterController.Move(velocity*Time.deltaTime);//실제로 움직이게한다.

        if (characterController.isGrounded)//발이 땅에 닿아있으면 중력값을 초기화 시켜 내려가는 힘을 없앤다.
            currentVelocityY = 0f;

    }

    public void Rotate()
    {
        float targetRotation = followCam.transform.eulerAngles.y;//카메라의 y 축 각도 가져옴
        //Mathf.SmoothDampAngle 로 부드럽게 각도를 바꾸게함
        targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetRotation,ref turnSmoothVelocity, turnSmoothTime);//(현제값, 목표값, 속도, 시간)

        transform.eulerAngles = Vector3.up * targetRotation;// y 축에대해서만 값을 설정함

    }
    //점프하기
    public void Jump()
    {
        lastJumpTime = Time.time;
        animator.SetTrigger(hashJump);
    }
    public void JumpStart()
    {
        //Debug.Log("점프시작");
        playerHealth.isInvincibility = true;
        isJumpState = true;
        speed = jumpStopSpeed;
    }
    public void JumpMoveStart()
    {
        speed = jumpSpeed;
    }
    public void JumpMoveStop()
    {
        speed = jumpStopSpeed;
    }
    public void JumpEnd()
    {
        //Debug.Log("점프끝");
        playerHealth.isInvincibility = false;
        isJumpState = false;
        animator.applyRootMotion = false;
        speed = runSpeed;
    }

    //사용자 의 입력을 받아 에니메이션을 업데이트함
    private void UpdateAnimation(Vector2 moveInput)
    {
        //부드럽게 값이 변화 하기위해 사용
        float animationSpeedPercent = currentSpeed / speed;
        if(playerInput.IszoomIn == true)
        {
            animator.SetFloat(hashVerticalMove, moveInput.y * animationSpeedPercent/2, 0.05f, Time.deltaTime);
            animator.SetFloat(hashHorizontalMove, moveInput.x * animationSpeedPercent/2, 0.05f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(hashVerticalMove, moveInput.y * animationSpeedPercent, 0.05f, Time.deltaTime);
            animator.SetFloat(hashHorizontalMove, moveInput.x * animationSpeedPercent, 0.05f, Time.deltaTime);
        }
        
    }
}