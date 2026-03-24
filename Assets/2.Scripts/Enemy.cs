using UnityEngine;

/// <summary>
/// 굼바의 행동을 관리하는 스크립트
/// </summary>
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f; // 움직이는 속도
    private Rigidbody2D rb;
    private Animator aim;
    private float dir = 1f; // 이동 방향
    private bool alive; // 생명 플래그
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        aim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(alive)
        rb.velocity = new Vector2(transform.localScale.x * moveSpeed * dir, rb.velocity.y);    
    }

    // 방향 제어 함수 (외부 호출)
    public void Flip()
    {
        dir *= -1;
    }

    // 생명 시작 알림 함수 (외부 호출)
    public void StartLife()
    {
        alive = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Foot")) // 플레이어의 발
        {
            aim.SetTrigger("isDeath");
            // 실수로 충돌 감지용(플레이어)에 닿을 경우를 대비
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            alive = false;
        }
    }
    
    // 애니메이션 이벤트 호출
    public void OnisDeathAnimationEnd() 
    {
        Destroy(gameObject);
    }
}
