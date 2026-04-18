using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string targetSceneName;
    public string spawnPointID;
    public int requiredStep = -1;  // -1 表示不需要条件，否则需要主线进度 >= 这个值

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 检查条件
        if (requiredStep != -1 && GameStateManager.Instance.mainQuestStep < requiredStep)
        {
            Debug.Log($"条件不足：需要主线进度 >= {requiredStep}，当前 = {GameStateManager.Instance.mainQuestStep}");
            return;
        }

        PlayerPrefs.SetString("SpawnPoint", spawnPointID);
        PlayerPrefs.Save();
        SceneManager.LoadScene(targetSceneName);
    }
}