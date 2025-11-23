using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;

    private float facingDir = 1f;
    private bool canMove;
    private bool attackCheck;

    [SerializeField] private float speed;
    [SerializeField] private float reactBounceForce = 15f;

    [SerializeField] private float groundCheck;
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask targetLayerMask;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();
        setMove(true);
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheck, groundLayerMask);
        if (hit.collider == null) return;

        anim.SetBool("isGround", true);

        Collider2D collider = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayerMask);
        if (collider)
        {
            anim.SetBool("attack", true);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            setMove(false);
            if (attackCheck)
            {
                collider.GetComponent<Target>()?.TakeDamage();
                setAttackCheck(false);
            }
        }

        if (canMove)
            rb.linearVelocity = new Vector2(facingDir * speed, rb.linearVelocity.y);
    }

    public void TakeDamage()
    {
        anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, reactBounceForce);
    }

    public void setMove(bool enable)
    {
        canMove = enable;
    }

    public void setAttackCheck(bool enable)
    {
        attackCheck = enable;
    }

    private void OnDrawGizmos()
    {
        Vector3 toGround = new Vector3(transform.position.x, transform.position.y - groundCheck, 0);
        Gizmos.DrawLine(transform.position, toGround);

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
