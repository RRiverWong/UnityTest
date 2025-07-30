using System.Collections.Generic;
using UnityEngine;

public class RelationshipLineDrawer : MonoBehaviour
{
    public Material lineMaterial;
    public float nodeRadius = 0.5f; // 控制线条从圆球边缘开始

    private List<LineRenderer> activeLines = new();

    public void ClearLines()
    {
        foreach (var line in activeLines)
        {
            if (line != null) Destroy(line.gameObject);
        }
        activeLines.Clear();
    }

    public void DrawRelationshipLine(Vector3 from, Vector3 to, float kinshipStrength)
    {
        if (lineMaterial == null)
        {
            Debug.LogWarning("Line material is not assigned.");
            return;
        }

        // 调整起止点：从球体边缘开始而不是圆心
        Vector3 dir = (to - from).normalized;
        Vector3 adjustedFrom = from + dir * nodeRadius;
        Vector3 adjustedTo = to - dir * nodeRadius;

        GameObject lineObj = new GameObject("RelationshipLine");
        lineObj.transform.SetParent(this.transform);

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.positionCount = 2;
        lr.SetPosition(0, adjustedFrom);
        lr.SetPosition(1, adjustedTo);

        // ✅ 宽度根据亲密程度调整
        float width = Mathf.Lerp(0.02f, 0.08f, kinshipStrength);
        lr.startWidth = width;
        lr.endWidth = width;

        // ✅ 改进：颜色从黑（弱）到红（强），加透明度渐变
        Color c = Color.Lerp(Color.black, Color.red, kinshipStrength);
        c.a = Mathf.Lerp(0.2f, 1f, kinshipStrength); // 越强越不透明
        lr.startColor = c;
        lr.endColor = c;

        // ✅ 其他设置
        lr.useWorldSpace = true;
        lr.numCapVertices = 2;
        lr.numCornerVertices = 2;
        lr.sortingOrder = 10;

        activeLines.Add(lr);
    }
}
