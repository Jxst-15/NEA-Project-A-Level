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
    public float posX
    { get; set; }
    public float posY
    { get; set; }
    public string accCreated
    { get; set; }
    public string lastSaveDate
    { get; set; }
    public string colour
    { get; set; }
    #endregion

    public static PlayerData instance; // So data can be modified without needing a reference to it, able to be kept the same between scenes
    #endregion

    private void Awake()
    {
        // If there is already a PlayerData in the scene then destroy the newly created gameobject
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
        colour = "Default";
        ResetData();
    }

    public void ResetData()
    {
        enemiesDefeated = 0;
        points = 0;
        currentHealth = 900;
        currentStamina = 450;
        posX = -32.70996f;
        posY = -4.51f;
    }
}

