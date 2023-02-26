﻿using UnityEngine;
/// <summary>
/// 플레이어의 입력값을 받아 
/// 플레이어가 이동하게 해준다.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //플레이어 컴포넌트 연결 변수
    private CharacterController characterController;
    private PlayerInput playerInput;
    private PlayerShooter playerShooter;
    private PlayerHealth playerHealth;
    private PlayerCamera playerCamera;
    private Animator playerAnimator;
    //카메라를 기준으로 움직이기에 필요한 카메라 변수
    private Camera followCam;
    //애니메이션 최적화를 위한 해쉬값
    public readonly int hashJump = Animator.StringToHash("Jump");
    public readonly int hashRun = Animator.StringToHash("Run");
    public readonly int hashVerticalMove = Animator.StringToHash("Vertical Move");
    public readonly int hashHorizontalMove = Animator.StringToHash("Horizontal Move");
    //플레이어 값
    public float speed;//속도
    [HideInInspector] public float runSpeed;//뛰기 속도
    [HideInInspector] public float fireWalkSpeed;// 사격 걷기 속도
    [HideInInspector] public float walkSpeed;// 일반속도
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
    [HideInInspector] public bool isRunState = false;// 뛰기 상태
    [HideInInspector] public bool isRunSpeed = false; // 뛰기 속도 도달
    //지면상의 현제 속도를 표현한다. 람다식 활용
    public float currentSpeed =>
        new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;//x축 과 z 축의 값을 백터 형식으로 구한다.
    // 플레이어 오브젝트 접근하기
    public Transform playerTransform;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerShooter = GetComponent<PlayerShooter>();
        playerHealth = GetComponent<PlayerHealth>();
        playerCamera = GetComponent<PlayerCamera>();
        playerAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        followCam = Camera.main;
        jumpSpeed = 6.1f;
        fireWalkSpeed = 2f;
        jumpStopSpeed = 0.1f;
        walkSpeed = 4f;
        runSpeed = 7f; 
        speed = walkSpeed;
    }

    private void FixedUpdate()//업데이트 문 필요
    {
        //회전 관련 함수를 실행하게함
        if (currentSpeed > 0.2f ||playerInput.IsZoomIn|| playerInput.IsFire || playerShooter.aimState == PlayerShooter.AimState.FireReady) 
            Rotate();
        //움직임 함수를 실행하게함
        Move(playerInput.moveInput);
       
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
        if (isRunState == true)
            moveDiection = Vector3.Normalize(transform.forward * moveInput.y);//방향 결정

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
        //실제로 움직이게한다.
        characterController.Move(velocity*Time.deltaTime);

        if (characterController.isGrounded)//발이 땅에 닿아있으면 중력값을 초기화 시켜 내려가는 힘을 없앤다.
            currentVelocityY = 0f;

    }

    public void Rotate()
    {
        Vector3 forward = followCam.transform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;//앞방향의 y 축은 0으로 함
        forward = forward.normalized;//단일백터화시킴
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);// right는 좌우값을 위해 사용한다.
        Vector3 targetDirection = Vector3.zero;
        if (isRunState == true)
            targetDirection = forward * (playerInput.moveInput.y * 0.5f) + right * (playerInput.moveInput.x * 0.5f);
        else
            targetDirection = forward;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);// 목표각도를 목표 위치벡터값을 통해 구한다.
        characterController.transform.rotation = Quaternion.Slerp(characterController.transform.rotation,
            targetRotation, turnSmoothTime);//움직여야할 목표위치와 부드러운 정
        // 전에 쓰던 방식
        //float targetRotation = followCam.transform.eulerAngles.y;//카메라의 y 축 각도 가져옴
        ////Mathf.SmoothDampAngle 로 부드럽게 각도를 바꾸게함
        //targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);//(현제값, 목표값, 속도, 시간)
        //transform.eulerAngles = Vector3.up * targetRotation;// y 축에대해서만 값을 설정함
    }

    public void RunStart()
    {
        if (!playerInput.IsZoomIn)
        {
            isRunState = true;
            playerAnimator.SetLayerWeight(1, 0.8f);
            speed = runSpeed;
            playerAnimator.SetBool(hashRun, true);
            playerCamera.SetCameraFov(PlayerCamera.FovValues.RUN, 2);

        }
    }
    public void IsRunSpeed()
    {
        if(isRunSpeed == false)
        {
            isRunSpeed = true;
        }
    }
    public void RunEnd()
    {
        if(isRunState == true)
        {
            isRunState = false;
            playerShooter.SetSpineDegree();
            playerAnimator.SetLayerWeight(1, 0.3f);
            speed = walkSpeed;
            playerAnimator.SetBool(hashRun, false);
            playerCamera.SetCameraFov(PlayerCamera.FovValues.ZOOMOUT, 2);
        }
    }
    //점프하기
    public void Jump()
    {
        if (!playerInput.IsZoomIn)
        {
            isJumpState = true;
            if (Time.time >= lastJumpTime + waitingForJump)
            {
                lastJumpTime = Time.time;
                playerAnimator.SetTrigger(hashJump);
            }
        }  
    }
    // 밑의 점프 시작 끝 호출은 애니메이션에서 호출함
    public void JumpStart()
    {
        //Debug.Log("점프시작");
        playerHealth.isInvincibility = true;
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
        playerAnimator.applyRootMotion = false;
        if (playerInput.RunKeyActionCheck())
        {
            speed = walkSpeed;
        }
        else
        {
            speed = runSpeed;
        }
    }

    //사용자 의 입력을 받아 에니메이션을 업데이트함
    private void UpdateAnimation(Vector2 moveInput)
    {
        //부드럽게 값이 변화 하기위해 사용
        float animationSpeedPercent = currentSpeed / speed;
        if(playerInput.IsZoomIn == true)
        {
            playerAnimator.SetFloat(hashVerticalMove, moveInput.y * animationSpeedPercent/2, 0.05f, Time.deltaTime);
            playerAnimator.SetFloat(hashHorizontalMove, moveInput.x * animationSpeedPercent/2, 0.05f, Time.deltaTime);
        }
        else
        {
            playerAnimator.SetFloat(hashVerticalMove, moveInput.y * animationSpeedPercent, 0.05f, Time.deltaTime);
            playerAnimator.SetFloat(hashHorizontalMove, moveInput.x * animationSpeedPercent, 0.05f, Time.deltaTime);
        }
        
    }
}