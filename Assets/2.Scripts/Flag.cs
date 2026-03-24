using UnityEngine;

/// <summary>
/// 깃발 하강 로직 스크립트
/// </summary>
public class Flag : MonoBehaviour
{
    [SerializeField] private GameEnding gEnding;
    private float fallSpeed = 12f; // 하강 속도
    private float targetY = -6.85f; // 하강 멈출 목표 Y축 
    void Update()
    {
        if(gEnding.currentTransform == null) return; // 플레이어 없으면 실행X
        DownFlag();
    }

    // 설정된 targetY 까지 깃발 하강시키는 함수
    void DownFlag()
    {
        if(transform.localPosition.y <= targetY) return;
        transform.localPosition -= Vector3.up * fallSpeed * Time.deltaTime;
    }
}
