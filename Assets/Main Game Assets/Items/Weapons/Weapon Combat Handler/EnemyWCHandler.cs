public class EnemyWCHandler : WeaponCombatHandler
{
    #region Fields
    public EnemyStats enemyStats;
    public EnemyCombat enemyCombat;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyCombat = GetComponent<EnemyCombat>();
    }
    #endregion

    public override void WeaponAttackLogic()
    {
        bool unique = false;
        bool lightAtk = false;

        enemyCombat.attacking = true;
        enemyCombat.parryable = true;

        int toDecBy = 0; // A stamina decrease
        System.Random rng = new System.Random();
        int randNum = rng.Next(1, 11);

        if (1 <= randNum && 6 <= randNum)
        {
            lightAtk = true;
            toDecBy = weaponScript.stamDecWLAtk;
        }
        else if (randNum == 7 || randNum == 8)
        {
            lightAtk = false;
            toDecBy = weaponScript.stamDecWHAtk;
        }
        else if (randNum == 9 || randNum == 10)
        {
            unique = true;
            toDecBy = weaponScript.stamDecWUEAtk;
        }
        WeaponAttack(lightAtk, unique, toDecBy);

        enemyCombat.attacking = false;
        enemyCombat.parryable = false;
    }

    protected override void UseNormalWeaponAtk(int toDecBy)
    {
        enemyStats.AffectCurrentStamima(toDecBy, "dec");
    }

    protected override void UseUniqueWeaponAtk(int toDecBy)
    {
        enemyStats.AffectCurrentStamima(toDecBy, "dec");
    }

}
