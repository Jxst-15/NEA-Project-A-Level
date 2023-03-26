using UnityEngine;

public abstract class CharStats : MonoBehaviour, IDamageable
{
    #region Fields
    #region Variables
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int maxStamina;

    protected float nextRegen;
    protected float regenCooldown;
    protected int toIncBy;

    protected float tillUnstun;
    protected float unstunCooldown;
    #endregion

    #region Getters and Setters
    public int lDmg
    { get; set; }
    public int hDmg
    { get; set; }
    public bool stun
    { get; set; }
    public int currentHealth
    { get; protected set; }
    public int currentStamina
    { get; protected set; }
    #endregion
    #endregion

    #region Unity Methods
    protected abstract void Awake();
    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetVariables();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (DeathCheck() == false)
        {
            StaminaRegen();
            if (stun == true)
            {
                WaitForUnstun();
            }
        }
        else
        {
            Death();
        }
    }
    #endregion

    protected virtual void SetVariables()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    // Parameters indicate the amount to affect stamina by and whether it is an increase or decrease
    public void AffectCurrentStamima(int stam, string incOrDec)
    {
        if (incOrDec == "dec")
        {
            currentStamina -= stam;
        }
        else if (incOrDec == "inc")
        {
            currentStamina += stam;
        }
        StaminaCheck();
    }

    // Runs a check to make sure stamina does not go above or below allowed levels
    protected void StaminaCheck()
    {
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        else if (currentStamina <= 0)
        {
            currentStamina = 0;
            if (stun != true)
            {
                // Stun the character
                Stun();
            }
        }
    }

    protected abstract void StaminaRegen();

    public virtual void TakeDamage(int dmg, bool weapon)
    {
        // currentHealth is affected by the damage given as the parameter
        currentHealth -= dmg;
    }

    public virtual void Stun()
    {
        stun = true;

        tillUnstun = Time.time + unstunCooldown;
    }

    protected abstract void WaitForUnstun();

    protected bool DeathCheck()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            return true;
        }
        return false;
    }

    // Runs when this game object has been defeated
    protected abstract void Death();
}
