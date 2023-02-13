using UnityEngine;
using Cinemachine;
/// <summary>
/// 플레이어가 사격하는 기능 관련 스크립트로
/// 줌인 기능등 여러기능이 있으며
/// 마우스 회전은 여기서가아닌
/// 시네머신 카메라가 관리한다.
/// </summary>
public class PlayerShooter : MonoBehaviour
{
    //상태값 변수
    public enum AimState
    {
        Idle,
        FireReady//발사상태
    }
    //파라미터형 변수
    public AimState aimState { get; private set; }
    //사용할 오브젝트
    [HideInInspector] public Gun gun;// 총 스크립트와 연결
    [HideInInspector] public LateUpdateFollow lateUpdateFollow;//총잡는 부분 수적
    public Gun[] allGuns;//총기들 종합
    public LayerMask excludeTarget;//조준에서 제외할 대상
    [HideInInspector] public PlayerHealth playerHealth;//플레이어 헬스 스크립트와 연결
    private PlayerInput playerInput;//플레이어 입력 스크립트와 연결
    private PlayerMovement playerMovement;//플레이어 움직임 스크립트와 연결
    private Animator playerAnimator;//플레이어 에니메이션
    private Camera playerCamera;//플레이어 카메러
    //줌인카메라 케싱용 변수
    private CinemachineComposer forrowCamCinemachineComposerGetRig0;
    private CinemachineComposer forrowCamCinemachineComposerGetRig1;
    private CinemachineComposer forrowCamCinemachineComposerGetRig2;

    public GameObject scopeCamera;
    public GameObject scopeImage;
    public CinemachineFreeLook forrowCam;// 줌인 카메라
    public GameObject playerDgree;// 플레이어 각도 수정
    public Transform gunPivot;//총 위치를 위한 오브젝트

    //private float waitingTimeForReleasingAim = 2.5f;//총 조준후 다시 풀어지는 시간
    private float lastFireInputTime; //마지막 발사 시간
    //줌인 줌아웃 float 변수
    private const float zoomOutFieldOfView = 60f;
    private const float zoomOutScreenX = 0.45f;//0.4 전에 사용하던값
    private const float zoomOutTopScreenY = 0.55f;//0.6
    private const float zoomOutMidScreenY = 0.55f;
    private const float zoomOutBotScreenY = 0.55f;
    private const float zoomInFieldOfView = 40f;
    private const float zoomInScreenX = 0.35f;
    private const float zoomInTopScreenY = 0.65f;
    private const float zoomInMidScreenY = 0.65f;
    private const float zoomInBotScreenY = 0.65f;
    private float zoomFieldOfView = 0;
    private float zoomScreenX = 0;
    private float zoomTopScreenY = 0;
    private float zoomMidScreenY = 0;
    private float zoomBotScreenY = 0;
    private float waitingTimeForZoom = 0.000001f;//줌인 속도
    private float lastZoomTime; //줌 동작 한틱 기준
    //마우스 감도용 변수
    private float currentXAxis;
    private float currentYAxis;
    //애니메이터 해쉬값 변수
    public readonly int hashAngle = Animator.StringToHash("Angle");
    public readonly int hashShoot = Animator.StringToHash("Shoot");
    public readonly int hashReload = Animator.StringToHash("Reload");
    public readonly int hashFireReady = Animator.StringToHash("FireReady");

    private Vector3 aimPoint;//실제 조준대상 tps 기에 사용한다. 실제 조준점이 무조건 정중앙이 아니라서이다.
    private bool isLinedUp => !(Mathf.Abs(playerCamera.transform.eulerAngles.y - transform.eulerAngles.y) > 1f);//플레이어가 바라보는 각도와 실제 조준 각도를 너무큰치 체크해준다.
    //정면에 사격할수 있는지 적정거리가 되는지 체크하는 변수 (플레이어 케릭터 위치 + Vector3.up *
    //총의 발사 위치의 y축, 발사 포지션, 사격제외대상 레이어)
    //값에 따라 사격할수 없는 거리가되면 크로스 헤어를 비활성화 하고 스코프를 비활성화 시키기위한 조건값을 구해준다.
    private bool isEnoughDistance => !Physics.Linecast(transform.position + Vector3.up * gun.fireTransform.position.y, gun.fireTransform.position, ~excludeTarget);
    private bool isZoomIn=false;
    void Awake()
    {
        //플레이어가 자기자신을 쏘는 상황을 방지하기위하여 자기자신의 레이어를 추가한다.
        if (excludeTarget != (excludeTarget | (1 << gameObject.layer)))
        {
            excludeTarget |= 1 << gameObject.layer;
        }
        //줌관련 값을 초기화
        zoomFieldOfView = zoomOutFieldOfView;
        zoomScreenX = zoomOutScreenX;
        zoomTopScreenY = zoomOutTopScreenY;
        zoomMidScreenY = zoomOutMidScreenY;
        zoomBotScreenY = zoomOutBotScreenY;
    }

