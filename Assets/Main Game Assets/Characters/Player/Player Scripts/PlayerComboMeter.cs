using UnityEngine;

public class PlayerComboMeter : MonoBehaviour
{
    #region Fields
    #region Script Reference
    public PlayerCombat playerCombat;
    #endregion

    #region Variables
    private int comboSnapshot;
    private float comboEndTime;
    #endregion

    #region Getters and Setters
    public float comboDuration
    { get; set; }
    public bool inCombo
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    void Awake()
    {
        playerCombat = GetComponent<PlayerCombat>();
        comboDuration = 5f;
        comboSnapshot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (inCombo == true)
        {
            ComboTimer();
        }
    }
    #endregion

    public void ComboStart()
    {
        // Combo meter will be configured here
        if (playerCombat._comboCount >= 1 && inCombo == false)
        {
            inCombo = true;
            comboSnapshot = playerCombat._comboCount;
            comboEndTime = Time.time + comboDuration;
        }
    }

    public void ResetCombo()
    {
        playerCombat._comboCount = 0;
        inCombo = false;
    }

    private void ComboTimer()
    {
        // If timer has run out then reset combo and set no longer in combo
        if (Time.time >= comboEndTime)
        {
            ResetCombo();
        }
        else 
        {
            // Check if there has been any changes in the combo count
            if (playerCombat._comboCount != comboSnapshot)
            {
                // If there are changes in combo count then keep the timer going and set new timer
                comboEndTime = Time.time + comboDuration;
                comboSnapshot = playerCombat._comboCount;
            }
        }
    }
}
