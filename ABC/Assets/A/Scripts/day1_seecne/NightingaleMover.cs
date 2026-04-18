using UnityEngine;

public class NightingaleMover : MonoBehaviour
{
    public Transform[] waypoints;  // 路径点数组
    public float moveSpeed = 2f;
    public string nextSceneName = "N_Sce_Office304";  // 304场景名
    public string spawnPointID = "Office304_Entrance";  // 304中的出生点ID

    private int currentWaypoint = 0;
    private bool isMoving = false;
    private bool hasReachedDoor = false;

    void Update()
    {
        if (!isMoving) return;
        if (hasReachedDoor) return;

        MoveToNextWaypoint();
    }

    public void StartMoving()
    {
        Debug.Log("StartMoving 被调用，路径点数量：" + (waypoints != null ? waypoints.Length.ToString() : "null"));
        isMoving = true;
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("没有设置路径点！");
            return;
        }

        if (currentWaypoint >= waypoints.Length)
        {
            if (!hasReachedDoor)
            {
                hasReachedDoor = true;
                OnReachDoor();
            }
            return;
        }

        Transform target = waypoints[currentWaypoint];
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypoint++;
            Debug.Log($"到达路径点 {currentWaypoint}/{waypoints.Length}");
        }
    }

    private void OnReachDoor()
    {
        Debug.Log("夜莺到达304门口，消失");

        // 夜莺消失
        gameObject.SetActive(false);

        // 保存夜莺状态，用于304场景
        PlayerPrefs.SetInt("NightingaleArrived", 1);
        PlayerPrefs.Save();

        // 显示提示
        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("夜莺飞进了304...\n\n按空格继续");
            mask.OnMaskClosed += () => {
                // 玩家可以进入304了
                Debug.Log("玩家可以进入304了");
            };
        }
    }
}