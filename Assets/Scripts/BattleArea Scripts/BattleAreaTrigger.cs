using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAreaTrigger : MonoBehaviour
{
    #region Gameobjects
    public GameObject BWallL, BWallR;
    #endregion

    #region Variables
    [SerializeField] private bool areaCleared;
    private bool activateWalls;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        areaCleared = false;

        BWallL.SetActive(false);
        BWallR.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        activateWalls = true;
        if (areaCleared == false)
        {
            if (player.gameObject.tag == "Player")
            {
                Walls(activateWalls);
            }
        }
    }

    private void Walls(bool activateWalls)
    {
        if (activateWalls == true)
        {
            BWallL.SetActive(true);
            BWallR.SetActive(true);
        }
    }

}
