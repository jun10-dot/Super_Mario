using UnityEngine;

/// <summary>
/// 큰 마리오 피격 시 작은 마리오로 전환하는 스크립트
/// </summary>
public class BigPlayer : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private SmallPlayer smallPlayer; //작은마리오 객체 참조
    [SerializeField] private Transform hand; // 블록 충돌 감지용 손 위치
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // 활성화시 GameManager에 큰 마리오 정보 등록
    void OnEnable()
    {
        GameManager.instance.SetPlayerInfo(transform); 
        GameManager.instance.SetPlayerHandInfo(hand);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            anim.SetTrigger("Down"); //큰 마리오 -> 작은 마리오 (애니메이션)
        }
    }

    //애니메이션 이벤트 호출
    public void OnDamageEnd() 
    {
        Vector3 pos = gameObject.transform.position;
        smallPlayer.transform.position = pos; // 현재 위치로 업데이트
        gameObject.SetActive(false); //큰 마리오 비활성화
        smallPlayer.gameObject.SetActive(true); //작은 마리오 활성화
        smallPlayer.isbig = false;
    }
  
}
