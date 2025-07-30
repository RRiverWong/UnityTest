using UnityEngine;

public class SimulationYearTracker : MonoBehaviour
{
    public static int CurrentYear { get; private set; } = 0;
    public float yearDuration = 1f; // 每“年”持续的时间（秒）

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= yearDuration)
        {
            timer -= yearDuration;
            CurrentYear++;
        }
    }

    public static void ResetYear()
    {
        CurrentYear = 0;
    }
}
