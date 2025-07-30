// WorldGridManager.cs
using System.Collections.Generic;
using UnityEngine;

public class WorldGridManager : MonoBehaviour
{
    public static WorldGridManager Instance;
    public float cellSize = 1f;
    public int maxPerCell = 5;

    private Dictionary<Vector2Int, List<HumanIdentity>> gridMap = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public Vector2Int GetGridPosition(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / cellSize),
            Mathf.FloorToInt(worldPos.y / cellSize)
        );
    }

    public void Register(HumanIdentity npc)
    {
        Vector2Int pos = GetGridPosition(npc.transform.position);
        if (!gridMap.ContainsKey(pos)) gridMap[pos] = new List<HumanIdentity>();
        if (!gridMap[pos].Contains(npc)) gridMap[pos].Add(npc);
    }

    public void Unregister(HumanIdentity npc, Vector2Int fromPos)
    {
        if (gridMap.ContainsKey(fromPos))
        {
            gridMap[fromPos].Remove(npc);
            if (gridMap[fromPos].Count == 0)
                gridMap.Remove(fromPos);
        }
    }

    public List<HumanIdentity> GetHumansInCell(Vector2Int pos)
    {
        if (gridMap.ContainsKey(pos)) return gridMap[pos];
        return new List<HumanIdentity>();
    }

    public bool IsOvercrowded(Vector2Int pos)
    {
        return GetHumansInCell(pos).Count > maxPerCell;
    }
}
