using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;
    private SpriteRenderer sr;

    private float health;
    private float maxHealth = 10f;
    private float currentTime;
    private const float reactTime = 0.1f;
    private const float reactBounceForce = 15f;

    private Material original;
    [SerializeField] private Material reactMaterial;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        original = sr.material;
        health = maxHealth;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= reactTime)
        {
            sr.material = original;
            currentTime = 0f;
        }
    }

    public void TakeDamage()
    {
        health -= 1f;
        sr.material = reactMaterial;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.enabled = false;
        col.enabled = false;
        rb.gravityScale = 12f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, reactBounceForce);
    }
}
