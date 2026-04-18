using UnityEngine;

public class ChenDeskTrigger : MonoBehaviour, IInteractable
{
    [Header("张主管生成设置")]
    public GameObject zhangBossPrefab;
    public Transform spawnPosition;
    public Transform[] waypoints;

    // 静态变量，跨场景保留
    private static bool hasTriggered = false;
    private static bool hasSpawned = false;
    private static GameObject currentZhangBoss = null;

    void OnEnable()
    {
        // 每次进入场景时调用
        if (hasSpawned && currentZhangBoss == null)
        {
            // 张主管已经生成过，但被销毁了（比如切换场景），重新生成
            RespawnZhangBoss();
        }
    }

    void OnDestroy()
    {
        // 场景卸载时，标记张主管需要重新生成，但不销毁静态标记
        // 保留 hasTriggered 和 hasSpawned
    }

    public void OnInteract()
    {
        Debug.Log($"[ChenDeskTrigger] OnInteract, hasTriggered={hasTriggered}, step={GameStateManager.Instance.mainQuestStep}");

        if (hasTriggered) return;
        if (GameStateManager.Instance.mainQuestStep != 6) return;

        hasTriggered = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().DisableControl();

        GameStateManager.Instance.hasSurfaceClue = true;
        Inventory.Instance.AddItem("表层线索1（带有淡绿色污染的标签）");

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("你找到了一个带有淡绿色污染的标签...\n\n这看起来不对劲。\n\n按空格继续");
            mask.OnMaskClosed += () => {
                ShowBossDialogue(player);
            };
        }
    }

    private void ShowBossDialogue(GameObject player)
    {
        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("张主管（电话声）：那个新来的好像在查什么东西...\n\n派人盯紧点。\n\n按空格继续");
            mask.OnMaskClosed += () => {
                GameStateManager.Instance.mainQuestStep = 7;
                player.GetComponent<PlayerController>().EnableControl();
                GetComponent<Collider2D>().enabled = false;
            };
        }

        if (!hasSpawned)
        {
            Invoke("SpawnZhangBoss", 0.2f);
        }
    }

    private void SpawnZhangBoss()
    {
        hasSpawned = true;

        if (zhangBossPrefab == null || spawnPosition == null)
        {
            Debug.LogError("ZhangBoss 预制体或生成位置未设置");
            return;
        }

        currentZhangBoss = Instantiate(zhangBossPrefab, spawnPosition.position, spawnPosition.rotation);

        EnemyPatrol patrol = currentZhangBoss.GetComponent<EnemyPatrol>();
        if (patrol != null && waypoints != null && waypoints.Length > 0)
        {
            patrol.waypoints = waypoints;
            Debug.Log($"张主管已生成，并赋值了 {waypoints.Length} 个路径点");
        }
    }

    private void RespawnZhangBoss()
    {
        if (zhangBossPrefab == null || spawnPosition == null) return;

        currentZhangBoss = Instantiate(zhangBossPrefab, spawnPosition.position, spawnPosition.rotation);

        EnemyPatrol patrol = currentZhangBoss.GetComponent<EnemyPatrol>();
        if (patrol != null && waypoints != null && waypoints.Length > 0)
        {
            patrol.waypoints = waypoints;
            Debug.Log($"张主管重新生成（切换场景后），并赋值了 {waypoints.Length} 个路径点");
        }
    }

    public void OnSteal() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().EnterInteractRange(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ExitInteractRange();
        }
    }
}