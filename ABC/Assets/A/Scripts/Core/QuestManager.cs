using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public Text questText;  // 任务栏文字组件
    private int lastStep = -1;

    void Update()
    {
        int currentStep = GameStateManager.Instance.mainQuestStep;
        if (currentStep != lastStep)
        {
            lastStep = currentStep;
            UpdateQuestText();
        }
    }

    void UpdateQuestText()
    {
        string text = "";
        switch (lastStep)
        {
            case 0: text = "与公司大厅前台对话"; break;
            case 1: text = "坐电梯去 307 找主管领取身份牌"; break;
            case 2: text = "去小陈办公室一探究竟"; break;
            case 3: text = "偷听同事对话"; break;
            case 4: text = "进入小陈办公室"; break;
            case 5: text = "搜寻小陈办公桌"; break;
            case 6: text = "回家休息"; break;
            case 7: text = "在夜晚中寻找声音来源"; break;
            case 8: text = "跟随夜莺前进"; break;
            case 9: text = "查看异常柜子"; break;
            case 10: text = "靠近通风管道"; break;
            case 11: text = "任务完成"; break;
            default: text = ""; break;
        }
        if (questText != null) questText.text = text;
    }
}