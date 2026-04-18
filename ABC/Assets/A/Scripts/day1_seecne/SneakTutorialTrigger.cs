using UnityEngine;

public class SneakTutorialTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (GameStateManager.Instance.mainQuestStep != 5) return;

        hasTriggered = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().DisableControl();

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("【潜行教学】\n\n躲避保安的视线，不要被发现\n\n按空格继续");
            mask.OnMaskClosed += () => {
                GameStateManager.Instance.mainQuestStep = 6;
                player.GetComponent<PlayerController>().EnableControl();
                gameObject.SetActive(false); // 教学完成后禁用触发器
            };
        }
    }
}