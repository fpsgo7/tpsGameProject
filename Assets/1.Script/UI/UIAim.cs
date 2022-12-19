using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAim : MonoBehaviour
{
    //싱글톤 방식 사용
    private static UIAim instance;

    public static UIAim Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIAim>();

            return instance;
        }
    }

    //각 오브젝트들을 유니티 인스팩터 상에서 연결하기위하여 SerializeField 사용
    [SerializeField] private Slider xAxisSlider;
    [SerializeField] private Slider yAxisSlider;
    [SerializeField] private Text xAxisText;
    [SerializeField] private Text yAxisText;
    public Crosshair crosshair;
    [HideInInspector] public float xAxis;
    [HideInInspector] public float yAxis;
    //크로스해어 업데이트
    public void SetCrossHairPosition(Vector3 worldPosition)
    {
        crosshair.UpdatePosition(worldPosition);
    }
    //크로스헤어 보여주기 관련 
    public void ActiveCrosshair(bool isActive)
    {
        crosshair.SetActiveCrosshair(isActive);
    }
    //조준감도 조절 
    public void ChangeXAxisSlider()
    {
        xAxis = xAxisSlider.value;
        xAxisText.text = xAxis.ToString();
    }
    public void ChangeYAxisSlider()
    {
        yAxis = yAxisSlider.value;
        yAxisText.text = yAxis.ToString();
    }
    //게임 시작할때 조준감도 UI 에 적용하여 보여주기
    public void SetAxisUI(float x, float y)
    {
        xAxisSlider.value = x;
        xAxisText.text = x.ToString();
        yAxisSlider.value = y;
        yAxisText.text = y.ToString();
    }
}
