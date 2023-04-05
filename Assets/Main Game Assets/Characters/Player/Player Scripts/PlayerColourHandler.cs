using UnityEngine;

public class PlayerColourHandler : MonoBehaviour
{
    #region Fields
    public SpriteRenderer spriteRenderer;

    #region Colours
    private Color32 defCol = new Color32(255, 206, 233, 255);
    private Color32 redCol = new Color32(248, 53, 67, 255);
    private Color32 greenCol = new Color32(53, 248, 54, 255);
    private Color32 purpCol = new Color32(167, 53, 248, 255);
    #endregion
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
                break;
            case "Red":
                spriteRenderer.color = redCol;
                break;
            case "Green":
                spriteRenderer.color = greenCol;
                break;
            case "Purple":
                spriteRenderer.color = purpCol;
                break;
        }

        Debug.Log(spriteRenderer.color.ToString());
    }
    #endregion
}
