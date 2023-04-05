using UnityEngine;

public class ColourButtonHandler : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    public GameObject DefBtn;
    public GameObject RedBtn;
    public GameObject GreenBtn;
    public GameObject PurpBtn;
    #endregion
    #endregion

    public void PressRed()
    {
        PlayerData.instance.colour = "Red";
        Debug.Log(PlayerData.instance.colour);
    }

    public void PressDefault()
    {
        PlayerData.instance.colour = "Default";
        Debug.Log(PlayerData.instance.colour);
    }

    public void PressGreen()
    {
        PlayerData.instance.colour = "Green";
        Debug.Log(PlayerData.instance.colour);
    }

    public void PressPurp()
    {
        PlayerData.instance.colour = "Purple";
        Debug.Log(PlayerData.instance.colour);
    }
}
