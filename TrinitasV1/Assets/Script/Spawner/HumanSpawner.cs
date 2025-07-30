using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    public GameObject humanPrefab;
    public GameObject deathMarkerPrefab; // 拖入你制作好的十字 prefab
    public int initialCount = 30;
    public float spawnRadius = 5f;

    public static int NextID = 0;

    void Start()
    {
        // 设置死亡标记的 prefab 和容器
        HumanIdentity.deathMarkerPrefab = deathMarkerPrefab;
        HumanIdentity.markerContainer = new GameObject("DeathMarkers").transform;

        for (int i = 0; i < initialCount; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
            GameObject human = Instantiate(humanPrefab, randomPos, Quaternion.identity);
            HumanIdentity identity = human.GetComponent<HumanIdentity>();
            identity.Id = NextID++;

            // ✅ 正确地记录出生
            GameStatsTracker.RegisterBirth();
        }
    }
}
