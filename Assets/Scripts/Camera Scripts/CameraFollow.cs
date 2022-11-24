using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public bool camFollow;
    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camFollow = true;
        offset = 6.3f;
    }

    private void LateUpdate()
    {
        if (camFollow != false)
        {
            transform.position = new Vector3(player.transform.position.x + offset, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Boundary")
        {
            camFollow = false;
            Debug.Log("Collided with boundary");
        }
    }
}
