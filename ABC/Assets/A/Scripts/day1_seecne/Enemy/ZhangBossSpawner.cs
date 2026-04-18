using UnityEngine;

public class ZhangBossSpawner : MonoBehaviour
{
    public GameObject zhangBossPrefab;
    public Transform spawnPosition;
    public Transform[] waypoints;  // 👈 在这里拖入路径点（场景中的空物体）

    private bool hasSpawned = false;

    void Start()
    {
        InvokeRepeating("CheckSpawn", 0f, 0.5f);
    }

    void CheckSpawn()
    {
        if (!hasSpawned && GameStateManager.Instance.mainQuestStep == 7)
        {
            SpawnZhangBoss();
        }
    }

    void SpawnZhangBoss()
    {
        hasSpawned = true;
        CancelInvoke("CheckSpawn");

        if (zhangBossPrefab != null && spawnPosition != null)
        {
            // 生成张主管
            GameObject newBoss = Instantiate(zhangBossPrefab, spawnPosition.position, spawnPosition.rotation);

            // 👇 关键：把路径点赋值给生成的张主管
            EnemyPatrol patrol = newBoss.GetComponent<EnemyPatrol>();
            if (patrol != null && waypoints.Length > 0)
            {
                patrol.waypoints = waypoints;
                Debug.Log($"张主管已生成，并赋值了 {waypoints.Length} 个路径点");
            }
            else
            {
                Debug.LogWarning("张主管生成成功，但没有路径点");
            }
        }
        else
        {
            Debug.LogError("生成失败：预制体或生成位置未设置");
        }
    }
}