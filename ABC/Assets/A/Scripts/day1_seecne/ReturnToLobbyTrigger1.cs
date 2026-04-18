using UnityEngine;
using System.Collections;

public class ReturnToLobbyTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"触发大门，当前 step = {GameStateManager.Instance.mainQuestStep}");

        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (GameStateManager.Instance.mainQuestStep != 8) return;

        hasTriggered = true;
        StartCoroutine(TransitionToNight());
    }

    private IEnumerator TransitionToNight()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().DisableControl();

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("天色渐暗...");
            yield return new WaitForSeconds(1.5f);
            mask.CloseMask();
        }

        // 切换到夜晚大厅场景
        UnityEngine.SceneManagement.SceneManager.LoadScene("N_Sce_Lobby");
    }
}