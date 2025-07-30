using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public float cellSize = 1f;
    public Color gridColor = Color.black;

    private void OnDrawGizmos()
    {
        Gizmos.color = gridColor;

        // 水平方向
        for (int y = 0; y <= gridSizeY; y++)
        {
            Vector3 from = new Vector3(0, 0, y * cellSize);
            Vector3 to = new Vector3(gridSizeX * cellSize, 0, y * cellSize);
            Gizmos.DrawLine(from, to);
        }

        // 垂直方向
        for (int x = 0; x <= gridSizeX; x++)
        {
            Vector3 from = new Vector3(x * cellSize, 0, 0);
            Vector3 to = new Vector3(x * cellSize, 0, gridSizeY * cellSize);
            Gizmos.DrawLine(from, to);
        }
    }
}
