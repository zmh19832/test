using UnityEngine;

public class NightingaleTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"夜莺触发器被触发，物体：{other.name}，标签：{other.tag}");

        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (GameStateManager.Instance.mainQuestStep != 8) return;

        hasTriggered = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().DisableControl();

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("夜莺：跟我来，我知道304的柜子里藏着什么...\n\n（获得304钥匙）\n\n按空格继续");
            mask.OnMaskClosed += () => {
                GameStateManager.Instance.mainQuestStep = 9;
                GameStateManager.Instance.hasDeepClue = true;
                Inventory.Instance.AddItem("深层线索1（304钥匙）");
                player.GetComponent<PlayerController>().EnableControl();

                // 夜莺开始移动
                NightingaleMover mover = GetComponent<NightingaleMover>();
                if (mover != null)
                {
                    Debug.Log("夜莺开始移动");
                    mover.StartMoving();
                }
                else
                {
                    Debug.LogError("NightingaleMover 脚本未找到！");
                }

                GetComponent<Collider2D>().enabled = false;
            };
        }
    }
}