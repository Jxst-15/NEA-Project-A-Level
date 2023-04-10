using UnityEngine;

public class SavePointHandler : MonoBehaviour, IInteractable
{
    #region Fields
    #region Gameobject References
    public GameObject UICanvas;
    #endregion

    #region Script References
    public SaveMenu saveMenu;
    #endregion

    #region Getters and Setters
    public bool canSave
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        UICanvas = GameObject.Find("UI Canvas"); 
        saveMenu = UICanvas.GetComponent<SaveMenu>(); // The UI
        canSave = true;
    }
    #endregion

    public void Interact()
    {
        if (canSave == true)
        {
            Debug.Log("Accessed save point");
            saveMenu.EnterSaveMenu(); // Can now save
        }
        else
        {
            Debug.Log("Can't save now");
        }
    }
}
