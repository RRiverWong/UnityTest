using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HumanGravityManager : MonoBehaviour
{
    public float baseStrength = 25f;
    public float distanceFalloff = 10f;
    public float soundCooldown = 1f;

    private static List<HumanGravity> humans = new();
    private Dictionary<(int, int), float> lastSoundTime = new();

    private GravitySoundManager soundManager;
    private RelationshipLineDrawer lineDrawer;

    public Material lineMaterial;

    public static void Register(HumanGravity hg)
    {
        if (!humans.Contains(hg)) humans.Add(hg);
    }

    public static void Unregister(HumanGravity hg)
    {
        if (humans.Contains(hg)) humans.Remove(hg);
    }

    void Start()
    {
        soundManager = FindObjectOfType<GravitySoundManager>();
        lineDrawer = GetComponent<RelationshipLineDrawer>();

        if (lineDrawer == null)
        {
            lineDrawer = gameObject.AddComponent<RelationshipLineDrawer>();
        }

        lineDrawer.lineMaterial = lineMaterial;
    }

    void LateUpdate()
    {
        lineDrawer?.ClearLines();
    }

    void FixedUpdate()
    {
        foreach (var hg in humans)
        {
            if (hg == null || !hg.GetIdentity().IsAlive) continue;

            Vector2 totalForce = Vector2.zero;
            HumanIdentity myIdentity = hg.GetIdentity();
            Vector2 myPos = hg.GetTransform().position;

            foreach (var other in humans)
            {
                if (other == hg || other == null) continue;
                var otherIdentity = other.GetIdentity();
                if (!otherIdentity.IsAlive) continue;

                Vector2 otherPos = other.GetTransform().position;
                Vector2 dir = otherPos - myPos;
                float dist = dir.magnitude;
                if (dist == 0) continue;

                // ✅ 获取亲属类型
                var kinType = myIdentity.GetKinshipWith(otherIdentity);

                // ✅ 决定引力强度倍数
                float kinMultiplier = kinType != HumanIdentity.KinshipType.None ? 2f : 1f;
                float strength = baseStrength * kinMultiplier * (1 - dist / distanceFalloff);
                strength = Mathf.Max(0, strength);
                totalForce += dir.normalized * strength;

                if (kinType != HumanIdentity.KinshipType.None)
                {
                    var key = (myIdentity.Id, otherIdentity.Id);
                    if (!lastSoundTime.ContainsKey(key) || Time.time - lastSoundTime[key] > soundCooldown)
                    {
                        float normalizedStrength = Mathf.Clamp01(strength / (baseStrength * 2f));

                        // ✅ 映射成声音系统用的类型
                        RelationshipType audioType = kinType switch
                        {
                            HumanIdentity.KinshipType.ParentChild => RelationshipType.ParentChild,
                            HumanIdentity.KinshipType.Sibling => RelationshipType.Sibling,
                            HumanIdentity.KinshipType.DistantRelative => RelationshipType.DistantRelative,
                            _ => RelationshipType.ParentChild
                        };

                        soundManager?.PlayRelationshipTone(normalizedStrength, myPos, otherPos, audioType);
                        lastSoundTime[key] = Time.time;
                    }

                    float normalizedKin = Mathf.Clamp01(kinMultiplier / 2f);
                    lineDrawer?.DrawRelationshipLine(myPos, otherPos, normalizedKin);
                }
            }

            hg.ApplyForce(totalForce);
        }
    }
}
