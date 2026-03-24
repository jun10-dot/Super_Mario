using UnityEngine;

/// <summary>
/// 코인 동작을 관리하는 스크립트입니다.
/// </summary>
public class Coin : MonoBehaviour
{
    private float riseSpeed = 3f; // 수직 상승 속도
    private float riseDistance = 2f; // 생성 위치로부터 상승할 최대 거리
    private Rigidbody2D rb;
    private Vector3 spawnPos; // 초기 생성 위치

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        spawnPos = transform.position; 
        rb.isKinematic = true; // 물리 엔진의 영향을 받지 않도록 설정 
    }

    void Update()
    {
        // 일정한 속도로 수직 상승
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        // 목표 Y축 도달 시 오브젝트 파괴
        if (transform.position.y >= spawnPos.y + riseDistance)
           Destroy(gameObject);
    }
}
