using UnityEngine;

// WIP
public abstract class CharMovement : MonoBehaviour
{
    #region Fields
    #region Variables
    // Variables for movement
    [SerializeField] protected int vSpeed, vRunSpeed, hSpeed, hRunSpeed;
    [SerializeField] protected bool isRunning;
    
    // For flipping the character
    protected bool facingRight;
    protected float scaleX, scaleY;

    // Variables for dodging
    [SerializeField] protected bool isDodging;
    [SerializeField] protected float dodgeSpeed;
    [SerializeField] protected float startDodgeTime;
    [SerializeField] protected int side;
    protected float dodgeTime;

    [SerializeField] protected bool onGround;
    #endregion

    #region Getters and Setters
    public bool canMove
    { get; set; }
    #endregion

    protected Rigidbody2D rb;
    #endregion

    #region Unity Methods
    protected virtual void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        SetVariables();
    }

    protected abstract void Update();

    protected abstract void FixedUpdate();
    #endregion

    protected virtual void SetVariables()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;

        canMove = true;
        isRunning = false;
        isDodging = false;
        onGround = true;
    }

    protected abstract void Movement();

    protected abstract void Run();

    protected abstract void Dodge();

    protected abstract void Action();

    protected abstract void Flip(bool value, float scaleX, float scaleY);
}
