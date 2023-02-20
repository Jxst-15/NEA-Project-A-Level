public class PointsController : UIController
{
    protected override void UpdateText()
    {
        text.text = PlayerPoints.points.ToString();
    }
}
