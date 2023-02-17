﻿using UnityEngine;
using Cinemachine;
using System.Collections;
/// <summary>
/// 플레이어의 메인카메라 담당
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    private PlayerShooter playerShooter;
    // 줌인 카메라를 위한 시네머신 카메라 변수들
    public CinemachineFreeLook forrowCam;// 줌인 카메라
    private CinemachineComposer forrowCamCinemachineComposerGetRig0;
    private CinemachineComposer forrowCamCinemachineComposerGetRig1;
    private CinemachineComposer forrowCamCinemachineComposerGetRig2;
    private float setFov;
    private int fovChangeSpeed;
    //줌인 줌아웃 float 변수
    public const float zoomOutFieldOfView = 60f;
    private const float zoomOutScreenX = 0.45f;//0.4 전에 사용하던값
    private const float zoomOutTopScreenY = 0.55f;//0.6
    private const float zoomOutMidScreenY = 0.55f;
    private const float zoomOutBotScreenY = 0.55f;
    private const float zoomInFieldOfView = 40f;
    private const float zoomInScreenX = 0.35f;
    private const float zoomInTopScreenY = 0.65f;
    private const float zoomInMidScreenY = 0.65f;
    private const float zoomInBotScreenY = 0.65f;
    private const float runFieldOfView = 80f;
    // 조준경 조준을 위한 스크립트
    public GameObject scopeCamera;
    public GameObject scopeImage;

    public enum FovValues
    {
        ZOOMOUT,
        ZOOMIN,
        RUN
    }

    private void Awake()
    {
        playerShooter = GetComponent<PlayerShooter>();

        forrowCamCinemachineComposerGetRig0 = forrowCam.GetRig(0).GetCinemachineComponent<CinemachineComposer>();
        forrowCamCinemachineComposerGetRig1 = forrowCam.GetRig(1).GetCinemachineComponent<CinemachineComposer>();
        forrowCamCinemachineComposerGetRig2 = forrowCam.GetRig(2).GetCinemachineComponent<CinemachineComposer>();
        //카메라 값 초기화
        forrowCam.m_Lens.FieldOfView = zoomOutFieldOfView;
        forrowCamCinemachineComposerGetRig0.m_ScreenY = zoomOutTopScreenY;
        forrowCamCinemachineComposerGetRig1.m_ScreenY = zoomOutMidScreenY;
        forrowCamCinemachineComposerGetRig2.m_ScreenY = zoomOutBotScreenY;
        forrowCamCinemachineComposerGetRig0.m_ScreenX = zoomOutScreenX;
        forrowCamCinemachineComposerGetRig1.m_ScreenX = zoomOutScreenX;
        forrowCamCinemachineComposerGetRig2.m_ScreenX = zoomOutScreenX;
        // fov값 초기화
        setFov = zoomOutFieldOfView;
    }

    private void Update()
    {
        forrowCam.m_Lens.FieldOfView = Mathf.Lerp(forrowCam.m_Lens.FieldOfView, setFov , Time.deltaTime * fovChangeSpeed);
    }

    //조준 시작
    //int i = 0;
    public void ZoomIn()
    {
        if (playerShooter.aimState == PlayerShooter.AimState.Idle)
        {
            playerShooter.SetAimStateFireReady();
        }
        SetCameraFov(FovValues.ZOOMIN ,10);
        forrowCamCinemachineComposerGetRig0.m_ScreenY = Mathf.Lerp(forrowCamCinemachineComposerGetRig0.m_ScreenY, zoomInTopScreenY, Time.deltaTime * 5);
        forrowCamCinemachineComposerGetRig1.m_ScreenY = Mathf.Lerp(forrowCamCinemachineComposerGetRig1.m_ScreenY, zoomInMidScreenY, Time.deltaTime * 5);
        forrowCamCinemachineComposerGetRig2.m_ScreenY = Mathf.Lerp(forrowCamCinemachineComposerGetRig2.m_ScreenY, zoomInBotScreenY, Time.deltaTime * 5);
        forrowCamCinemachineComposerGetRig0.m_ScreenX = Mathf.Lerp(forrowCamCinemachineComposerGetRig0.m_ScreenX, zoomInScreenX, Time.deltaTime * 5);
        forrowCamCinemachineComposerGetRig1.m_ScreenX = Mathf.Lerp(forrowCamCinemachineComposerGetRig1.m_ScreenX, zoomInScreenX, Time.deltaTime * 5);
        forrowCamCinemachineComposerGetRig2.m_ScreenX = Mathf.Lerp(forrowCamCinemachineComposerGetRig2.m_ScreenX, zoomInScreenX, Time.deltaTime * 5);

        if (forrowCam.m_Lens.FieldOfView <= zoomInFieldOfView + 0.1f)
        {
            forrowCam.m_Lens.FieldOfView = zoomInFieldOfView;
            forrowCamCinemachineComposerGetRig0.m_ScreenY = zoomInTopScreenY;
            forrowCamCinemachineComposerGetRig1.m_ScreenY = zoomInMidScreenY;
            forrowCamCinemachineComposerGetRig2.m_ScreenY = zoomInBotScreenY;
            forrowCamCinemachineComposerGetRig0.m_ScreenX = zoomInScreenX;
            forrowCamCinemachineComposerGetRig1.m_ScreenX = zoomInScreenX;
            forrowCamCinemachineComposerGetRig2.m_ScreenX = zoomInScreenX;
            playerShooter.SetisZoomIn(true);
          
        }

    }
    //조준 끝
    public void ZoomOut()
    {
        playerShooter.SetisZoomIn(false);
        SetCameraFov(FovValues.ZOOMOUT,10);
        forrowCamCinemachineComposerGetRig0.m_ScreenY = Mathf.Lerp(forrowCamCinemachineComposerGetRig0.m_ScreenY, zoomOutTopScreenY, Time.deltaTime * 10);
        forrowCamCinemachineComposerGetRig1.m_ScreenY = Mathf.Lerp(forrowCamCinemachineComposerGetRig1.m_ScreenY, zoomOutMidScreenY, Time.deltaTime * 10);
        forrowCamCinemachineComposerGetRig2.m_ScreenY = Mathf.Lerp(forrowCamCinemachineComposerGetRig2.m_ScreenY, zoomOutBotScreenY, Time.deltaTime * 10);
        forrowCamCinemachineComposerGetRig0.m_ScreenX = Mathf.Lerp(forrowCamCinemachineComposerGetRig0.m_ScreenX, zoomOutScreenX, Time.deltaTime * 10);
        forrowCamCinemachineComposerGetRig1.m_ScreenX = Mathf.Lerp(forrowCamCinemachineComposerGetRig0.m_ScreenX, zoomOutScreenX, Time.deltaTime * 10);
        forrowCamCinemachineComposerGetRig2.m_ScreenX = Mathf.Lerp(forrowCamCinemachineComposerGetRig0.m_ScreenX, zoomOutScreenX, Time.deltaTime * 10);

        if (forrowCam.m_Lens.FieldOfView >= zoomOutFieldOfView - 0.1f)
        {
            forrowCam.m_Lens.FieldOfView = zoomOutFieldOfView;
            forrowCamCinemachineComposerGetRig0.m_ScreenY = zoomOutTopScreenY;
            forrowCamCinemachineComposerGetRig1.m_ScreenY = zoomOutMidScreenY;
            forrowCamCinemachineComposerGetRig2.m_ScreenY = zoomOutBotScreenY;
            forrowCamCinemachineComposerGetRig0.m_ScreenX = zoomOutScreenX;
            forrowCamCinemachineComposerGetRig1.m_ScreenX = zoomOutScreenX;
            forrowCamCinemachineComposerGetRig2.m_ScreenX = zoomOutScreenX;
        }
    }
    // 조준경 조준
    public void ScopeZoomIn()
    {
        scopeCamera.SetActive(true);
        scopeImage.SetActive(true);
        UIAim.Instance.crosshair.UseCrosshair(false);
    }

    public void ScopeZoomOut(bool isCrosshair)
    {
        scopeCamera.SetActive(false);
        scopeImage.SetActive(false);
        UIAim.Instance.crosshair.UseCrosshair(isCrosshair);
    }
    //Fov 설정
    public void SetCameraFov(FovValues fovValues, int fovChangeSpeed)
    {
        if(fovValues == FovValues.ZOOMOUT)
            setFov = zoomOutFieldOfView;
        if (fovValues == FovValues.ZOOMIN)
            setFov = zoomInFieldOfView;
        if (fovValues == FovValues.RUN)
            setFov = runFieldOfView;
        this.fovChangeSpeed = fovChangeSpeed;
    }
}