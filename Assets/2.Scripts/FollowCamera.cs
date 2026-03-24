using UnityEngine;

/// <summary>
/// 플레이어의 이동을 추적하는 카메라 제어 스크립트
/// </summary>
public class FollowCamera : MonoBehaviour
{
    private Transform player;  // 타겟 대상 마리오
    // 카메라가 플레이어를 따라 이동하기 시작하기 전의 최소 허용 거리
    public int xMargin = 1;
    // 플레이어 위치로 이동 시 적용되는 부드러움 정도 (값이 높을수록 빠르게 따라감)
    public int xSmooth = 8;     
    public int mapLimitX = 313; // 카메라 이동 제한 좌표

    // 카메라 위치와 플레이어 위치 간의 거리가 xMargin 보다 값이 큰지 확인하는 함수
    bool CheckXMargin() 
    {
        // 절대값 사용
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }

    void FixedUpdate()
    {
        if (player != GameManager.instance.GetPlayerInfo()) 
            player = GameManager.instance.GetPlayerInfo(); // 마리오 변신 시 객체 참조
        
        if(player == null)
           return;
        TrackPlayer();
    }

    void TrackPlayer()
    {
        // 현재 위치로부터 플레이어가 이동한만큼 움직이기위한 변수
        float targetX = transform.position.x;

        // 만약 플레이어가 xMargin 이상 이동했다면
        if (CheckXMargin())
            // 선형 보간을 사용하여 현재 카메라 위치에서 플레이어 위치로 부드럽게 목표 위치를 계산
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

        // 오른쪽으로만 이동
        if(transform.position.x > targetX) 
            return;
        
        // 맵 최대 크기까지만 이동하도록 제한
        if(transform.position.x >= mapLimitX)  
            return; 
        // 계산된 targetX을 사용하여 카메라의 위치 업데이트
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
