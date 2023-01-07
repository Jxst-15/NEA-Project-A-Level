using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    #region Variables
    [SerializeField] private int points;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
    }

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
    }
}
