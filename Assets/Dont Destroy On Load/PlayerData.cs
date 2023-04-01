using UnityEngine;

// Handles player data in relation to saving and loading data
public class PlayerData : MonoBehaviour
{
    #region Fields
    #region Getters and Setters
    public string username
    { get; set; }
    public int enemiesDefeated
    { get; set; }
    public int points
    { get; set; }
    public Vector2 playerPos
    { get; set; }
    #endregion

    public static PlayerData instance;
    #endregion

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        username = "None";
        points = 0;
    }

    public void ResetData()
    {
        enemiesDefeated = 0;
        points = 0;
    }
}

