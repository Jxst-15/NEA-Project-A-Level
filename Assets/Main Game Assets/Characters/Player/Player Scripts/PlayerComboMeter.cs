using UnityEngine;

public class PlayerComboMeter : MonoBehaviour, IComboMeter
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
    public int highestCombo
    { get; set; }
    #endregion

    private const int comboForFinisher = 6;
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

    public void ComboStart(int val)
    {
        inCombo = true;
        comboSnapshot = val;
        comboEndTime = Time.time + comboDuration;
    }

    public void ResetCombo()
    {
        CheckIfHighest();
        
        playerCombat.comboCount = 0;

        inCombo = false;
    }

    private void CheckIfHighest()
    {
        if (playerCombat.comboCount > highestCombo)
        {
            highestCombo = playerCombat.comboCount;
        }
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
            if (playerCombat.comboCount != comboSnapshot)
            {
                // If there are changes in combo count then keep the timer going and extend the timer
                ComboStart(playerCombat.comboCount);
            }
        }
    }

    // Combo for finisher
    public int GetComboForF()
    {
        return comboForFinisher;
    }
}
