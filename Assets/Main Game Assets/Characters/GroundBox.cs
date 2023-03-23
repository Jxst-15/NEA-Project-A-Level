using UnityEngine;

public class GroundBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // The following is used to ignore the collision between the player and enemies
        // This is so they are able to walk past eachother without colliding and so both can still collide with walls etc.
        if (this.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("Enemy"));
        }
        else if (this.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("Player"));
        }
    }
}
