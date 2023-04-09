using UnityEngine;
using UnityEngine.UI;

public abstract class BarManager : MonoBehaviour
{
    #region Fields
    #region Class References
    private GameObject player;

    protected PlayerStats playerStats;

    public Slider barSlider; // Used to show the value of the variable given in the child classes
    #endregion
    #endregion

    #region Unity Methods
    protected void Start()
    {
        barSlider = GetComponent<Slider>();

        player = GameObject.FindWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();

        SetValues();
    }

    protected abstract void Update();
    #endregion

    public void SetBarVal(int val)
    {
        barSlider.value = val;
    }
    protected abstract void SetValues();
}
