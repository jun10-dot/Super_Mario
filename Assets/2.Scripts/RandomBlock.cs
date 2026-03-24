using System.Collections;
using UnityEngine;

/// <summary>
/// 스프라이트 애니메이션, 블록 반동, 아이템 생성을 처리하는 스크립트  
/// </summary>
/// 
public class RandomBlock : MonoBehaviour
{
    public Sprite[] changeImg; // 랜덤 블록 이미지 배열
    public Sprite emptySprite; // 상호작용 후 스프라이트 
    private SpriteRenderer spriteimg;
    private float animTime = 0f; // 누적 시간
    private int spImgCount = 0; // 현재 스프라이트 배열 인덱스
    private float triggerDistance = 0.5f; // 닿았다고 간주할 거리
    private Transform children; // 거리 기반 충돌 감지용
    private Vector3 currPos; // 초기 위치
    private float maxHight = 0.8f; // 블록이 솟아오를 최대 높이
    private bool rock; // 중복 실행 방지
    private float changeTime = 0.25f;
    public GameObject Item; // 랜덤 블록 아이템 (동전, 버섯)
    private Transform playerHand; // 마리오 자식 오브젝트 (손)
    void Awake()
    {
        spriteimg = GetComponent<SpriteRenderer>();
        children = transform.GetChild(0);
        currPos = transform.position;
        maxHight += transform.position.y;
    }

    void Update()
    {
        if (rock) return;

        SpriteAnimation();
        Rebound();
    }

    // 일정시간마다 스프라이트를 교체하는 함수
    void SpriteAnimation()
    {
        animTime += Time.deltaTime;
        if (animTime >= changeTime)
        {
            spriteimg.sprite = changeImg[spImgCount];
            spImgCount += 1;
            if (spImgCount >= changeImg.Length)
                spImgCount = 0;
            animTime = 0f;
        }
    }

    // 마리오 손과 거리 판정하여 블록 반동을 시작하는 함수
    void Rebound()
    {  
        if (playerHand != GameManager.instance.GetPlayerHandInfo())
            playerHand = GameManager.instance.GetPlayerHandInfo();

        if (playerHand == null) return;

        bool isHit = Vector2.Distance(playerHand.transform.position, children.position) < triggerDistance;

        if (!isHit || rock) return;
        
        rock = true;
        StartCoroutine(BlockUp());
        
    }

    // 위로 솟아오르는 연출
    IEnumerator BlockUp()
    {
        Vector3 pos = transform.position;
        while(maxHight > transform.position.y)
        {
            pos.y += 0.1f;
            transform.position = pos;
            yield return null;
        }
        StartCoroutine(BlockDown());
    }

    // 원래 위치로 내려앉는 연출
    IEnumerator BlockDown()
    {
        Vector3 targetPos = transform.position;
        while (currPos.y < targetPos.y)
        {
            targetPos.y -= 0.1f;
            transform.position = targetPos;
            yield return null;
        }
        transform.position = currPos;
        spriteimg.sprite = emptySprite;
        Instantiate(Item, transform.position, Quaternion.identity);  
    }
}
