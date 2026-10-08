using System.Collections;
using UnityEngine;

public class EnemyFlash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float restoreMatTime = 0.2f;
    private Material defaultMat;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    public float GetRestoreMatTime()
    {
        return restoreMatTime;
    }

    public IEnumerator FlashRutine()
    {
        spriteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(restoreMatTime);
        spriteRenderer.material = defaultMat;
    }
}
