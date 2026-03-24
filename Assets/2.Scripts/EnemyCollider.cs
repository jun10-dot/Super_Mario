using UnityEngine;

/// <summary>
/// 적의 충돌 감지를 처리하는 스크립트
/// </summary>
public class EnemyCollider : MonoBehaviour
{
    private Enemy enemy;
 
    void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        if (enemy == null)
            Debug.LogError("Null Enemy");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // 카메라 영역 진입 시 활동 시작
        if(col.tag == "MainCamera")
        {
            enemy.StartLife();
        }
        // 벽 충돌 시 방향 전환
        else if (col.tag == "Wall")
        {
            enemy.Flip();
        }
    }
}
