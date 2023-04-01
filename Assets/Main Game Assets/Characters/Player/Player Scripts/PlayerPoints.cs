using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    #region Fields
    #region Getters and Setters
    public static int points
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        points = 0;
    }
    #endregion

    public void ChangePoints(int pointsToChangeBy, string incOrDec)
    {
        if (incOrDec == "inc")
        {
            points += pointsToChangeBy;
        }
        else if (incOrDec == "dec")
        {
            points-= pointsToChangeBy;
        }
        CheckPoints();
    }

    private void CheckPoints()
    {
        if (points < 0)
        {
            points = 0;
        }
    }
}
