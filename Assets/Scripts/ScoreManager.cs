using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int currentScore;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Preserve across scene loads

            // Reset score when game starts
            currentScore = 0;

        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    // Call this function to reset the score
    public void ResetScore()
    {
        currentScore = 0;
    }
}
