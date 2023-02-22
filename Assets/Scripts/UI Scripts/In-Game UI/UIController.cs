using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    #region Text Reference
    [SerializeField] protected TMP_Text text;
    #endregion

    protected void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateText();
    }

    protected virtual void UpdateText() { }
}
