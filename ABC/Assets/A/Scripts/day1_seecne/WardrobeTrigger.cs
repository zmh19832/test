using UnityEngine;

public class WardrobeTrigger : MonoBehaviour
{
    private bool hasTriggered = false;
    public float moveDistance = 2f;
    public float moveSpeed = 3f;
    public GameObject ventObject;  // 👈 新增：在 Inspector 中拖入 Vent 物体

    private Vector2 targetPosition;
    private bool isMoving = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"柜子触发器被触发，当前 step = {GameStateManager.Instance.mainQuestStep}");

        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (GameStateManager.Instance.mainQuestStep != 11) return;

        hasTriggered = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().DisableControl();

        targetPosition = (Vector2)transform.position + Vector2.right * moveDistance;
        isMoving = true;

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("你用力推开了柜子，后面果然有条通风管道...\n\n按空格继续");
            mask.OnMaskClosed += () => {
                GameStateManager.Instance.mainQuestStep = 12;
                player.GetComponent<PlayerController>().EnableControl();
                GetComponent<Collider2D>().enabled = false;
            };
        }
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                Debug.Log("柜子已推开，露出通风管道");

                if (ventObject != null)
                {
                    ventObject.SetActive(true);
                    Debug.Log("通风口已激活");
                }
                else
                {
                    Debug.LogError("请把 Vent 物体拖到 WardrobeTrigger 的 ventObject 栏位");
                }
            }
        }
    }
}