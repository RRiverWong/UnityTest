using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    [Header("世界边界设置")]
    public Vector2 center = Vector2.zero;
    public Vector2 size = new Vector2(20f, 20f);

    // 可供其他类使用的静态边界
    public static Rect Bounds;

    void Awake()
    {
        // 初始化静态边界（左下角坐标 + 尺寸）
        Bounds = new Rect(center - size / 2f, size);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
