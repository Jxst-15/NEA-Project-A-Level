using UnityEngine;

// CodeMonkey Tutorial
public class InfiniteBackground : MonoBehaviour
{
    private Transform camTransform;
    private float textureUnitSizeX;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        
        Sprite sprite = GetComponent<SpriteRenderer>().sprite; 
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit * transform.localScale.x;
    }

    private void LateUpdate()
    {
        if (Mathf.Abs(camTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPosX = (camTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(camTransform.position.x - offsetPosX, transform.position.y);
        }
    }
}
