using UnityEngine;

// Handles player data in relation to saving and loading data
public class PlayerData : MonoBehaviour
{
    #region Fields
    // Denotes the data that the player has
    #region Getters and Setters
    public string username
    { get; set; }
    public int enemiesDefeated
    { get; set; }
    public int points
    { get; set; }
    public int currentHealth
    { get; set; }
    public int currentStamina
    { get; set; }
    public Vector2 playerPos
    { get; set; }
    public float posX
    { get; set; }
    public float posY
    { get; set; }
    public string accCreated
    { get; set; }
    public string lastSaveDate
    { get; set; }
    #endregion

    public static PlayerData instance; // So data can be modified without needing a reference to it 
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
        accCreated = "2005-04-15";
        lastSaveDate = "2005-04-15";
        ResetData();
    }

    public void ResetData()
    {
        enemiesDefeated = 0;
        points = 0;
        currentHealth = 700;
        currentStamina = 350;
        posX = -32.70996f;
        posY = -4.51f;
    }
}

