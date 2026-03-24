using System.Collections;
using UnityEngine;

/// <summary>
/// 애니메이션과 물리제어로 게임 엔딩 과정을 거치는 스크립트
/// </summary>
public class GameEnding : MonoBehaviour
{
    [HideInInspector] public Transform currentTransform; // 마리오 객체
    private int fallSpeed = 10; // 낙하 속도
    private Rigidbody2D rb; 
    private Animator anim;
    private float targetY = - 6.85f; // 목표 Y축
    public bool isBlockCtrl; // 컨트롤 제어 플래그
    private int playerSpeed = 6; // 마리오 속도
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            isBlockCtrl = true;
            currentTransform = col.transform;
            rb = currentTransform.GetComponent<Rigidbody2D>();
            anim = currentTransform.GetComponent<Animator>();
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // 모든 움직임 차단
            anim.SetTrigger("Grab");
            StartCoroutine(DownPlayer());
        }
    }

    IEnumerator DownPlayer()
    {
        // 1초 대기시간 후 낙하
        yield return new WaitForSeconds(1f); 
        while(currentTransform.position.y > targetY)
        {
            currentTransform.position -= Vector3.up * fallSpeed * Time.deltaTime;
            yield return null;
        }
        // 목표 지점 도달 후 성 안으로 진입
        anim.SetTrigger("Idle");
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; // 상하 Y축 물리 해제  
        while(currentTransform != null)
        {
            currentTransform.position += Vector3.right * playerSpeed * Time.deltaTime;
            anim.SetFloat("Speed", 1); 
            yield return null;
        }
    }
}
