using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class HumanReproduction : MonoBehaviour
{
    public GameObject humanPrefab;
    public GameObject homePrefab;  // ✅ 拖入 Home Prefab
    public float reproductionCooldown = 5f;

    private float lastReproductionTime = -999f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time - lastReproductionTime < reproductionCooldown) return;

        HumanIdentity myIdentity = GetComponent<HumanIdentity>();
        HumanIdentity otherIdentity = collision.gameObject.GetComponent<HumanIdentity>();

        if (otherIdentity != null && myIdentity != null &&
            myIdentity.IsAlive && otherIdentity.IsAlive &&
            !myIdentity.hasReproduced && !otherIdentity.hasReproduced &&
            humanPrefab != null)
        {
            Vector3 spawnPos = (transform.position + collision.transform.position) / 2f;
            GameObject child = Instantiate(humanPrefab, spawnPos, Quaternion.identity);

            // 设置身份信息
            HumanIdentity childIdentity = child.GetComponent<HumanIdentity>();
            childIdentity.Id = HumanSpawner.NextID++;
            childIdentity.parents.Add(myIdentity.Id);
            childIdentity.parents.Add(otherIdentity.Id);

            myIdentity.children.Add(childIdentity.Id);
            otherIdentity.children.Add(childIdentity.Id);
            GameStatsTracker.RegisterBirth();


            // 设置新生儿年龄为 0
            HumanAging aging = child.GetComponent<HumanAging>();
            if (aging != null)
                aging.currentAge = 0f;

            myIdentity.hasReproduced = true;
            otherIdentity.hasReproduced = true;

            lastReproductionTime = Time.time;

            // ✅ 实例化 Home prefab
            if (homePrefab != null)
            {
                Instantiate(homePrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}

