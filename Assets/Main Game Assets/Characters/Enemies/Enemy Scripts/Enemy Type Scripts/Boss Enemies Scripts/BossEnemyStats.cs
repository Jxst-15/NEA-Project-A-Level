using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyStats : EnemyStats
{
    #region 
    private FinishMenu finishMenu;
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
        finishMenu = GameObject.Find("UI Canvas").GetComponent<FinishMenu>();
    }

    protected override void Death()
    {
        finishMenu.finishedGame = true;
        base.Death();
    }
}