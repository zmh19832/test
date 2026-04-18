using UnityEngine;

public class ExitOfficeTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (GameStateManager.Instance.mainQuestStep != 6) return;

        hasTriggered = true;
        GameStateManager.Instance.mainQuestStep = 7;
        Debug.Log("离开办公室，主线进度变为 7");
    }
}