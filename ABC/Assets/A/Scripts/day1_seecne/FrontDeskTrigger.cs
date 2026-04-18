using UnityEngine;

public class FrontDeskTrigger : MonoBehaviour, IInteractable
{
    private bool hasTriggered = false;

    public void OnInteract()
    {
        Debug.Log("FrontDeskTrigger.OnInteract 被调用");
        Debug.Log($"hasTriggered={hasTriggered}, mainQuestStep={GameStateManager.Instance.mainQuestStep}");

        if (hasTriggered) return;
        if (GameStateManager.Instance.mainQuestStep != 0) return;

        hasTriggered = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().DisableControl();

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("前台：新员工先去 307 找主管领取身份牌\n\n按空格继续");
            mask.OnMaskClosed += () => {
                GameStateManager.Instance.mainQuestStep = 1;
                GameStateManager.Instance.hasIDCard = true;
                Inventory.Instance.AddItem("工牌");
                GetComponent<Collider2D>().enabled = false;
                player.GetComponent<PlayerController>().EnableControl();
            };
        }
    }

    public void OnSteal() { Debug.Log("没什么可偷听的"); }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("进入前台范围，触发物体：" + other.name + "，标签：" + other.tag);
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家进入前台范围，调用 EnterInteractRange");
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