using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private float receiveDamageColorDuration = 1f;

    private float currentTime;
    private float lastTime;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        currentTime = Time.time;
        if (currentTime > lastTime + receiveDamageColorDuration)
        {
            if (sr.color != Color.white)
                sr.color = Color.white;
        }
    }

    public void TakeDamage()
    {
        sr.color = Color.red;
        lastTime = Time.time;
    }
}
