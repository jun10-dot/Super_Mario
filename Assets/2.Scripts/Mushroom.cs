using UnityEngine;

/// <summary>
/// 버섯의 물리 제어 및 이동을 처리하는 스크립트
/// </summary>
public class Mushroom : MonoBehaviour
{
    public float riseDistance = 1.0f; // 생성 위치로부터 상승할 최대 거리
    public float riseSpeed = 2.5f; // 상승 속도    
    public float moveSpeed = 5.0f; // 이동 속도

    private Rigidbody2D rb;
   
    private Vector3 spawnPos; // 생성 위치
    public bool isRising = true; // 현재 상승중인지 여부

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPos = transform.position; 
        rb.isKinematic = true; // 물리 무시
    }

    void FixedUpdate()
    {
        if (!isRising) return;

        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        if (transform.position.y >= spawnPos.y + riseDistance)
        {
            isRising = false;
            StartMove();
        }  
    }

    void StartMove()
    {
        rb.isKinematic = false;  // 물리 적용
        rb.velocity = Vector2.right * moveSpeed;
    }

    // 방향 전환하는 함수
    public void Flip()
    {
        Vector3 msScale = transform.localScale;
        msScale.x *= -1;
        transform.localScale = msScale;

        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
    }
}
