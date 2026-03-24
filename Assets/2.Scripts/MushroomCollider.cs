using UnityEngine;

/// <summary>
/// 버섯의 충돌 감지용 스크립트
/// </summary>
public class MushroomCollider : MonoBehaviour
{
    private Mushroom mushroom;

    void Awake()
    {
        mushroom = transform.parent.GetComponent<Mushroom>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (mushroom.isRising) return; // 상승 중일 때는 무시
        if(col.CompareTag("Wall"))
        {
            mushroom.Flip(); // 방향 전환
        }
    }
}
