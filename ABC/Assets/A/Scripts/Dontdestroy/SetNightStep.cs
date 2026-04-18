using UnityEngine;

public class SetNightStep : MonoBehaviour
{
    private bool hasSet = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSet) return;
        if (!other.CompareTag("Player")) return;

        hasSet = true;
        if (GameStateManager.Instance.mainQuestStep != 8)
        {
            GameStateManager.Instance.mainQuestStep = 8;
            Debug.Log("夜晚场景：设置主线进度为 8");
        }
    }
}