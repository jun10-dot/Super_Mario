using UnityEngine;

/// <summary>
/// 적을 밟았을 때 위로 튀어오르게 처리하는 스크립트
/// </summary>
public class GroundCheck : MonoBehaviour
{
    private PlayerCtrl playerCtrl;
    private float bounceForce = 700f; // 반동 힘
    void Awake()
    {
        playerCtrl = transform.parent.GetComponent<PlayerCtrl>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<Enemy>() != null)
        {
            // 낙하 속도에 관계없이 일정한 높이로 튀어오르기 위해 velocity.y 초기화
            playerCtrl.rb.velocity = new Vector2(playerCtrl.rb.velocity.x, 0);
            playerCtrl.rb.AddForce(new Vector2(0f, bounceForce)); 
        }
    }
}
