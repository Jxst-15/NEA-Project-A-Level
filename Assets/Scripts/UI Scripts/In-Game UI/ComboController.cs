using UnityEngine;

public class ComboController : UIController
{
    #region Fields
    #region Script References
    public PlayerCombat playerCombat;
    #endregion
    #endregion

    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }
    #endregion

    protected override void UpdateText()
    {
        text.text = ("x" + playerCombat._comboCount.ToString());
    }
}
