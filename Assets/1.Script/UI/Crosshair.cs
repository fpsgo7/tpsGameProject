﻿using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image aimPointReticle;//조준위치
    public Image hitPointReticle;//실제 맞는 위치

    public float smoothTime = 0.2f;//조준점의 스무딩 지연시간
    
    private Camera screenCamera;//메인카메라
    private RectTransform crossHairRectTransform;//총알이 실제 닿게되는 위치

    private Vector2 currentHitPointVelocity;//스무딩에 사용할 값의 변화량
    private Vector2 targetPoint;// 화면상 조준점이 목표로하는 위치

    private void Awake()
    {
        screenCamera = Camera.main;
        crossHairRectTransform = hitPointReticle.GetComponent<RectTransform>();
    }

    public void SetActiveCrosshair(bool active)
    {
        hitPointReticle.enabled = active;
        aimPointReticle.enabled = active;
    }

    public void UpdatePosition(Vector3 worldPoint)
    {
        targetPoint = screenCamera.WorldToScreenPoint(worldPoint);
    }

    private void Update()
    {
        if (!hitPointReticle.enabled)
            return;

        crossHairRectTransform.position = Vector2.SmoothDamp(crossHairRectTransform.position, targetPoint, ref currentHitPointVelocity, smoothTime);

    }
}