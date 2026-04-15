using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private void Awake()
    {
        // 获取刚体组件
        rb = GetComponent<Rigidbody2D>();
        // 获取动画组件（以后再加动画时用）
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 读取键盘输入（WASD 或 上下左右）
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 存储移动方向（归一化防止斜向更快）
        moveInput = new Vector2(moveX, moveY).normalized;

        // 控制动画（以后再加动画时用）
        if (animator != null)
        {
            animator.SetBool("isWalking", moveInput != Vector2.zero);
        }
    }

    private void FixedUpdate()
    {
        // 移动角色：Unity 2022.3 用 velocity，不是 linearVelocity
        rb.velocity = moveInput * moveSpeed;
    }
}