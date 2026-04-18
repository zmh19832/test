using UnityEngine;
using System.Collections;

public class EnemyVision : MonoBehaviour
{
    [Header("视野设置")]
    public float visionRadius = 4f;
    public float visionAngle = 90f;
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;

    [Header("警戒设置")]
    public float detectTime = 1.5f;

    [Header("视觉显示")]
    public Color visionColor = new Color(1f, 0f, 0f, 0.3f);

    private float currentDetectTime = 0f;
    private GameObject player;
    private MeshFilter meshFilter;
    private EnemyPatrol patrolScript;
    private bool isPlayerInVision = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        patrolScript = GetComponent<EnemyPatrol>();
        CreateVisionMesh();
    }

    void CreateVisionMesh()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = visionColor;
        meshRenderer.material = mat;

        UpdateVisionMesh();
    }

    void Update()
    {
        if (player == null) return;

        bool playerDetected = CheckPlayerInVision();

        if (playerDetected)
        {
            if (!isPlayerInVision)
            {
                isPlayerInVision = true;
                if (patrolScript != null) patrolScript.StopMoving();
            }

            currentDetectTime += Time.deltaTime;

            // 视野跟随玩家方向
            UpdateVisionMesh();

            if (currentDetectTime >= detectTime)
            {
                OnPlayerDetected();
            }
        }
        else
        {
            if (isPlayerInVision)
            {
                isPlayerInVision = false;
                if (patrolScript != null) patrolScript.ResumeMoving();
            }
            currentDetectTime = 0f;
            UpdateVisionMesh();
        }
    }

    void UpdateVisionMesh()
    {
        Mesh mesh = new Mesh();
        int segments = 30;
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        float halfAngle = visionAngle / 2;
        Vector3 forward = transform.right;

        // 如果玩家在视野内，视野朝向玩家
        if (isPlayerInVision && player != null)
        {
            Vector2 directionToPlayer = player.transform.position - transform.position;
            forward = directionToPlayer.normalized;
        }

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfAngle + (float)i / segments * visionAngle;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * forward;
            Vector3 point = direction * visionRadius;
            vertices[i + 1] = point;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        if (meshFilter != null) meshFilter.mesh = mesh;
    }

    bool CheckPlayerInVision()
    {
        Vector2 directionToPlayer = player.transform.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance > visionRadius) return false;

        Vector2 forward = transform.right;
        float angle = Vector2.Angle(forward, directionToPlayer);

        if (angle > visionAngle / 2) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distance, obstacleLayer);
        if (hit.collider != null) return false;

        return true;
    }

    void OnPlayerDetected()
    {
        Debug.Log($"{gameObject.name} 发现了玩家！");

        UIMaskManager mask = FindObjectOfType<UIMaskManager>();
        if (mask != null)
        {
            mask.ShowMask("你被发现了...\n\n按空格继续");
            mask.OnMaskClosed += () => {
                TeleportPlayerToCheckpoint();
            };
        }
        else
        {
            TeleportPlayerToCheckpoint();
        }

        currentDetectTime = 0f;
    }

    void TeleportPlayerToCheckpoint()
    {
        // 保存要传送到的出生点 ID
        PlayerPrefs.SetString("SpawnPoint", "Corridor_AfterChenOffice");
        PlayerPrefs.Save();

        // 切换到走廊场景
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_Corridor");

        Debug.Log("玩家被发现，传送到走廊的 Corridor_AfterChenOffice");

        // 恢复玩家控制（切换场景后会在新场景中重新获取 Player）
        StartCoroutine(RestorePlayerControlAfterLoad());
    }

    IEnumerator RestorePlayerControlAfterLoad()
    {
        yield return null;  // 等待一帧，让新场景加载
        GameObject newPlayer = GameObject.FindGameObjectWithTag("Player");
        if (newPlayer != null)
        {
            newPlayer.GetComponent<PlayerController>().EnableControl();
        }
    }
}