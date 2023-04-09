using UnityEngine;
using TMPro;

public abstract class UITextController : MonoBehaviour
{
    #region Fields
    #region Text Reference
    [SerializeField] protected TMP_Text text; // Text object
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
