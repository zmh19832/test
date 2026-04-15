using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    public string spawnID = "StartPoint";

    private IEnumerator Start()
    {
        if (PlayerPrefs.GetString("SpawnPoint", "") == spawnID)
        {
            yield return null;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                player.transform.position = transform.position;

            PlayerPrefs.DeleteKey("SpawnPoint");

            if (FadeManager.Instance != null)
                StartCoroutine(FadeManager.Instance.FadeIn(1f));
        }
    }
}