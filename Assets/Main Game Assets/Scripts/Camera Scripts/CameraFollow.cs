using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region fields
    [SerializeField] private GameObject player;
    public bool camFollow;
    public float offset;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camFollow = true;
        offset = 4.5f;
    }

    private void LateUpdate()
    {
        if (camFollow == true && player != null)
        {
            // Sets the position of the camera
            transform.position = new Vector3(player.transform.position.x + offset, transform.position.y, transform.position.z);
        }
    }
    #endregion
}
