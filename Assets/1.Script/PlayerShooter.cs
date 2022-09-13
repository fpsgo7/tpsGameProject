using UnityEngine;


public class PlayerShooter : MonoBehaviour
{
    //상태값 변수
    public enum AimState
    {
        Idle,
        HipFire//발사상태
    }
    //파라미터형 변수
    public AimState aimState { get; private set; }
    //사용할 오브젝트
    public Gun gun;// 총 스크립트와 연결
    public LayerMask excludeTarget;//조준에서 제외할 대상
    
    private PlayerInput playerInput;//플레이어 입력 스크립트와 연결
    private Animator playerAnimator;//플레이어 에니메이션
    private Camera playerCamera;//플레이어 카메러

    private float waitingTimeForReleasingAim = 2.5f;//총 조준후 다시 풀어지는 시간
    private float lastFireInputTime; //마지막 발사 시간
    
    private Vector3 aimPoint;//실제 조준대상 tps 기에 사용한다. 실제 조준점이 무조건 정중앙이 아니라서이다.
    private bool linedUp => !(Mathf.Abs( playerCamera.transform.eulerAngles.y - transform.eulerAngles.y) > 1f);//플레이어가 바라보는 각도와 실제 조준 각도를 너무큰치 체크해준다.
    //정면에 사격할수 있는지 적정거리가 되는지 체크하는 변수 (플레이어 케릭터 위치 + Vector3.up *  총의 발사 위치의 y축, 발사 포지션, 사격제외대상 레이어)
    private bool hasEnoughDistance => !Physics.Linecast(transform.position + Vector3.up * gun.fireTransform.position.y,gun.fireTransform.position, ~excludeTarget);
    
    void Awake()
    {
        //플레이어가 자기자신을 쏘는 상황을 방지하기위하여 자기자신의 레이어를 추가한다.
        if (excludeTarget != (excludeTarget | (1 << gameObject.layer)))
        {
            excludeTarget |= 1 << gameObject.layer;
        }

    }

    private void Start()
    {
        playerCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        aimState = AimState.Idle;
        gun.gameObject.SetActive(true);
        gun.Setup(this);
    }

    private void OnDisable()
    {
        aimState = AimState.Idle;
        gun.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (playerInput.fire)
        {
            lastFireInputTime = Time.time;
            Shoot();
        }
        else if (playerInput.reload)
        {
            Reload();
        }
    }

    private void Update()
    {
        UpdateAimTarget();// 총의 조준지점은 계혹 업데이트해준다.
        
        var angle = playerCamera.transform.eulerAngles.x;//카메라가 보는 위아래 각도를 구함
        if (angle > 270f) angle -= 360f;
        angle = angle / -180f + 0.5f;
        playerAnimator.SetFloat("Angle", angle);// 에니메이터에 각도값을 보내어 총을 위아레로 움직이게함
        //발사버튼을 안누른시간이 지정된시간보다 오래걸리면 실행됨
        if(!playerInput.fire && Time.time >= lastFireInputTime + waitingTimeForReleasingAim)
        {
            aimState = AimState.Idle;
        }

        UpdateUI();
    }

    public void Shoot()
    {
        //플레이어가 총쏘지 않는 동안에는 카메라와 플레이어가 보는 방향이 달라도되지만 
        //총을쏠때는 플레이어와 카메라방향이 일치해야한 다 이부분을 ㅊ크하기위하여 사용한다.
        if(aimState == AimState.Idle)
        {

            if (linedUp)
            { //조준점과 캐릭터가 정렬될 경우 
                aimState = AimState.HipFire;//사격 상태로 바꾼다.
            }
           
        }
        else if (aimState == AimState.HipFire)//발사중인 상태인경우
        {
            //정면에 충분한 공간을 확보하는지 체크함
            if (hasEnoughDistance)
            {
                if (gun.Fire(aimPoint))//발사를 실행함과 동시에 발사가 성공하는 것을 2가지동작을한다.
                {
                    playerAnimator.SetTrigger("Shoot");
                }
            }
            else
            {
                aimState = AimState.Idle;
            }
        }
    }

    public void Reload()
    {
        if (gun.Reload())
        {
            playerAnimator.SetTrigger("Reload");
        }
    }
    //카메라 정중앙을 기준으로 광선을 쏜다음 이후 광선을 쏜지점을 에임 타깃으로 지정
    //실제로 플레이어가 총을 쏜경우 해당부분에 닿는지 계산 갱신한다.
    private void UpdateAimTarget()
    {
        RaycastHit hit;
        //ViewPortToRay 지정한 지점에서 레이저를 발사한다.
        var ray = playerCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));

        if(Physics.Raycast(ray, out hit, gun.fireDistance, ~excludeTarget))
        {
            aimPoint = hit.point;//레이저가 충돌한 부분을 조준지점으로 설정한다.
            //총발사와 벽사이에 끼어드는 충돌대상이 있으면
            if(Physics.Linecast(gun.fireTransform.position,hit.point,out hit, ~excludeTarget))
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
        
        UIManager.Instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);//남은 탄약수 갱신
        
        UIManager.Instance.SetActiveCrosshair(hasEnoughDistance);//크로스 해어 활성화
        UIManager.Instance.UpdateCrossHairPosition(aimPoint);//총알이 맞게되는 지점을 보내줘 조준점 이동하게함
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
}