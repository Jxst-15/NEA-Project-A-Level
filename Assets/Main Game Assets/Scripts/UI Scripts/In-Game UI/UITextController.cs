using UnityEngine;
using TMPro;

public class UITextController : MonoBehaviour
{
    #region Fields
    #region Text Reference
    [SerializeField] protected TMP_Text text;
    #endregion
    #endregion

    #region Unity Methods
    protected virtual void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateText();
    }
    #endregion

    protected virtual void UpdateText() { }
}
