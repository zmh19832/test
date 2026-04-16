using UnityEngine;
using UnityEngine.UI;

public class UIMaskManager : MonoBehaviour
{
    public GameObject maskPanel;   // 遮罩面板（全屏半透白）
    public Text maskText;          // 遮罩上的文字（旧版 UI）
    // 如果用 TextMeshPro，把上面的 Text 改成 TMP_Text

    private bool isWaitingForSpace = false;

    void Start()
    {
        if (maskPanel != null) maskPanel.SetActive(false);
    }

    void Update()
    {
        if (isWaitingForSpace && Input.GetKeyDown(KeyCode.Space))
        {
            CloseMask();
        }
    }

    public void ShowMask(string message)
    {
        if (maskPanel == null)
        {
            Debug.LogError("UIMaskManager: maskPanel 未赋值");
            return;
        }

        maskPanel.SetActive(true);
        if (maskText != null) maskText.text = message;
        isWaitingForSpace = true;

        // 禁用玩家控制
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.GetComponent<PlayerController>().DisableControl();
    }

    public void CloseMask()
    {
        if (maskPanel != null) maskPanel.SetActive(false);
        isWaitingForSpace = false;

        // 恢复玩家控制
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.GetComponent<PlayerController>().EnableControl();
    }
}