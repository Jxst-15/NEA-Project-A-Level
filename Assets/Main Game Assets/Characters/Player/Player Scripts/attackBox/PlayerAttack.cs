using UnityEngine;

public class PlayerAttack : AttackBox
{
    #region Unity Methods
    private void Start()
    {
        toAttack = LayerMask.NameToLayer("Enemy");
    }
    #endregion
}
