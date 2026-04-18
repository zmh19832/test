using UnityEngine;

public class Exit307Trigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (GameStateManager.Instance.mainQuestStep != 2) return;

        hasTriggered = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().DisableControl();

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            // 强制对话 + 偷听教学
            string message = "同事 A：听说了吗，小陈好像被叫去 901 了...\n" +
                             "同事 B：嘘，小声点！\n\n" +
                             "【偷听教学】看到头顶有 [G] 图标的同事，靠近后按 G 即可偷听\n\n" +
                             "按空格继续";

            mask.ShowMask(message);
            mask.OnMaskClosed += () => {
                GameStateManager.Instance.mainQuestStep = 3;
                player.GetComponent<PlayerController>().EnableControl();
            };
        }
    }
}