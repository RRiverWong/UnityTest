using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldLoopManager : MonoBehaviour
{
    public float waitBeforeSummary = 5f;   // 所有人死亡后等待时间
    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered && AllHumansAreDead())
        {
            hasTriggered = true;
            StartCoroutine(HandleWorldEnd());
        }
    }

    bool AllHumansAreDead()
    {
        foreach (var human in FindObjectsOfType<HumanIdentity>())
        {
            if (human.IsAlive) return false;
        }
        return true;
    }

    IEnumerator HandleWorldEnd()
    {
        yield return new WaitForSeconds(waitBeforeSummary);

        // 获取统计数据
        int totalBirths = GameStatsTracker.TotalBirths;
        int lastDeathYear = GameStatsTracker.LastDeathYear;
        int worldNumber = GameStatsTracker.WorldCount;

        string[] lines = new string[]
        {
            $"WORLD {worldNumber} SIMULATION COMPLETE",
            $"TOTAL BIRTHS: {totalBirths}",
            $"THE LAST HUMAN DIED IN YEAR {lastDeathYear}"
        };

        // 调用 OverlayManager 显示总结
        OverlayManager overlay = FindObjectOfType<OverlayManager>();
        if (overlay != null)
        {
            yield return StartCoroutine(overlay.RunSummarySequence(lines));
        }

        // 让摄像机平滑缩回全图
        CameraController cam = FindObjectOfType<CameraController>();
        if (cam != null)
        {
            yield return StartCoroutine(cam.SmoothZoomOut(cam.zoomOutSize, cam.zoomLerpSpeed));
        }

        // 重置数据并重启世界
        GameStatsTracker.ResetForNewWorld();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("⏭ Reloading scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

}
