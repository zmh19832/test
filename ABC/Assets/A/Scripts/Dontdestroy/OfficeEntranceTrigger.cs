using UnityEngine;

public class OfficeEntranceTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (GameStateManager.Instance.mainQuestStep != 4) return;

        hasTriggered = true;
        GameStateManager.Instance.mainQuestStep = 5;
        Debug.Log("进入小陈办公室，主线进度变为 5");
    }
}