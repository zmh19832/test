using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VentTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void Start()
    {
        Debug.Log($"[VentTrigger] Start 被调用，场景：{gameObject.scene.name}，激活状态：{gameObject.activeSelf}");
        // 初始禁用，等柜子推开后才激活
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log($"[VentTrigger] OnEnable 被调用，Vent 被激活了");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[VentTrigger] OnTriggerEnter2D 被调用！");
        Debug.Log($"  - 进入的物体：{other.name}");
        Debug.Log($"  - 物体标签：{other.tag}");
        Debug.Log($"  - 当前 step：{GameStateManager.Instance.mainQuestStep}");

        if (hasTriggered)
        {
            Debug.Log($"[VentTrigger] 已经触发过了，跳过");
            return;
        }

        if (!other.CompareTag("Player"))
        {
            Debug.Log($"[VentTrigger] 不是玩家，跳过");
            return;
        }

        if (GameStateManager.Instance.mainQuestStep < 12)
        {
            Debug.Log($"[VentTrigger] step 条件不满足（需要 >=12，当前={GameStateManager.Instance.mainQuestStep}），跳过");
            return;
        }

        Debug.Log($"[VentTrigger] 条件满足，开始切换到第二天");
        hasTriggered = true;
        StartCoroutine(TransitionToLobby());
    }

    private IEnumerator TransitionToLobby()
    {
        Debug.Log($"[VentTrigger] 开始切换协程");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[VentTrigger] 找不到 Player！");
            yield break;
        }

        player.GetComponent<PlayerController>().DisableControl();

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("视野渐渐模糊...");
            yield return new WaitForSeconds(1.5f);
            mask.CloseMask();
        }

        // 重置主线进度（第二天开始）
        GameStateManager.Instance.mainQuestStep = 0;

        // 切换到白天大厅
        SceneManager.LoadScene("Scene_Lobby");

        // 设置第二天出生点位置为公司大门
        PlayerPrefs.SetString("SpawnPoint", "Lobby_Start_Day2");
        PlayerPrefs.Save();

        Debug.Log("[VentTrigger] 切换到第二天大厅完成");
    }
}