using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Content;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }
    [SerializeField] public int maxHealth = 3;  // Defina o valor máximo de vida
    [SerializeField] public float knockBackThrustAmount = 10f;
    [SerializeField] public float damageRecoveryTime = 1f;

    private Slider healthSlider;
    public int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    public bool atu; // Flag para indicar quando restaurar a saúde
   

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    //const string TOWN_TEXT = "Menu";  // Troque "Scene1" pelo nome da sua cena inicial
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;  // Inicializa a saúde do jogador
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
         
    }

    private void Start()
    {
        isDead = false;
        UpdateHealthSlider();  // Atualiza a barra de saúde no início
         
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.atu)
        {
            RestoreHealthToMax();

            GameManager.Instance.atu = false;  // Desmarcar 'atu' para não restaurar novamente até uma nova ativação
        }
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAi enemy = other.gameObject.GetComponent<EnemyAi>();

        if (enemy)
        {
           
            TakeDamage(1, other.transform);  // O jogador recebe dano ao colidir com inimigos
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
          
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

      /*  // Verificando se o 'atu' está ativado no GameManager
        if (GameManager.Instance != null && GameManager.Instance.atu)
        {
            Debug.Log("PlayerHealth: TakeDamage - 'atu' está ativado no GameController.");
            RestoreHealthToMax(); 
           
            GameManager.Instance.atu = false;  // Desmarcar 'atu' para não restaurar novamente até uma nova ativação
        }
        else
            */
            // Processamento normal do dano
           
            ScreenShakeManager.Instance.ShakeScreen();
            knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
            StartCoroutine(flash.FlashRoutine());
            canTakeDamage = false;
            currentHealth -= damageAmount;  // Diminui a vida do jogador
           
            StartCoroutine(DamageRecoveryRoutine());
            UpdateHealthSlider();  // Atualiza a barra de saúde
            CheckIfPlayerDeath();
        
        
    }


    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
           
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;  // Garante que a saúde seja 0 ao morrer
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
       
        yield return new WaitForSeconds(2f);  // Espera um tempo para animação de morte

        string topicoEscolhido = PlayerPrefs.GetString("TopicoEscolhido", "");

        Destroy(gameObject);

        if(topicoEscolhido == "soma")
        {
           SceneManager.LoadScene("Tela_Nivel_S");
        }
        if (topicoEscolhido == "mult")
        {
            SceneManager.LoadScene("Tela_Nivel_M");
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        
        yield return new WaitForSeconds(damageRecoveryTime);  // Delay para permitir que o jogador receba dano novamente
        canTakeDamage = true;
         
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
            
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;  // Atualiza a barra de saúde com o valor atual de vida
         
    }

    // Método para restaurar a saúde para o valor máximo
    public void RestoreHealthToMax()
    {
        atu = true;
         
        currentHealth = maxHealth;  // Restaura a saúde para o máximo
        UpdateHealthSlider();  // Atualiza a barra de saúde
        
    }
}
