using UnityEngine;
using UnityEngine.UI;

public class PlayerColourHandler : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    // The player portrait image
    public GameObject playerImage;
    #endregion

    #region Colours
    // Denotes the possible colours that the player can choose
    private Color32 defCol = new Color32(255, 206, 233, 255);
    private Color32 redCol = new Color32(248, 53, 67, 255);
    private Color32 greenCol = new Color32(53, 248, 54, 255);
    private Color32 purpCol = new Color32(167, 53, 248, 255);
    #endregion
    
    public SpriteRenderer spriteRenderer;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (PlayerData.instance.colour)
        {
            case "Default":
                spriteRenderer.color = defCol;
                playerImage.GetComponent<Image>().color = defCol;
                break;
            case "Red":
                spriteRenderer.color = redCol;
                playerImage.GetComponent<Image>().color = redCol;
                break;
            case "Green":
                spriteRenderer.color = greenCol;
                playerImage.GetComponent<Image>().color = greenCol;
                break;
            case "Purple":
                spriteRenderer.color = purpCol;
                playerImage.GetComponent<Image>().color = purpCol;
                break;
        }
    }
    #endregion
}
