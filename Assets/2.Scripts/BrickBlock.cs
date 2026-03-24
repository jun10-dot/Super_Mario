using System.Collections;
using UnityEngine;

/// <summary>
/// 상호작용 시 벽돌의 고유 기능을 처리하는 스크립트 
/// </summary>
public class BrickBlock : MonoBehaviour
{
    [SerializeField] private SmallPlayer sPlayer; //작은 마리오 객체 참조
    [SerializeField] private GameObject particleBlock; // 벽돌 파괴 시 파티클
    private Transform playerHand; // 마리오의 손 위치 정보
    private Transform block; // 거리 기반 충돌 감지용
    private bool rock; // 중복 실행 방지 
    private Vector3 currPos; // 초기 위치
    private float maxHight = 0.8f; // 블록이 솟아오를 최대 높이
    private float verticalSpeed = 0.1f; // 수직 상하 속도
    private float triggerDistance = 0.5f; // 닿았다고 간주할 거리
    
    void Awake()
    {
        block = transform.GetChild(0);        
        maxHight += transform.position.y;
        currPos = transform.position;
    }

    void Update()
    {
        if (playerHand != GameManager.instance.GetPlayerHandInfo())
            playerHand = GameManager.instance.GetPlayerHandInfo(); // 마리오 변신 시 객체 참조

        if (playerHand == null) return;

        // 마리오 손과 거리 판정
        bool isHit = Vector2.Distance(playerHand.transform.position, block.position) < triggerDistance;

        if (!isHit || rock) return;

        BrickHit();
    }

    // 충돌 시 로직 처리
    void BrickHit()
    {
        if (!sPlayer.isbig) //작은 마리오
        {
            rock = true;
            StartCoroutine(BlockUp());
        }

        else //큰 마리오
        {
            Instantiate(particleBlock, transform.position, Quaternion.identity);  
            gameObject.SetActive(false); 
        }
    }

    // 위로 솟아오르는 연출
    IEnumerator BlockUp()
    {
        Vector3 pos = transform.position;
        while (maxHight > transform.position.y)
        {
            pos.y += verticalSpeed;
            transform.position = pos;
            yield return null;
        }
        StartCoroutine(BlockDown());
    }

    // 원래 위치로 내려앉는 연출
    IEnumerator BlockDown()
    {
        Vector3 targetPos = transform.position;
        while (targetPos.y > currPos.y)
        {
            targetPos.y -= verticalSpeed;
            transform.position = targetPos;
            yield return null;
        }
        transform.position = currPos;
        yield return new WaitForSeconds(0.5f); // 연속 실행 방지
        rock = false;
    }
}
