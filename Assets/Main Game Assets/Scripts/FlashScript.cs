using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashScript : MonoBehaviour
{
    #region Variables
    private SpriteRenderer spriteRenderer;
    private Material originalMat;
    [SerializeField] private Material flash;

    [SerializeField] private float duration;

    private Coroutine flashing;
    #endregion

    public List<Material> flashMaterials = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMat = spriteRenderer.material;
        duration = 0.09f;
    }

    public void Flash(Material flash)
    {
        if (flashing != null)
        {
            StopCoroutine(flashing);
        }
        flashing = StartCoroutine(flashRoutine(flash));
    }

    private IEnumerator flashRoutine(Material flash)
    {
        spriteRenderer.material = flash;

        yield return new WaitForSeconds(duration);

        spriteRenderer.material = originalMat;

        flashing = null;
    }

    public Material GetFlashMaterial(int index)
    {
        Material flashMaterial = null;
        
        if (index < flashMaterials.Count - 1 && index >= 0)
        {
            flashMaterial = flashMaterials[index];
        }
        
        return flashMaterial;
    }
}
