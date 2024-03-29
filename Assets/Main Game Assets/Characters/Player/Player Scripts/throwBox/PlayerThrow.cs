using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    #region Fields
    #region Script References
    public PlayerCombat playerCombat;
    #endregion

    #region GameObjects
    public GameObject throwEnd; // Which direction the player is facing in

    private GameObject toThrow;
    #endregion

    #region Variables
    private float speed;
    private float throwDuration;
    #endregion

    #region Getters and Setters
    // How long the throw will last, set to 3 seconds by default
    public float maxThrowDuration
    { get; set; }
    #endregion

    // Stores a list of colliders which will be used to determine which enemy to throw
    private List<Collider2D> objectsHit = new List<Collider2D>();
    #endregion

    #region Unity Methods
    void Awake()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
        maxThrowDuration = 3f;
        throwDuration = maxThrowDuration;
    }

    void Update()
    {
        // Only if the player is currently throwing
        if (playerCombat.throwing == true && toThrow != null)
        {
            if (throwDuration <= 0f)
            {
                // Ends the throw
                playerCombat.throwing = false;
                
                toThrow.GetComponent<EnemyMovement>().canMove = true;
                toThrow.GetComponent<EnemyCombat>().canAttack = true;
                toThrow.GetComponent<EnemyCombat>().canDefend = true;
                
                toThrow.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                throwDuration = maxThrowDuration;

                toThrow = null;
            }
            // If the throw duration has not reached 0 yet
            else if (throwDuration > 0)
            {
                throwDuration -= Time.deltaTime;
            }
        }
    }
    #endregion

    // Main method for throwing an enemy
    public void Throw()
    {
        // Takes the position of the throwEnd object and takes it away from the current position of this object 
        // It is normalized so that it only stores its direction
        Vector2 direction = ((throwEnd.transform.position) - transform.position).normalized;
        
        if (objectsHit.Count == 0 || objectsHit[0].gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("No valid enemy to throw");
        }
        else if (objectsHit[0] != null && objectsHit[0].gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Takes the first enemy in the list to throw
            toThrow = objectsHit[0].gameObject;
            playerCombat.throwing = true;
            
            toThrow.GetComponent<EnemyMovement>().canMove = false;
            toThrow.GetComponent<EnemyMovement>().StopMovement();
            toThrow.GetComponent<EnemyCombat>().canAttack = false;
            toThrow.GetComponent<EnemyCombat>().canDefend = false;

            // The enemy can no longer move and a force is applied so that it moves in the specified direction
            toThrow.GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Impulse);
            Debug.Log("Throw attack performed");
        }
    }

    public int ObjectsHitCount()
    {
        return objectsHit.Count;
    }

    #region OnTrigger Methods
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!objectsHit.Contains(col) && col.gameObject.layer == LayerMask.NameToLayer("Enemy") || col.gameObject.layer == LayerMask.NameToLayer("CanHit"))
        {
            objectsHit.Add(col);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (objectsHit.Contains(col) && col.gameObject.layer == LayerMask.NameToLayer("Enemy") || col.gameObject.layer == LayerMask.NameToLayer("CanHit"))
        {
            objectsHit.Remove(col);
        }
    }
    #endregion
}
