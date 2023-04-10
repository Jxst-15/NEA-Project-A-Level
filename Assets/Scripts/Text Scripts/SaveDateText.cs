public class SaveDateText : UITextController
{
    protected override void UpdateText()
    {
        text.text = "Last Save Date: " + PlayerData.instance.lastSaveDate;
    }
}
