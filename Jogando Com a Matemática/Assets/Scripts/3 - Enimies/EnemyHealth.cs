using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour // Controla a vida do inimigo, incluindo dano, recuo e morte com efeitos visuais e drops.
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;
    

    private int currentHealth;
    private Knockback Knockback;
    private Flash flash;


    private void Awake()
    {
        flash = GetComponent<Flash>();
        Knockback = GetComponent<Knockback>();
    }
    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDemage(int damage)
    {
        currentHealth -= damage;
        Knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDealthRoutine());
    }

    private IEnumerator CheckDetectDealthRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            EstatisticasManager.instance.AdicionarInimigoDerrotado(); // matou um inimigo

            GetComponent<PickUpSpawner>().DropItems();
            Destroy(gameObject);
        }
    }
}
