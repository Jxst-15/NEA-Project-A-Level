using System;
using UnityEngine;

public class TimeController : UIController
{
    #region Getters and Setters
    public int time
    { get; set; }
    #endregion

    protected override void UpdateText()
    {
        time = Convert.ToInt32(Time.time);
        text.text = time.ToString();
    }
}
