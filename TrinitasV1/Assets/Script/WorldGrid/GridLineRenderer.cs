using UnityEngine;

[ExecuteAlways]
public class GridLineRenderer : MonoBehaviour
{
    public Material lineMaterial;
    public int gridSizeX = 20;
    public int gridSizeY = 20;
    public float cellSize = 1f;

    void OnPostRender()
    {
        if (!lineMaterial) return;

        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(Color.black);

        // 画垂直线
        for (int x = 0; x <= gridSizeX; x++)
        {
            float xPos = x * cellSize;
            GL.Vertex(new Vector3(xPos, 0, 0));
            GL.Vertex(new Vector3(xPos, gridSizeY * cellSize, 0));
        }

        // 画水平线
        for (int y = 0; y <= gridSizeY; y++)
        {
            float yPos = y * cellSize;
            GL.Vertex(new Vector3(0, yPos, 0));
            GL.Vertex(new Vector3(gridSizeX * cellSize, yPos, 0));
        }

        GL.End();
    }
}
