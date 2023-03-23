using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    #region Fields
    #region Variables
    private Transform camTransform;
    private float textureUnitSizeX;
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        // To work out where to place the offset and position of the BG dependent on the camera position
        camTransform = Camera.main.transform;
        
        Sprite sprite = GetComponent<SpriteRenderer>().sprite; 

        // The actual image, used to calculate the 
        Texture2D texture = sprite.texture;

        /* Gets the width of the texture and divides by how many pixels the BG uses to fill one Unity unit, used for setting the x boundary of the BG
           Multiplies by its localScale.x to ensure that the transition is smooth as it needs to account for resizing */
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit * transform.localScale.x;
    }

    // LateUpdate runs after all other update methods, used as better for correlating BG with player movement
    private void LateUpdate()
    {
        /* Used to determine if the x bound has been passed by the camera or on it
           Uses Mathf.Abs to make it work when moving left and right */
        if (Mathf.Abs(camTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            // Calculates the offset of the BG as the position it is translated at may not be correct
            float offsetPosX = (camTransform.position.x - transform.position.x) % textureUnitSizeX;

            // Sets the new pos of the BG as the x pos of the camera - the offset and the y pos as the same
            transform.position = new Vector3(camTransform.position.x - offsetPosX, transform.position.y);
        }
    }
    #endregion
}
