using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    //PlayerHealth, PlayerMovement, PlayerShooter을 총괄 관리하기위한 스크립트이다.
    //생명 리스폰 아이템 먹는 기능 도포함

    private Animator animator;//애니메이터
    public AudioClip itemPickupClip;//아이템 먹는 소리
    public int lifeRemains = 3;//남은 생명수
    private AudioSource playerAudioPlayer;//플레이어의 오디오 컴포넌트
    private PlayerHealth playerHealth;//플레이어 체력
    private PlayerMovement playerMovement;//플레이어 움직임
    private PlayerShooter playerShooter;//플레이어 슈터
    private FireGrenade fireGrenade;//플레이어 수류탄 발사

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerHealth.OnDeath += HandleDeath;// 플레이어 가 죽으면 핸들 데스만큼 값증가

        UIManager.Instance.SetLifeText(lifeRemains);
        Cursor.visible = false;// 게임이 시작되면 마우스 커서를 비활성화 한다.
    }

    private void HandleDeath()
    {
        // 사망하면 플레이어의 기능을 비활성화함
        playerMovement.enabled = false;
        playerShooter.enabled = false;

        if (lifeRemains > 0)
        {
            lifeRemains--;
            UIManager.Instance.SetLifeText(lifeRemains);
            Invoke("Respawn", 3f);
        }
        else
        {
            GameManager.Instance.EndGame();
        }

        Cursor.visible = true;
    }

    public void Respawn()
    {
        //게임 오브젝트를 비활성화 활성화 함으로써
        // 각 스크립트의 disabled 함수 실행후 enabled 함수를 실행하여
        //값을 초기화한다.
        gameObject.SetActive(false);
        transform.position = Utility.GetRandomPointOnNavMesh(transform.position, 30f, NavMesh.AllAreas);

        playerMovement.enabled = true;
        playerShooter.enabled = true;
        gameObject.SetActive(true);

        playerShooter.gun.ammoRemain = 120;

        Cursor.visible = false;
    }

}