using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string levelToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 레벨 종료 지점에 도달했을 때
            // 다음 레벨로 이동
            LoadNextLevel();
        }
    }
    
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
