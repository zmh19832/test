using UnityEngine;

public class AdminTalkTrigger : MonoBehaviour, IInteractable
{
    private bool hasTriggered = false;

    public void OnInteract()
    {
        Debug.Log($"AdminTalkTrigger.OnInteract 被调用，当前 step={GameStateManager.Instance.mainQuestStep}");

        if (hasTriggered) return;
        if (GameStateManager.Instance.mainQuestStep != 1) return;

        hasTriggered = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().DisableControl();

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("主管：这是你的身份牌，去小陈办公室看看吧\n\n按空格继续");
            mask.OnMaskClosed += () => {
                GameStateManager.Instance.mainQuestStep = 2;
                GetComponent<Collider2D>().enabled = false;
                player.GetComponent<PlayerController>().EnableControl();
            };
        }
    }

    public void OnSteal() { Debug.Log("主管没什么可偷听的"); }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"AdminTalkTrigger 触发，物体：{other.name}，标签：{other.tag}");
        if (other.CompareTag("Player"))
        {
            Debug.Log("AdminTalkTrigger: 调用 EnterInteractRange");
            other.GetComponent<PlayerController>().EnterInteractRange(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ExitInteractRange();
        }
    }
}