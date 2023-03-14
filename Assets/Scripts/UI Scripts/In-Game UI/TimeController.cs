using System;
using UnityEngine;

public class TimeController : UIController
{
    #region Fields
    #region Getters and Setters
    public int time
    { get; set; }
    #endregion
    #endregion

    protected override void UpdateText()
    {
        time = Convert.ToInt32(Time.time);
        text.text = time.ToString();
    }
}
