using UnityEngine;
/// <summary>
/// 회전 스크립트이다.
/// 미사용 스크립트
/// </summary>
public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}