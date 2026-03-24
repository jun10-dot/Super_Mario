using System.Collections;
using UnityEngine;

/// <summary>
/// 기본 마리오의 고유 기능(버섯 상호작용, 죽는 연출)을 처리하는 스크립트
/// </summary>
public class SmallPlayer : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    [HideInInspector] public bool isbig = false; // 큰 마리오인지 여부
    private bool hasTriggered; // 중복 처리 방지
    private float reboundDelay = 1.2f; // 튀어오르기 전 대기시간
    private float riseForce = 1000f; // 죽을 때 위로 튀어오르는 힘
    [SerializeField] private GameObject bigPlayer; // 큰 마리오 객체
    private PlayerCtrl playerCtrl;
    [SerializeField] private Transform hand; // 블록 충돌 감지용 손 위치
    private Rigidbody2D rb;
    private BoxCollider2D box;
    void Awake()
    {
        GameObject playerCtrlObj = GameObject.FindGameObjectWithTag("Player");
        if(playerCtrlObj == null) { Debug.LogError("Null playerCtrlObj"); return; }
        playerCtrl = playerCtrlObj.GetComponent<PlayerCtrl>();
        if(playerCtrl == null) { Debug.LogError("Null playerCtrl"); return; }

        anim= GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    // 활성화시 GameManager에 작은 마리오 정보 등록
    void OnEnable()
    {
        GameManager.instance.SetPlayerInfo(transform); 
        GameManager.instance.SetPlayerHandInfo(hand);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // 버섯 획득 시 큰 마리오로 변신
        if (col.CompareTag("Mushroom"))
        {
            Destroy(col.transform.parent.gameObject);
            anim.SetTrigger("Eat");
        }
        else if(col.CompareTag("Enemy") && !hasTriggered)
        {
            hasTriggered = true;
            DieMotion();
        }
    }

    // 애니메이션 이벤트 호출
    // 작은 마리오의 위치를 큰 마리오 위치에 대입
    public void OnEatAnimationEnd()
    {
        Vector3 pos = gameObject.transform.position;
        bigPlayer.transform.position = pos;
        gameObject.SetActive(false);
        bigPlayer.SetActive(true);
        isbig = true;
    }

    // 죽을 때 물리 제어로 이동 및 점프를 제한하는 함수
    void DieMotion()
    {
        anim.SetTrigger("Die");
        playerCtrl.groundCheck.gameObject.SetActive(false); // 적의 의도치않은 죽음판정 회피
        playerCtrl.groundCheck = null; // 점프 입력 제한
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 물리 제한
        
        StartCoroutine(Died(reboundDelay));
    }

    // 죽는 애니메이션 연출 후 하늘로 상승
    // 그 후 중력에 의해 땅을 통과하며 밑으로 낙하 
    IEnumerator Died(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; // Y축만 해제
        rb.AddForce(Vector2.up * riseForce); 
        box.isTrigger = true;
    }

}
