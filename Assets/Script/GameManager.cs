using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int killCount = 0;

    public void IncrementKillCount()
    {
        killCount++;
        scoreText.text = "Score: " + killCount;
    }
}
