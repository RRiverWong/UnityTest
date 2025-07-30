using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    public GameObject homePrefab;
    public AudioClip homeSpawnSound; // 拖入你准备的 boom 声音

    private class Home
    {
        public GameObject marker;
        public HashSet<int> relatedNPCIds = new();
    }

    private static List<Home> homes = new();

    public static HomeManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public static void CreateHome(Vector3 position, int parent1Id, int parent2Id, int childId, GameObject homePrefab)
    {
        GameObject marker = Instantiate(homePrefab, position, Quaternion.identity);
        Home newHome = new Home
        {
            marker = marker,
            relatedNPCIds = new HashSet<int> { parent1Id, parent2Id, childId }
        };
        homes.Add(newHome);

        // ✅ 播放 3D 声音 —— 最稳妥方式
        if (Instance != null && Instance.homeSpawnSound != null)
        {
            AudioSource.PlayClipAtPoint(Instance.homeSpawnSound, position, 1f);
        }

        // ✅ Debug 检查播放位置
        Debug.Log("🏠 Home Created & Sound Played at " + position);
    }

    void Update()
    {
        for (int i = homes.Count - 1; i >= 0; i--)
        {
            Home home = homes[i];
            bool allDead = true;

            foreach (var npc in Object.FindObjectsByType<HumanIdentity>(FindObjectsSortMode.None))
            {
                if (home.relatedNPCIds.Contains(npc.Id) && npc.IsAlive)
                {
                    allDead = false;
                    break;
                }
            }

            if (allDead)
            {
                Destroy(home.marker);
                homes.RemoveAt(i);
            }
        }
    }

    public static List<Transform> GetAllHomePositionsForNPC(HumanIdentity npc)
    {
        List<Transform> list = new();
        foreach (var home in homes)
        {
            if (home.relatedNPCIds.Contains(npc.Id))
            {
                list.Add(home.marker.transform);
            }
        }
        return list;
    }
}
