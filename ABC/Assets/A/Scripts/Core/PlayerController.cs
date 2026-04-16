using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool canControl = true;

    private bool canInteract = false;
    private IInteractable currentInteractable;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canControl)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(moveX, moveY).normalized;
        }
        else
        {
            moveInput = Vector2.zero;
        }

        if (canControl && Input.GetKeyDown(KeyCode.F) && canInteract)
        {
            currentInteractable?.OnInteract();
        }

        if (canControl && Input.GetKeyDown(KeyCode.G) && canInteract)
        {
            currentInteractable?.OnSteal();
        }
    }

    void FixedUpdate()
    {
        // 修正：用 velocity 而不是 linearVelocity
        rb.velocity = moveInput * moveSpeed;
    }

    public void DisableControl()
    {
        canControl = false;
        moveInput = Vector2.zero;
        rb.velocity = Vector2.zero;
    }

    public void EnableControl()
    {
        canControl = true;
    }

    public void EnterInteractRange(IInteractable interactable)
    {
        canInteract = true;
        currentInteractable = interactable;
    }

    public void ExitInteractRange()
    {
        canInteract = false;
        currentInteractable = null;
    }
}