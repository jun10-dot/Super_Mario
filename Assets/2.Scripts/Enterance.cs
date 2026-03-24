using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어 물리 설정 및 지하 이동 스크립트
/// </summary>
public class Enterance : MonoBehaviour
{
    private Vector3 playerPos = new Vector3(61f, -15f, 0f);
    private Vector3 cameraPos = new Vector3(72f, -26f, -10f);
    private float fallSpeed = 2f; // 내려가는 속도
    private float gravity = 0f; // 중력 무시
    private float delay = 1.5f; // 지하로 바뀔때까지의 대기 시간
    private PlayerCtrl playerCtrl;
    private Rigidbody2D rb;

    private RigidbodyConstraints2D originalConstraints; // 물리 이동 제어 
    private float preGravity; // 기존 중력 값
    private bool preIsTrigger; // 기존 트리거 설정
    private float preJumpForce; // 기존 점프력
    private bool hasTriggered; // 재진입 차단

    void OnTriggerStay2D(Collider2D col)
    {
        // 트리거 내 S키 입력 시 로직 실행
        if (col.CompareTag("Player") && Input.GetKey(KeyCode.S) && !hasTriggered)
        {
            hasTriggered = true;
            playerCtrl = col.GetComponent<PlayerCtrl>();
            if(playerCtrl == null) { Debug.LogError("Null playerCtrl"); return; }
            rb = col.GetComponent<Rigidbody2D>();
            // 현재 물리 상태 저장
            preIsTrigger = playerCtrl.box.isTrigger;
            preJumpForce = playerCtrl.jumpForce;
            originalConstraints = rb.constraints;
            preGravity = rb.gravityScale;

            // 내려가기 위한 물리 설정 적용
            playerCtrl.box.isTrigger = true; // 충돌 무시
            playerCtrl.jumpForce = 0f; // 점프 무시
            rb.gravityScale = gravity; // 중력 무시
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; // 좌우 회전 고정
            rb.velocity = Vector2.down * fallSpeed;
            StartCoroutine(EnterUnderground(delay));
        }      
    }

    // 지하 이동 및 물리 설정 복원하는 코루틴 
    IEnumerator EnterUnderground(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (playerCtrl == null) {Debug.LogError("Null player"); yield break; }
        playerCtrl.transform.position = playerPos; // 플레이어 위치 업데이트

        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObj == null) { Debug.LogError("Null cameraObj"); yield break; }

        FollowCamera followCamera = cameraObj.GetComponent<FollowCamera>();
        if(followCamera == null) { Debug.LogError("Null FollowCamera"); yield break; } 
        followCamera.transform.position = cameraPos; // 카메라 위치 업데이트

        // 물리 설정 복원
        rb.constraints = originalConstraints;
        rb.gravityScale = preGravity;
        playerCtrl.box.isTrigger = preIsTrigger;
        playerCtrl.jumpForce = preJumpForce;

        hasTriggered = false;
    }
}