    private void Start()
    {
        playerCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        forrowCamCinemachineComposerGetRig0 = forrowCam.GetRig(0).GetCinemachineComponent<CinemachineComposer>();
        forrowCamCinemachineComposerGetRig1 = forrowCam.GetRig(1).GetCinemachineComponent<CinemachineComposer>();
        forrowCamCinemachineComposerGetRig2 = forrowCam.GetRig(2).GetCinemachineComponent<CinemachineComposer>();
        //카메라 값 초기화
        forrowCam.m_Lens.FieldOfView = zoomFieldOfView;
        forrowCamCinemachineComposerGetRig0.m_ScreenY = zoomTopScreenY;
        forrowCamCinemachineComposerGetRig1.m_ScreenY = zoomMidScreenY;
        forrowCamCinemachineComposerGetRig2.m_ScreenY = zoomBotScreenY;
        forrowCamCinemachineComposerGetRig0.m_ScreenX = zoomScreenX;
        forrowCamCinemachineComposerGetRig1.m_ScreenX = zoomScreenX;
        forrowCamCinemachineComposerGetRig2.m_ScreenX = zoomScreenX;
    }
    
    private void OnEnable()
    {
        aimState = AimState.Idle;
        //Debug.Log(LobbyScript.chooseWeapon);
        //ChooseGun(LobbyScript.chooseWeapon,0.0f);//무기 시작 선택기능은 삭제 보류
        EquipGun(allGuns[0], 10);
        gun.gameObject.SetActive(true);
        gun.Setup(this,0.0f);
    }

    private void OnDisable()
    {
        aimState = AimState.Idle;
        gun.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (playerInput.IsFire)
        {
            lastFireInputTime = Time.time;
            Shoot();
        }

       
        if (playerInput.IsZoomIn == true && isZoomIn == false)
        {
            ZoomIn();
        }
        else if (playerInput.IsZoomIn == false && isZoomIn == true && playerInput.IsScopeZoomIn == false)
        {
            ZoomOut();
        }

        if (playerInput.IsScopeZoomIn == true && scopeCamera.activeSelf == false
           && isEnoughDistance == true && gun.guns == Gun.Guns.DMRGUN && isZoomIn == true &&playerInput.IsZoomIn ==true)
        {
            ScopeZoomIn();
        }
        else if (playerInput.IsScopeZoomIn == false && scopeCamera.activeSelf == true
            || isEnoughDistance == false && gun.guns == Gun.Guns.DMRGUN)
        {   //스코프 줌인 입력 상태가 false 이고 스코프 카메라가 트루이거나 , 사격거리가 짧아 사격이 불가능할경우 실행
            ScopeZoomOut(playerInput.IsZoomIn);
        }

    }

    private void Update()
    {
        UpdateAimTarget();// 총의 조준지점은 계혹 업데이트해준다.

        float angle = playerCamera.transform.eulerAngles.x;//카메라가 보는 위아래 각도를 구함
        if (angle > 270f) angle -= 360f;
        angle = angle / -180f + 0.5f;
        playerAnimator.SetFloat(hashAngle, angle);// 에니메이터에 각도값을 보내어 총을 위아레로 움직이게함
        if (!playerInput.IsFire && !playerInput.IsZoomIn && aimState == AimState.FireReady) 
        {
            SetAimStateIdle();
        }

        UpdateUI();
    }
    public void SetAimStateIdle()
    {
        aimState = AimState.Idle;
        playerMovement.speed = playerMovement.walkSpeed;
        playerAnimator.SetLayerWeight(1, 0.3f);
        playerAnimator.SetBool(hashFireReady, false);
    }
    public void SetAimStateFireReady()
    {
        aimState = AimState.FireReady;
        playerMovement.speed = playerMovement.fireWalkSpeed;
        playerAnimator.SetLayerWeight(1, 1);
        playerAnimator.SetBool(hashFireReady, true);
    }
    public void ChooseGun(int weaponIndex, float damage)
    {
        EquipGun(allGuns[weaponIndex], damage);
    }
    public void EquipGun(Gun gunToEquip,float damage)
    {
        if (gun != null)
        {
            Destroy(gun.gameObject);
        }
        gun = Instantiate(gunToEquip, gunPivot.position, gunPivot.rotation) as Gun;
        gun.transform.parent = gunPivot;
        if(damage != 0.0f)
        {
            gun.Setup(this,damage);
        }
        
    }
    public void Shoot()
    {
        //플레이어가 총쏘지 않는 동안에는 카메라와 플레이어가 보는 방향이 달라도되지만 
        //총을쏠때는 플레이어와 카메라방향이 일치해야한 다 이부분을 ㅊ크하기위하여 사용한다.
        if (aimState == AimState.Idle)
        {
            if (isLinedUp)
            { //조준점과 캐릭터가 정렬될 경우 
                SetAimStateFireReady();
            }
        }
        else if (aimState == AimState.FireReady)//발사중인 상태인경우
        {
            //정면에 충분한 공간을 확보하는지 체크함
           
            if (isEnoughDistance)
            {
                if (gun.Fire(aimPoint))//발사를 실행함과 동시에 발사가 성공하는 것을 2가지동작을한다.
                {
                    playerAnimator.SetTrigger(hashShoot);
                }
            }
            else
            {
                SetAimStateIdle();
            }
        }
    }

