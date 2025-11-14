using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float speed = 2;
    private bool canMove = false;
    private int facingDir = 1;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            setAnimParams(false, true, false);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            setAnimParams(true, false, false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            setAnimParams(false, false, true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Flip();
        }

        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(facingDir * speed, rb.linearVelocity.y);
    }
    private void setAnimParams(bool idle, bool move, bool attack)
    {
        anim.SetBool("idle", idle);
        anim.SetBool("move", move);
        anim.SetBool("attack", attack);
        canMove = move;
    }
    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingDir = -facingDir;
    }
}
