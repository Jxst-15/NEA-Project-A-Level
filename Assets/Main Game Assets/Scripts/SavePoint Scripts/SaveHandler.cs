using UnityEngine;
using UnityEngine.Networking;

// Will handle database operations related to saving data
public class SaveHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame(float posX, float posY, int health, int stamina, int points)
    {
        Debug.Log("Saved");
        
        PlayerData.instance.posX = posX; 
        PlayerData.instance.posY = posY;
        PlayerData.instance.currentHealth = health;
        PlayerData.instance.currentStamina = stamina;
        PlayerData.instance.points = points;
        
        StartCoroutine(ConnectionHandler.instance.SaveGame("", health, stamina, posX, posY, points));
    }
}
