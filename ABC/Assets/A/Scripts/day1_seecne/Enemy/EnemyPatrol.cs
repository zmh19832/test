using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 2f;
    public float waitTime = 1f;

    private int currentWaypoint = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private bool isMovingEnabled = true;  // 新增：是否允许移动

    void Update()
    {
        if (!isMovingEnabled) return;  // 被视野脚本禁用移动

        if (waypoints.Length == 0) return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
            }
            return;
        }

        Transform target = waypoints[currentWaypoint];

        FaceDirection(target.position);

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            isWaiting = true;
            waitTimer = waitTime;
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void FaceDirection(Vector3 targetPosition)
    {
        Vector2 direction = targetPosition - transform.position;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void StopMoving()
    {
        isMovingEnabled = false;
        Debug.Log($"{gameObject.name} 停止移动（玩家在视野内）");
    }

    public void ResumeMoving()
    {
        isMovingEnabled = true;
        Debug.Log($"{gameObject.name} 恢复移动（玩家离开视野）");
    }
}