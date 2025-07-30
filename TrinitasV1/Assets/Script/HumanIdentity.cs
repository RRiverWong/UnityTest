using System.Collections.Generic;
using UnityEngine;

public class HumanIdentity : MonoBehaviour
{
    public int Id;
    public List<int> parents = new();
    public List<int> children = new();
    public int HP = 100;
    public bool hasReproduced = false;

    public static GameObject deathMarkerPrefab;
    public static Transform markerContainer;

    public bool IsAlive => HP > 0;

    // 判断是否为“任何亲属关系”
    public bool IsRelatedTo(HumanIdentity other)
    {
        if (other == null) return false;

        foreach (int parentId in parents)
        {
            if (other.parents.Contains(parentId))
                return true;
        }

        if (children.Contains(other.Id) || other.children.Contains(this.Id))
            return true;

        return false;
    }

    // 亲属分类枚举
    public enum KinshipType
    {
        None,
        ParentChild,
        Sibling,
        DistantRelative
    }

    // 返回与某人之间的亲属类型（用于声音系统）
    public KinshipType GetKinshipWith(HumanIdentity other)
    {
        if (other == null) return KinshipType.None;

        if (children.Contains(other.Id) || other.children.Contains(this.Id))
            return KinshipType.ParentChild;

        foreach (int parentId in parents)
        {
            if (other.parents.Contains(parentId))
                return KinshipType.Sibling;
        }

        if (IsRelatedTo(other))
            return KinshipType.DistantRelative;

        return KinshipType.None;
    }

    public void Die()
    {
        if (!IsAlive) return;

        HP = 0;

        int currentYear = SimulationYearTracker.CurrentYear;
        GameStatsTracker.RegisterDeath(currentYear);

        if (deathMarkerPrefab != null)
        {
            GameObject cross = Object.Instantiate(deathMarkerPrefab, transform.position, Quaternion.identity);
            if (markerContainer != null)
                cross.transform.SetParent(markerContainer);
        }

        gameObject.SetActive(false);
    }
}
