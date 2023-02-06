using System;

public class TimerScript
{
    #region Getters and Setters
    public float duration
    { get; private set; }
    public float delay
    { get; private set; }
    #endregion

    public TimerScript(float duration, float delay)
    {
        this.duration = duration;
        this.delay = delay;
    }

}
