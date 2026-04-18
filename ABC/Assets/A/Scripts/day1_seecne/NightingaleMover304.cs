using UnityEngine;

public class NightingaleMover304 : MonoBehaviour
{
    public Transform[] waypoints;  // 路径点（从304门口到柜子前）
    public float moveSpeed = 2f;

    private int currentWaypoint = 0;
    private bool isMoving = false;
    private bool hasReachedEnd = false;

    void Start()
    {
        // 检查是否应该出现
        if (PlayerPrefs.GetInt("NightingaleArrived", 0) == 1)
        {
            gameObject.SetActive(true);
            // 清除标记，避免重复
            PlayerPrefs.DeleteKey("NightingaleArrived");

            // 开始移动
            isMoving = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isMoving) return;
        if (hasReachedEnd) return;

        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        if (currentWaypoint >= waypoints.Length)
        {
            if (!hasReachedEnd)
            {
                hasReachedEnd = true;
                OnReachEnd();
            }
            return;
        }

        Transform target = waypoints[currentWaypoint];
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypoint++;
        }
    }

    private void OnReachEnd()
    {
        Debug.Log("夜莺到达柜子前");

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("夜莺：就是这里，柜子后面有秘密...\n\n按空格继续");
            mask.OnMaskClosed += () => {
                GameStateManager.Instance.mainQuestStep = 11;
            };
        }
    }
}