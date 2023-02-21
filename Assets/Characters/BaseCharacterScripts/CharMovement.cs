using UnityEngine;

// WIP
public abstract class CharMovement : MonoBehaviour
{
    #region Variables
    [SerializeField] protected int vSpeed, vRunSpeed, hSpeed, hRunSpeed;
    
    protected bool facingRight;
    protected float scaleX, scaleY;

    [SerializeField] protected bool isRunning;

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

    protected virtual void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        SetVariables();
    }

    protected abstract void Update();

    protected abstract void FixedUpdate();

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
