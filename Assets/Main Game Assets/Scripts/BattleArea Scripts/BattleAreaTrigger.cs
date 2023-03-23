using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAreaTrigger : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    public GameObject bWallL, bWallR;
    public GameObject eSpawnerL, eSpawnerR;
    #endregion

    #region Variables
    [SerializeField] private bool areaCleared;
    #endregion

    #region Getters and Setters
    private bool activateArea
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        areaCleared = false;

        AreaController(areaCleared);
    }

    // Update is called once per frame
    void Update()
    {
        if (areaCleared == true)
        {
            activateArea = false;
            AreaController(activateArea);
        }
    }
    #endregion

    private void AreaController(bool val)
    {
        bWallL.SetActive(val);
        bWallR.SetActive(val);

        eSpawnerL.SetActive(val);
        eSpawnerR.SetActive(val);
    }

    #region OnTrigger Methods
    void OnTriggerEnter2D(Collider2D player)
    {
        if (areaCleared == false && activateArea == false)
        {
            if (player.gameObject.tag == "Player")
            {
                activateArea = true;
                AreaController(activateArea);
            }
        }
    }
    #endregion
}
