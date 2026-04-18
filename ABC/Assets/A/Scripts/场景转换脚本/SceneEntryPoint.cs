using UnityEngine;

public class SceneEntryPoint : MonoBehaviour
{
    public string spawnID;

    void Start()
    {
        if (PlayerPrefs.GetString("SpawnPoint", "") == spawnID)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = transform.position;
            }
            PlayerPrefs.DeleteKey("SpawnPoint");
        }
    }
}