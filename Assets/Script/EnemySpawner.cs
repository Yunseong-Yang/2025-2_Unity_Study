using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private float currentTime = 0f;
    private const float spawnerTimer = 3f;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= spawnerTimer)
        {
            Instantiate(enemyPrefab, transform.position, transform.rotation);
            currentTime = 0f;
        }
    }
}
