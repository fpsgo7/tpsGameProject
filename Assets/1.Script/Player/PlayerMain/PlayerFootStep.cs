using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 발자국 소리를 출력
/// </summary>
public class PlayerFootStep : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;
    private Animator myAnimator;// 나의 애니메이터
    private Transform leftFoot, rightFoot;// 발위치
    private float dist;//거리값
    private bool grounded;// 땅에 붙어있는 불값
    private int[] soundIndexess= new int[4];// 사운드 인덱스 접근값
    private int soundIndex;
    // 왼발 오른발 enum 값
    public enum Foot
    {
        LEFT,
        RIGHT,
    }
    private Foot step = Foot.LEFT;// 이넘형 변수 왼발로 시작
    private float oldDist, maxDist = 0;// 이동거리 체크에 사용하기 위한 변수 오래된거리와 최대거리

    private void Awake()
    {
        // 캐싱하며 값 연결하기
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        myAnimator = GetComponent<Animator>();
        leftFoot = myAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);// 왼발 뼈
        rightFoot = myAnimator.GetBoneTransform(HumanBodyBones.RightFoot);// 오른발뼈 
        soundIndexess[0] = (int)SoundList.FootStep1;
        soundIndexess[1] = (int)SoundList.FootStep2;
        soundIndexess[2] = (int)SoundList.FootStep3;
        soundIndexess[3] = (int)SoundList.FootStep4;
    }
    // 발자국 소리 제생
    private void PlayFootStep()
    {
        if (oldDist < maxDist)// 마지막 거리가 최대거리보다 작은 경우  return
        {// 아직 발이 땅바닥에 닿지 않음
            return;
        }
        oldDist = maxDist = 0;// 땅바닥에 닿은경우 값 초기화
        SoundToolManager.Instance.PlayOneShotSound(soundIndexess[soundIndex], transform.position, 0.5f);
        soundIndex++;
        if (soundIndex == 3)
            soundIndex = 0;
    }

    private void Update()
    {
        if (playerMovement.currentSpeed <= 3.0f) 
            return;
        grounded = !playerMovement.isJumpState;
        float factor = 0.15f;// 임의 값
        if (grounded )//만약 땅에 붙어 있고 움직이고 있다면
        {
            oldDist = maxDist;// oldDist 거리 업데이트
            switch (step)
            {
                case Foot.LEFT:// 왼발일경우
                    dist = leftFoot.position.y - transform.position.y;// 왼발에서 플레이어 포지션 높이까지 길이를 구한다.
                    maxDist = dist > maxDist ? dist : maxDist;// maxdist 에 ,방금 구한 길이값을 넣은후 maxDist보다 크다면 dist,
                    if (dist <= factor)//발이 특정 높이만큼 내려왔다면
                    {
                        // 해당 문장을 실행하고 오른발로 교체한다.
                        PlayFootStep();
                        step = Foot.RIGHT;
                    }
                    break;
                case Foot.RIGHT:
                    dist = rightFoot.position.y - transform.position.y;
                    maxDist = dist > maxDist ? dist : maxDist;
                    if (dist <= factor)//발이 특정 높이만큼 내려왔다면
                    {
                        PlayFootStep();
                        step = Foot.LEFT;
                    }
                    break;
            }
        }
    }
}