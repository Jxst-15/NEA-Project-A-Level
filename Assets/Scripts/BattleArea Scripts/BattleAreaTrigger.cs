using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAreaTrigger : MonoBehaviour
{
    #region Gameobjects
    public GameObject bWallL, bWallR;
    public GameObject eSpawnerL, eSpawnerR;
    #endregion

    #region Variables
    [SerializeField] private bool areaCleared;
    private bool activateArea;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        areaCleared = false;

        bWallL.SetActive(false);
        bWallR.SetActive(false);

        eSpawnerL.SetActive(false);
        eSpawnerR.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if (areaCleared == false && activateArea == false)
        {
            if (player.gameObject.tag == "Player")
            {
                activateArea = true;
                ActivateArea(activateArea);
            }
        }
    }

    private void ActivateArea(bool activateWalls)
    {
        if (activateArea == true)
        {
            bWallL.SetActive(true);
            bWallR.SetActive(true);

            eSpawnerL.SetActive(true);
            eSpawnerR.SetActive(true);
        }
    }

}
