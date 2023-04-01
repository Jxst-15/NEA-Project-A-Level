using UnityEngine;
using UnityEngine.UI;

public abstract class BarManager : MonoBehaviour
{
    #region Fields
    #region Class References
    private GameObject player;

    protected PlayerStats playerStats;

    public Slider barSlider;
    #endregion
    #endregion

    #region Unity Methods
    private void Start()
    {
        barSlider = GetComponent<Slider>();

        player = GameObject.FindWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();

        SetValues();
    }
    #endregion

    protected abstract void SetValues();

    public void SetBarVal(int val)
    {
        barSlider.value = val;
    }
}
