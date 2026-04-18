using UnityEngine;
using UnityEngine.UI;
using System;

public class UIMaskManager : MonoBehaviour
{
    public GameObject maskPanel;
    public Text maskText;

    private bool isWaitingForSpace = false;
    public Action OnMaskClosed;

    void Start()
    {
        if (maskPanel != null) maskPanel.SetActive(false);
    }

    void Update()
    {
        if (isWaitingForSpace && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("按下了空格，关闭遮罩");
            CloseMask();
        }
    }

    private void EnsureReferences()
    {
        if (maskPanel == null)
        {
            maskPanel = GameObject.Find("MaskPlane");
            if (maskPanel != null)
            {
                maskText = maskPanel.GetComponentInChildren<Text>();
            }
        }
    }

    public void ShowMask(string message)
    {
        EnsureReferences();

        if (maskPanel == null)
        {
            Debug.LogError("UIMaskManager: 找不到 MaskPlane");
            return;
        }

        Debug.Log("显示遮罩：" + message);
        maskPanel.SetActive(true);
        if (maskText != null) maskText.text = message;
        isWaitingForSpace = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.DisableControl();
        }
    }

    public void CloseMask()
    {
        Debug.Log("关闭遮罩");
        if (maskPanel != null) maskPanel.SetActive(false);
        isWaitingForSpace = false;

        OnMaskClosed?.Invoke();
        OnMaskClosed = null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.EnableControl();
        }
    }

    public bool IsMaskActive()
    {
        return maskPanel != null && maskPanel.activeSelf;
    }
}