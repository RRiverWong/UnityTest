// Step 1: GravityCalculator.cs
using UnityEngine;

public static class GravityCalculator
{
    /// <summary>
    /// 计算人类与目标（其他人或事件点）之间的引力值
    /// </summary>
    /// <param name="distance">两者之间的距离（建议归一化到0-100）</param>
    /// <param name="strength">引力强度（例如事件点的默认强度，0-100）</param>
    /// <param name="kinship">亲属力（0 = 无血缘，50 = 直系亲属）</param>
    /// <returns>最终引力值（0-250）</returns>
    public static float ComputeGravity(float distance, float strength, float kinship)
    {
        float distFactor = Mathf.Clamp(100f - distance, 0f, 100f); // 距离越近，引力越大
        float total = distFactor + Mathf.Clamp(strength, 0f, 100f) + Mathf.Clamp(kinship, 0f, 50f);
        return Mathf.Clamp(total, 0f, 250f);
    }
}
