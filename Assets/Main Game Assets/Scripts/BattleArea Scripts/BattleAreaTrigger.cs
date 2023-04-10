using UnityEngine;

public class BattleAreaTrigger : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    public GameObject battleArea;

    public GameObject bWallL, bWallR;
    public GameObject eSpawnerL, eSpawnerR;
    #endregion

    #region Script references
    private BattleArea areaControl;
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

        AreaController(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (eSpawnerL != null && eSpawnerR != null)
        {
            CheckIfAreaFinished();
        }
    }
    #endregion

    protected virtual void CheckIfAreaFinished()
    {
        if (eSpawnerL.GetComponent<BattleAreaEnemySpawner>().spawning == false && eSpawnerR.GetComponent<BattleAreaEnemySpawner>().spawning == false)
        {
            if (areaControl.enemies.Count <= 0 && activateArea == true)
            {
                activateArea = false;
                areaCleared = true;
                AreaController(activateArea);
                Debug.Log("Area Cleared");
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }

    // Activates or deactivates the area
    protected virtual void AreaController(bool val)
    {
        bWallL.SetActive(val);
        bWallR.SetActive(val);

        eSpawnerL.SetActive(val);
        eSpawnerR.SetActive(val);
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
