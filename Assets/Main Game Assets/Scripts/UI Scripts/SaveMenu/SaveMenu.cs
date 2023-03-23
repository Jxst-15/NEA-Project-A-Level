using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    #region Fields
    #region Gameobject References
    public GameObject gameUI;
    public GameObject backgroundObject;
    public GameObject saveMenuObject;
    #endregion

    #region Script References

    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        saveMenuObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}
