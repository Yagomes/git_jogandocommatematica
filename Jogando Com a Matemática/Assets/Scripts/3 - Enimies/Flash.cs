using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour // Controla o efeito de flash no inimigo, alterando sua cor temporariamente.
{
    [SerializeField] private Material whiteFlashMaterial;
    [SerializeField] private float restoreDefaultMatTime = .2f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;
  
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    public float GetRestoreMatTime() 
    {
        return restoreDefaultMatTime;
    }

    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = whiteFlashMaterial;
        yield return new WaitForSeconds(restoreDefaultMatTime);
        spriteRenderer.material = defaultMat;
    }
}
