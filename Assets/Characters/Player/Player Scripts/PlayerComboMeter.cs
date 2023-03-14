using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboMeter : MonoBehaviour
{
    public PlayerCombat playerCombat;

    private bool inCombo;
    private int comboSnapshot;
    public float comboTime
    { get; set; }

    void Awake()
    {
        playerCombat = GetComponent<PlayerCombat>();
        comboTime = 5f;
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

    public void ComboMeter()
    {
        // Combo meter will be configured here
        if (playerCombat._comboCount  >= 1 && inCombo == false)
        {
            inCombo = true;
            comboSnapshot = playerCombat._comboCount;
        }
    }

    private void ComboTimer()
    {
        // This timer runs until it reaches the comboTime
        if (comboTime >= Time.time + comboTime && playerCombat._comboCount == comboSnapshot || playerCombat._comboCount == 0)
        {
            inCombo = false;
        }
        // If no attacks that have dealt damage have been performed then the comboCount will be set to 0
        // Else update the timer to restart
    }
}
