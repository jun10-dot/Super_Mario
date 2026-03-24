using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 맵 밖으로 떨어진 오브젝트 제거, 플레이어인 경우 씬 재로드
/// </summary>
public class Remover : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) 
    {                     
        if (col.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamera>().enabled = false;
            Destroy(col.gameObject); 
            //일정 시간 후 현재 씬을 다시 로드
            StartCoroutine("ReloadGame"); 
        }
        else  // 나머지 오브젝트들은 파괴
        {
            Destroy(col.gameObject);
        }
    }

    IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
