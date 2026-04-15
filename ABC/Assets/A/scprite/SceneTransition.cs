using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [Header("目标设置")]
    public string targetSceneName = "Scene2";
    public string targetSpawnPointID = "StartPoint";

    [Header("黑屏时间")]
    public float fadeDuration = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        if (FadeManager.Instance != null)
            yield return StartCoroutine(FadeManager.Instance.FadeOut(fadeDuration));

        PlayerPrefs.SetString("SpawnPoint", targetSpawnPointID);
        PlayerPrefs.Save();

        SceneManager.LoadScene(targetSceneName);
    }
}