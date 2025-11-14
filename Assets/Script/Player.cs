using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private int facingDir = 1;
    [SerializeField] private float speed = 8f;

    private bool canMove = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleHorizontalMovement();
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

    private void TryToAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("attack", true);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            setMove(false);
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
}
