using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaTrigger : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    public GameObject battleArea;

    public GameObject bossWall;
    public GameObject eSpawner;

    public GameObject bossEnemy;
    #endregion

    #region Script references
    private BattleArea areaControl;

    private FinishMenu finishMenu;
    #endregion

    #region Variables
    [SerializeField] protected bool areaCleared;
    #endregion

    #region Getters and Setters
    protected bool activateArea
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    protected virtual void Start()
    {
        areaControl = battleArea.GetComponent<BattleArea>();

        finishMenu = GameObject.Find("UI Canvas").GetComponent<FinishMenu>();

        AreaController(false);
    }
    #endregion

    // Activates or deactivates the area
    protected virtual void AreaController(bool val)
    {
        bossWall.SetActive(val);

        eSpawner.SetActive(val);
    }

    // Detects whether or not the player has entered the trigger to activate the area
    #region OnTrigger Methods
    void OnTriggerEnter2D(Collider2D player)
    {
        if (areaCleared == false && activateArea == false)
        {
            if (player.gameObject.tag == "Player")
            {
                Debug.Log("Activated Area");
                activateArea = true;
                AreaController(activateArea);
            }
        }
    }
    #endregion
}
