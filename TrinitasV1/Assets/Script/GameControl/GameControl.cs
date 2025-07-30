using UnityEngine;

public class GameControl : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        // 退出游戏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 在编辑器中停止
#endif
        }

        // 暂停/取消暂停
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }
}

