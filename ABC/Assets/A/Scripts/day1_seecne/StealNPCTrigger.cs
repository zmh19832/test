using UnityEngine;

public class StealNPCTrigger : MonoBehaviour, IInteractable
{
    private bool hasTriggered = false;

    public void OnInteract()
    {
        // 按 F 无效
        Debug.Log("按 F 没用，试试按 G 偷听");
    }

    public void OnSteal()
    {
        if (hasTriggered) return;
        if (GameStateManager.Instance.mainQuestStep != 3) return;

        hasTriggered = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().DisableControl();

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("同事：你听说了吗，小陈好像被叫去 901 了...\n那房间有问题！\n\n按空格继续");
            mask.OnMaskClosed += () => {
                GameStateManager.Instance.mainQuestStep = 4;
                player.GetComponent<PlayerController>().EnableControl();
                GetComponent<Collider2D>().enabled = false;  // 偷听后不再可偷听
            };
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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