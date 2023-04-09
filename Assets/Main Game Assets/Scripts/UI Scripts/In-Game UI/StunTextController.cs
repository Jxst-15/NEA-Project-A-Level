using UnityEngine;

public class StunTextController : UITextController
{
    #region Fields
    #region Script References
    private PlayerStats playerStats;
    #endregion
    #endregion

    protected override void Awake()
    {
        base.Awake();
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    protected override void UpdateText()
    {
        if (playerStats.stun == true)
        {
            text.text = "-STUNNED-";
        }
        else
        {
            text.text = "";
        }
    }
}
