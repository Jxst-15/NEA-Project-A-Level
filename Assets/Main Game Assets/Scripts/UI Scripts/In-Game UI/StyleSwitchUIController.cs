using System.Collections.Generic;
using UnityEngine;

public class StyleSwitchUIController : MonoBehaviour
{
    #region Fields
    // The circles for the UI representing which fighting style the player is in
    public GameObject StyleCirc1, StyleCirc2, StyleCirc3;

    // A dictionary holding the corresponding circle with the right fighting style
    private Dictionary<string, GameObject> StyleUI;

    // Stores the previous fighting style so its circle can be deactivated
    private string prevFightStyle;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        // Creates the dictionary
        StyleUI = new Dictionary<string, GameObject>()
        {
            {"Iron Fist", StyleCirc1},
            {"Boulder Style", StyleCirc2},
            {"Grass Style", StyleCirc3},
        };

        // Ensures all circles are inactive first      
        foreach (var kvp in StyleUI)
        {
            kvp.Value.SetActive(false);
        }

        prevFightStyle = PlayerStyleSwitch.fightStyle;

        // Whichever the current fighting style, activate its circle
        GameObject toActivate = StyleUI[prevFightStyle];
        toActivate.SetActive(true);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Doesn't run if player chooses the same fighting style
        if (prevFightStyle != PlayerStyleSwitch.fightStyle)
        {
            GameObject toActivate = StyleUI[PlayerStyleSwitch.fightStyle];
            
            // Activates the new circle
            toActivate.SetActive(true);

            GameObject toDeactivate = StyleUI[prevFightStyle];
            
            // Deavtivattes the old and logs the prevFightStyle as the current
            toDeactivate.SetActive(false);
            prevFightStyle = PlayerStyleSwitch.fightStyle;
        }
    }
    #endregion
}
