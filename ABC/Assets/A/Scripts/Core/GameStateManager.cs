using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    // ===== 主线进度 =====
    public int mainQuestStep = 0;  // 0 到 11

    // ===== 永久解锁类 =====
    public bool hasIDCard = false;      // 是否有工牌
    public bool hasSurfaceClue = false; // 是否有表层线索
    public bool hasDeepClue = false;    // 是否有深层线索（304钥匙）
    public bool hasStealTeach = false;  // 是否看过偷听教学
    public bool hasStealDone = false;   // 是否完成偷听
    public bool hasSneakTeach = false;  // 是否看过潜行教学
    public bool hasGottenClue = false;  // 是否已获取表层线索（用于触发张主管）

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}