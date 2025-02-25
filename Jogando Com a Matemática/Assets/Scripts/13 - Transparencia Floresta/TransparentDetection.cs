using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TransparentDetection : MonoBehaviour // Script para detectar quando o jogador entra e sai de um trigger e
                                                  // fazer a transição de transparência de um SpriteRenderer ou Tilemap.
{
    [Range(0,1)]
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = .4f;

    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;

    public GameObject Top;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            if (spriteRenderer)
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));
            }
            else if(tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount));
            }
        }
    }


   /* private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if (spriteRenderer)
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));
            }
            else if (tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount));
            }
        }
    }*/


    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if (gameObject.activeInHierarchy) // Ensure GameObject is active
            {

                if (spriteRenderer)
                {
                    StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1f));
                }

                else if (tilemap)
                {
                    StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));
                }
            }
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetTransparency)
    {
        float elapseTime = 0;
        while(elapseTime < fadeTime)
        {
            elapseTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapseTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            yield return null;
        }
    }

    private IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetTransparency)
    {
        float elapseTime = 0;
        while (elapseTime < fadeTime)
        {
            elapseTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapseTime / fadeTime);
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, newAlpha);
            yield return null;
        }
    }

}


