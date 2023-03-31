using UnityEngine;

// Handles player data in relation to saving and loading data
public class PlayerData : MonoBehaviour
{
    public string username
    { get; set; }
    public int enemiesDefeated
    { get; set; }
    public int points
    { get; set; }
    public Vector2 playerPos
    { get; set; }

    public static PlayerData instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetData()
    {
        enemiesDefeated = 0;
        points = 0;
    }
}

