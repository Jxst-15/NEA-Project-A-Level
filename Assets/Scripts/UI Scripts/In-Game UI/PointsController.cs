public class PointsController : UITextController
{
    protected override void UpdateText()
    {
        text.text = PlayerPoints.points.ToString();
    }
}
