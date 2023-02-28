using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMat = spriteRenderer.material;
        duration = 0.09f;
    }

    public void Flash()
    {
        if (flashing != null)
        {
            StopCoroutine(flashing);
        }
        flashing = StartCoroutine(flashRoutine());
    }

    private IEnumerator flashRoutine()
    {
        spriteRenderer.material = flash;

        yield return new WaitForSeconds(duration);

        spriteRenderer.material = originalMat;

        flashing = null;
    }
}
