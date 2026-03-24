using UnityEngine;

/// <summary>
/// 플레이어 정보를 관리하는 매니저 클래스
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 객체
    private Transform player; // 플레이어 (부모 오브젝트)
    private Transform hand; // 손 (자식 오브젝트)

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    public void SetPlayerInfo(Transform player) // 플레이어 Transform 저장
    {
        if (player != null)
            this.player = player;
    }

    public void SetPlayerHandInfo(Transform hand) // 손 Transform 저장
    {
        if (hand != null)
           this.hand = hand;
    }

    public Transform GetPlayerInfo() // 저장된 플레이어 Transform 반환
    {
        if (this.player == null) return null;
        return this.player;
    }

    public Transform GetPlayerHandInfo() // 저장된 손 Transform 반환
    {
        if (this.hand == null) return null;
        return this.hand;
    }
}
