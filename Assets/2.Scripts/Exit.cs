using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어 물리 설정 및 지상 이동 스크립트
/// </summary>
public class Exit : MonoBehaviour
{
    private PlayerCtrl playerCtrl;
    private Rigidbody2D rb;
    private float moveSpeed = 3f; // 이동 속도
    private float riseDistance = 3.4f; // 목표 거리
    private float delay = 1.5f; // 지상으로 바뀔때까지의 대기 시간
    private Vector3 exitSpawnPos = new Vector3(72.5f, -4.8f, 0f); // 지상 스폰 위치 
    private RigidbodyConstraints2D preConstrain; // 물리 이동 제어
    private float prePlayerMoveForce; // 기존 이동 속도
    private BoxCollider2D box; 
    private bool hasTriggered; // 재진입 차단


    void OnTriggerStay2D(Collider2D col)
    {
        // 트리거 내 D키 입력 시 로직 실행
        if(col.CompareTag("Player") && Input.GetKey(KeyCode.D) && !hasTriggered)
        {
            hasTriggered = true;
            playerCtrl = col.GetComponent<PlayerCtrl>();
            if(playerCtrl == null) { Debug.LogError("Null playerCtrl"); return; } 
            if (!playerCtrl.grounded) // 땅을 밟고 있지 않는 경우 return
            { 
                hasTriggered = false; 
                return; 
            }
            rb = col.GetComponent<Rigidbody2D>();
            box = col.GetComponent<BoxCollider2D>();

            // 현재 물리 상태 저장
            preConstrain = rb.constraints;
            prePlayerMoveForce = playerCtrl.moveForce;

            // 오른쪽으로 가기 위한 물리 설정 적용
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // 상하 회전 고정
            playerCtrl.moveForce = 0f; // 이동 제한
            box.isTrigger = true; // 충돌 무시

            rb.velocity = Vector2.right * moveSpeed;
            StartCoroutine(ExitUnderground(delay));
        }
    }

    // 지상 이동 및 물리 설정 복원하는 코루틴 
    IEnumerator ExitUnderground(float delaytime)
    { 
        yield return new WaitForSeconds(delaytime);

        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObj == null) { Debug.LogError("Null cameraObj"); yield break; }

        FollowCamera followCamera = cameraObj.GetComponent<FollowCamera>();
        if(followCamera == null) { Debug.LogError("Null FollowCamera"); yield break; } 
        
        followCamera.transform.position = new Vector3(72f, 0f, -10f); // 카메라 위치 업데이트
        playerCtrl.transform.position = exitSpawnPos; // 플레이어 위치 업데이트
        hasTriggered = false;
        rb.constraints = preConstrain; 
        while(playerCtrl.transform.position.y <= exitSpawnPos.y + riseDistance)
        {
            rb.velocity = Vector2.up * moveSpeed;
            yield return null;
        }
        // 물리 설정 복원
        playerCtrl.moveForce = prePlayerMoveForce;
        box.isTrigger = false;
    }
}
