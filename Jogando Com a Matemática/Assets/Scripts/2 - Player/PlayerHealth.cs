using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHealth : Singleton<PlayerHealth> // Gerencia a vida do jogador, incluindo dano, cura e morte.

{
    public bool isDead { get; private set; }
    [SerializeField] public int maxHealth = 3;  // Defina o valor m�ximo de vida
    [SerializeField] public float knockBackThrustAmount = 10f;
    [SerializeField] public float damageRecoveryTime = 1f;

    private Slider healthSlider;
    public int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    public bool atu; // Flag para indicar quando restaurar a sa�de
   

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    //const string TOWN_TEXT = "Menu";  // Troque "Scene1" pelo nome da sua cena inicial
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;  // Inicializa a sa�de do jogador
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
         
    }

    private void Start()
    {
        isDead = false;
        UpdateHealthSlider();  // Atualiza a barra de sa�de no in�cio
         
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.atu)
        {
            RestoreHealthToMax();

            GameManager.Instance.atu = false;  // Desmarcar 'atu' para n�o restaurar novamente at� uma nova ativa��o
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

      /*  // Verificando se o 'atu' est� ativado no GameManager
        if (GameManager.Instance != null && GameManager.Instance.atu)
        {
            Debug.Log("PlayerHealth: TakeDamage - 'atu' est� ativado no GameController.");
            RestoreHealthToMax(); 
           
            GameManager.Instance.atu = false;  // Desmarcar 'atu' para n�o restaurar novamente at� uma nova ativa��o
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
            UpdateHealthSlider();  // Atualiza a barra de sa�de
            CheckIfPlayerDeath();
        
        
    }


    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
           
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;  // Garante que a sa�de seja 0 ao morrer
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    public IEnumerator DeathLoadSceneRoutine()
    {
        EstatisticasManager.instance.SalvarEstatisticas();

        yield return new WaitForSeconds(2f);  // Espera um tempo para anima��o de morte

        string topicoEscolhido = PlayerPrefs.GetString("TopicoEscolhido", "");

        Destroy(gameObject);

        if(topicoEscolhido == "soma")
        {
            if (GameManager_b.instance != null)
            {
                GameManager_b.instance.ResetChestStates();
            }

            SceneManager.LoadScene("Tela_Nivel_S");
        }
        if (topicoEscolhido == "mult" || topicoEscolhido == "multiplicacao")
        {
            if (GameManager_b.instance != null)
            {
                GameManager_b.instance.ResetChestStates();
            }
            SceneManager.LoadScene("Tela_Nivel_M");
        }
        if (topicoEscolhido == "sub" || topicoEscolhido == "subtracao")
        {
            if (GameManager_b.instance != null)
            {
                GameManager_b.instance.ResetChestStates();
            }
            SceneManager.LoadScene("Tela_Nivel_Su");
        }
        if (topicoEscolhido == "div" || topicoEscolhido == "divisao")
        {
            if (GameManager_b.instance != null)
            {
                GameManager_b.instance.ResetChestStates();
            }
            SceneManager.LoadScene("Tela_Nivel_D");
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
            // Primeiro tenta achar diretamente na cena
            GameObject sliderObj = GameObject.Find(HEALTH_SLIDER_TEXT);

            if (sliderObj == null)
            {
                // Se n�o encontrar, busca em todos os objetos incluindo os do DontDestroyOnLoad
                GameObject[] rootObjects = FindObjectsOfType<GameObject>(true);
                foreach (GameObject obj in rootObjects)
                {
                    if (obj.name == HEALTH_SLIDER_TEXT)
                    {
                        sliderObj = obj;
                        break;
                    }
                }
            }

            if (sliderObj != null)
            {
                healthSlider = sliderObj.GetComponent<Slider>();
            }
            else
            {
                Debug.LogWarning("Health Slider n�o encontrado nem na cena nem no DontDestroyOnLoad.");
            }
        }


        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;  // Atualiza a barra de sa�de com o valor atual de vida
         
    }

    // M�todo para restaurar a sa�de para o valor m�ximo
    public void RestoreHealthToMax()
    {
        atu = true;
         
        currentHealth = maxHealth;  // Restaura a sa�de para o m�ximo
        UpdateHealthSlider();  // Atualiza a barra de sa�de
        
    }
}
