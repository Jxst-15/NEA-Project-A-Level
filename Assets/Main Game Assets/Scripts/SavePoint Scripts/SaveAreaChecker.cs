using System.Collections.Generic;
using UnityEngine;

public class SaveAreaChecker : MonoBehaviour
{
    #region Fields
    #region Script References
    private SavePointHandler savePoint;
    #endregion
    #endregion

    public List<GameObject> gameObjects;
    // Start is called before the first frame update
    void Start()
    {
        savePoint = GetComponentInParent<SavePointHandler>();
    }

    private void AdjustList(Collider2D entity, bool exit)
    {
        if (entity.gameObject.layer == LayerMask.NameToLayer("Enemy") && !gameObjects.Contains(entity.gameObject) && exit == false)
        {
            gameObjects.Add(entity.gameObject);
        }
        else if ((entity.gameObject.layer == LayerMask.NameToLayer("Enemy") && gameObjects.Contains(entity.gameObject) && exit == true))
        {
            gameObjects.Remove(entity.gameObject);
        }

        CheckList();
    }

    private void CheckList()
    {
        if (gameObjects.Count > 0)
        {
            savePoint.canSave = false;
        }
        else
        {
            savePoint.canSave = true;
        }
    }

    #region OnTrigger methods
    void OnTriggerEnter2D(Collider2D entity)
    {
        AdjustList(entity, false);
    }

    void OnTriggerStay2D(Collider2D entity)
    {
        // savePoint.canSave = CheckIfEnemyPresent(entity, false);

        AdjustList(entity, false);
    }

    private void OnTriggerExit2D(Collider2D entity)
    {
        // Debug.Log("Exit");
        
        // savePoint.canSave = CheckIfEnemyPresent(entity, true);

        AdjustList(entity, true);
    }

    #endregion
}
