using UnityEngine;

public class SetStepOnEnterLobby : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        // 只有 step 为 7 时才设置为 8
        if (GameStateManager.Instance.mainQuestStep != 7) return;

        hasTriggered = true;
        GameStateManager.Instance.mainQuestStep = 8;
        Debug.Log($"[StepSetter] 玩家进入大厅，主线进度从 7 变为 8");

        // 可选：禁用自身，避免重复触发
        GetComponent<Collider2D>().enabled = false;
    }
}