    public void Reload()
    {
        if (gun.Reload())
        {
            playerAnimator.SetTrigger(hashReload);
        }
    }
    //카메라 정중앙을 기준으로 광선을 쏜다음 이후 광선을 쏜지점을 에임 타깃으로 지정
    //실제로 플레이어가 총을 쏜경우 해당부분에 닿는지 계산 갱신한다.
    private void UpdateAimTarget()
    {
        RaycastHit hit;
        //ViewPortToRay 지정한 지점에서 레이저를 발사한다.
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit, gun.fireDistance, ~excludeTarget))
        {
            aimPoint = hit.point;//레이저가 충돌한 부분을 조준지점으로 설정한다.
            //총발사와 벽사이에 끼어드는 충돌대상이 있으면
            if (Physics.Linecast(gun.fireTransform.position, hit.point, out hit, ~excludeTarget))
            {
                aimPoint = hit.point;
            }
        }
        else
        {
            aimPoint = playerCamera.transform.position + playerCamera.transform.forward * gun.fireDistance;
        }
    }

    private void UpdateUI()
    {
        if (gun == null || UIManager.Instance == null) return;

        UIPlayerInfo.Instance.SetAmmoText(gun.magAmmo, gun.ammoRemain);//남은 탄약수 갱신

        UIAim.Instance.ActiveCrosshair(isEnoughDistance);//크로스 해어 활성화
        UIAim.Instance.SetCrossHairPosition(aimPoint);//총알이 맞게되는 지점을 보내줘 조준점 이동하게함
    }
    //총을 쥐는 것을 다룸
    private void OnAnimatorIK(int layerIndex)
    {
        if (gun == null || gun.state == Gun.State.Reloading)
            return;

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, gun.leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, gun.leftHandMount.rotation);
       
    }
    //조준 시작
    //int i = 0;
    private void ZoomIn()
    {
        if (zoomFieldOfView > zoomInFieldOfView && Time.time >= lastZoomTime + waitingTimeForZoom)
        {
            //부드럽게 하기
            forrowCam.m_Lens.FieldOfView = (zoomFieldOfView -= 2f);
            forrowCamCinemachineComposerGetRig0.m_ScreenY = (zoomTopScreenY += 0.01f);
            forrowCamCinemachineComposerGetRig1.m_ScreenY = (zoomMidScreenY += 0.01f);
            forrowCamCinemachineComposerGetRig2.m_ScreenY = (zoomBotScreenY += 0.01f);
            forrowCamCinemachineComposerGetRig0.m_ScreenX = (zoomScreenX -= 0.01f);
            forrowCamCinemachineComposerGetRig1.m_ScreenX = (zoomScreenX);
            forrowCamCinemachineComposerGetRig2.m_ScreenX = (zoomScreenX);
            lastZoomTime = Time.time;
            // i++;
            // Debug.Log(i);
        }
        if (zoomFieldOfView <= zoomInFieldOfView + 0.1f)
            ZoomInEnd();
    }
    private void ZoomInEnd()
    {
        UIAim.Instance.crosshair.UseCrosshair(true);
        forrowCam.m_Lens.FieldOfView = zoomInFieldOfView;
        forrowCamCinemachineComposerGetRig0.m_ScreenY = zoomInTopScreenY;
        forrowCamCinemachineComposerGetRig1.m_ScreenY = zoomInMidScreenY;
        forrowCamCinemachineComposerGetRig2.m_ScreenY = zoomInBotScreenY;
        forrowCamCinemachineComposerGetRig0.m_ScreenX = zoomInScreenX;
        forrowCamCinemachineComposerGetRig1.m_ScreenX = zoomInScreenX;
        forrowCamCinemachineComposerGetRig2.m_ScreenX = zoomInScreenX;
        zoomFieldOfView = zoomInFieldOfView;
        zoomTopScreenY = zoomInTopScreenY;
        zoomMidScreenY = zoomInMidScreenY;
        zoomBotScreenY = zoomInBotScreenY;
        zoomScreenX = zoomInScreenX;
       
        //lateUpdateFollow.ZoomInFollow();
        //gun.ZoomInFollow();
        isZoomIn = true;
        if (aimState == AimState.Idle)
        {
            SetAimStateFireReady();
        }
    }
    //조준 끝
    private void ZoomOut()
    {
        if (zoomFieldOfView <= zoomOutFieldOfView && Time.time >= lastZoomTime + waitingTimeForZoom)
        {
            forrowCam.m_Lens.FieldOfView = (zoomFieldOfView += 2f);
            forrowCamCinemachineComposerGetRig0.m_ScreenY = (zoomTopScreenY -= 0.01f);
            forrowCamCinemachineComposerGetRig1.m_ScreenY = (zoomMidScreenY -= 0.01f);
            forrowCamCinemachineComposerGetRig2.m_ScreenY = (zoomBotScreenY -= 0.01f);
            forrowCamCinemachineComposerGetRig0.m_ScreenX = (zoomScreenX += 0.01f);
            forrowCamCinemachineComposerGetRig1.m_ScreenX = (zoomScreenX);
            forrowCamCinemachineComposerGetRig2.m_ScreenX = (zoomScreenX);
            lastZoomTime = Time.time;
        }
        if (zoomFieldOfView >= zoomOutFieldOfView - 0.1f)
            ZoomOutEnd();
    }
    private void ZoomOutEnd()
    {
        UIAim.Instance.crosshair.UseCrosshair(false);
        Debug.Log("줌아웃");
        forrowCam.m_Lens.FieldOfView = zoomOutFieldOfView;
        forrowCamCinemachineComposerGetRig0.m_ScreenY = zoomOutTopScreenY;
        forrowCamCinemachineComposerGetRig1.m_ScreenY = zoomOutMidScreenY;
        forrowCamCinemachineComposerGetRig2.m_ScreenY = zoomOutBotScreenY;
        forrowCamCinemachineComposerGetRig0.m_ScreenX = zoomOutScreenX;
        forrowCamCinemachineComposerGetRig1.m_ScreenX = zoomOutScreenX;
        forrowCamCinemachineComposerGetRig2.m_ScreenX = zoomOutScreenX;
        zoomFieldOfView = zoomOutFieldOfView;
        zoomTopScreenY = zoomOutTopScreenY;
        zoomMidScreenY = zoomOutMidScreenY;
        zoomBotScreenY = zoomOutBotScreenY;
        zoomScreenX = zoomOutScreenX;
        playerMovement.speed = playerMovement.walkSpeed;
        //lateUpdateFollow.ZoomOutFollow();
        //gun.ZoomOutFollow();
        isZoomIn = false;
    }

    private void ScopeZoomIn()
    {
        scopeCamera.SetActive(true);
        scopeImage.SetActive(true);
        UIAim.Instance.crosshair.UseCrosshair(false);
    }

    private void ScopeZoomOut(bool isCrosshair)
    {
        scopeCamera.SetActive(false);
        scopeImage.SetActive(false);
        UIAim.Instance.crosshair.UseCrosshair(isCrosshair);
    }
    //마우스 감도 조절하기 
    public void XAxisChange(float x )//설정 마우스 감도 조절용
    {
        currentXAxis = x;
    }
    public void YAxisChange(float y)//설정 마우스 감도 조절용
    {
        currentYAxis = y;
    }
    public void AxisActiveMenuChange()//메뉴 관련 마우스 감도
    {
        currentXAxis = forrowCam.m_XAxis.m_MaxSpeed;
        currentYAxis = forrowCam.m_YAxis.m_MaxSpeed;
        forrowCam.m_XAxis.m_MaxSpeed = 0;
        forrowCam.m_YAxis.m_MaxSpeed = 0;
    }
    public void AxisMenuInactiveChange()
    {
        forrowCam.m_XAxis.m_MaxSpeed = currentXAxis;
        forrowCam.m_YAxis.m_MaxSpeed = currentYAxis;
    }
    public void AxisStartSet(float x, float y) 
    {
        UIAim.Instance.SetAxisUI(x, y);
        currentXAxis = x;
        currentYAxis = y;
        forrowCam.m_XAxis.m_MaxSpeed = currentXAxis;
        forrowCam.m_YAxis.m_MaxSpeed = currentYAxis;
    }
}