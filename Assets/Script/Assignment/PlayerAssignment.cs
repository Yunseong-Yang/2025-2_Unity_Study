using UnityEngine;

public class PlayerAssignment : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private int facingDir = 1;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpValue = 12f;
    [SerializeField] private float dashValue = 30f; // 추가된 부분, Dash 속도
    [SerializeField] private float dashTime = 0.3f; // 추가된 부분, Dash 지속 시간

    [SerializeField] private float groundCheck;
    [SerializeField] private float wallCheck; // Dash 중 벽 충돌 체크용
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask enemyLayerMask;

    private bool canMove = true;
    private bool canDash = true; // 추가된 부분, Dash 가능 여부
    private bool isGround = true;
    private bool isDash = false; // 추가된 부분, Dash 상태 여부
    private float currentDashTime = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Dash();
        HandleHorizontalMovement();
        HandleVerticalMovement();
        TryToAttack();
    }

    private void HandleHorizontalMovement() // Dash 중에는 이동 불가, Dash 이벤트 처리
    {
        if (!canMove || isDash) return;

        float xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Dash") && canDash)
            setStartDashValue();

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

    private void HandleVerticalMovement() // Dash 중에는 점프 불가, Dash 이벤트 처리
    {
        if (isDash) return;

        if (Input.GetButtonDown("Jump") && isGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpValue);
            isGround = false;
        }

        if (Input.GetButtonDown("Dash") && canDash)
            setStartDashValue();

        anim.SetBool("isGround", isGround);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        RaycastHit2D hitResult = Physics2D.Raycast(transform.position, Vector2.down, groundCheck, groundLayerMask);
        isGround = hitResult.collider != null;

        if (isGround) // 땅에 닿았을 때, Dash 가능. 연속 Dash 방지
            canDash = true;
    }

    private void TryToAttack() // Dash 중에는 공격 불가
    {
        if (isDash) return;

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("attack", true);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            setMove(false);
        }
    }

    private void Dash() // 추가된 부분, Dash 처리
    {
        if (!isDash) return;

        currentDashTime += Time.deltaTime;
        if (currentDashTime <= dashTime)
        {
            rb.linearVelocity = new Vector2(facingDir * dashValue, 0); // Dash 중에는 수평 이동만 가능
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheck, groundLayerMask); // Dash 중 벽 충돌 체크하고
            if (hit.collider != null) // 벽에 충돌하면 Dash 종료
            {
                setEndDashValue();
            }
        }
        else // Dash 시간이 끝나면 Dash 종료
        {
            setEndDashValue();
        }
    }
    private void setStartDashValue() // 추가된 부분, Dash 시작 시 호출
    {
        isDash = true;
        anim.SetBool("isDash", isDash);
        canDash = false;
    }

    private void setEndDashValue() // 추가된 부분, Dash 종료 시 호출
    {
        currentDashTime = 0f;
        isDash = false;
        anim.SetBool("isDash", isDash);
    }

    public void AttackOverlap()
    {
        Collider2D[] collider2DList = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayerMask);
        foreach(Collider2D collider in collider2DList)
        {
            collider.GetComponent<Enemy>().TakeDamage();
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

        Vector3 toWall = new Vector3(transform.position.x + facingDir * wallCheck, transform.position.y, 0);
        Gizmos.DrawLine(transform.position, toWall);

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
