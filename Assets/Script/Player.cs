using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private int facingDir = 1;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpValue = 12f;
    [SerializeField] private float groundCheck;
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask enemyLayerMask;

    [SerializeField] private GameManager gameManager;

    private bool canMove = true;
    private bool isGround = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        HandleHorizontalMovement();
        HandleVerticalMovement();
        TryToAttack();
    }

    private void HandleHorizontalMovement()
    {
        if (!canMove) return;

        float xInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);
        anim.SetFloat("xVelocity", rb.linearVelocity.x);

        if (facingDir == 1 && rb.linearVelocity.x < 0)
        {
            Flip();
        }
        else if (facingDir == -1 && rb.linearVelocity.x > 0)
        {
            Flip();
        }
    }

    private void HandleVerticalMovement()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpValue);
            isGround = false;
        }

        anim.SetBool("isGround", isGround);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        RaycastHit2D hitResult = Physics2D.Raycast(transform.position, Vector2.down, groundCheck, groundLayerMask);
        isGround = hitResult.collider != null;
    }

    private void TryToAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("attack", true);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            setMove(false);
        }
    }

    public void AttackOverlap()
    {
        Collider2D[] collider2DList = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayerMask);
        foreach(Collider2D collider in collider2DList)
        {
            collider.GetComponent<Enemy>().TakeDamage();
            gameManager.IncrementKillCount();
        }
    }

    public void setMove(bool enable)
    {
        canMove = enable;
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingDir = -facingDir;
    }

    private void OnDrawGizmos()
    {
        Vector3 toGround = new Vector3(transform.position.x, transform.position.y - groundCheck, 0);
        Gizmos.DrawLine(transform.position, toGround);

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
