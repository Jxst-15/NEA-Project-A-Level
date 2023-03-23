using System.Collections;
using System.Collections.Generic;
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
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        UICanvas = GameObject.Find("UI Canvas");
        saveMenu = UICanvas.GetComponent<SaveMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    public void Interact()
    {
        saveMenu.sp = this;
        saveMenu.EnterSaveMenu();
    }
}
