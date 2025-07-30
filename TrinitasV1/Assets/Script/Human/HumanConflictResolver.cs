// HumanConflictResolver.cs
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanIdentity))]
[RequireComponent(typeof(HumanCombat))]
public class HumanConflictResolver : MonoBehaviour
{
    private HumanIdentity identity;
    private HumanCombat combat;
    private Vector2Int lastGridPos;

    void Start()
    {
        identity = GetComponent<HumanIdentity>();
        combat = GetComponent<HumanCombat>();
    }

    void Update()
    {
        if (!identity.IsAlive) return;

        Vector2Int currentPos = WorldGridManager.Instance.GetGridPosition(transform.position);

        if (currentPos != lastGridPos)
        {
            WorldGridManager.Instance.Unregister(identity, lastGridPos);
            WorldGridManager.Instance.Register(identity);
            lastGridPos = currentPos;
        }

        if (!WorldGridManager.Instance.IsOvercrowded(currentPos)) return;

        List<HumanIdentity> others = WorldGridManager.Instance.GetHumansInCell(currentPos);

        foreach (var other in others)
        {
            if (other == identity || !other.IsAlive) continue;
            if (identity.IsRelatedTo(other)) continue; // 亲属不攻击

            // 攻击目标
            var otherCombat = other.GetComponent<HumanCombat>();
            if (otherCombat != null)
            {
                otherCombat.TakeDamage(combat.attackPower);

                // 概率逃跑
                if (Random.value < 0.8f)
                {
                    Vector2 escapeDir = (other.transform.position - transform.position).normalized;
                    other.GetComponent<Rigidbody2D>().AddForce(escapeDir * 2f, ForceMode2D.Impulse);
                }
            }
        }
    }
}
