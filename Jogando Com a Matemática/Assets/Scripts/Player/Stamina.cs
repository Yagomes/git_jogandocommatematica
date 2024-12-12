using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private int timeBetweenStaminaRefresh = 3;

    private Transform staminaContainer;
    private int startingStamina = 3;
    private int maxStamina;
    private const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start()
    {
        FindStaminaContainer();
        UpdateStaminaImages();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reencontra o Stamina Container ao trocar de cena
        FindStaminaContainer();

        // Reseta a estamina se for necessário (como ao voltar para o menu)
        if (scene.name == "Menu" || scene.name == "Game")
        {
            ResetStamina();
        }
    }

    private void FindStaminaContainer()
    {
        GameObject container = GameObject.Find(STAMINA_CONTAINER_TEXT);

        if (container != null)
        {
            staminaContainer = container.transform;
        }
        else
        {
            Debug.LogError($"'{STAMINA_CONTAINER_TEXT}' não foi encontrado na cena. Certifique-se de que existe um GameObject com esse nome.");
        }
    }

    public void UseStamina()
    {
        if (CurrentStamina > 0)
        {
            CurrentStamina--;
            UpdateStaminaImages();
        }
    }

    public void RefreshStamina()
    {
        if (CurrentStamina < maxStamina)
        {
            CurrentStamina++;
            UpdateStaminaImages();
        }
    }

    public void ResetStamina()
    {
        CurrentStamina = startingStamina;
        UpdateStaminaImages();
    }

    public void AddRewardStamina(int amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + amount, 0, maxStamina);
        UpdateStaminaImages();
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }

    private void UpdateStaminaImages()
    {
        if (staminaContainer == null)
        {
            Debug.LogWarning("Stamina Container não está definido.");
            return;
        }

        for (int i = 0; i < maxStamina; i++)
        {
            if (i < staminaContainer.childCount)
            {
                var image = staminaContainer.GetChild(i).GetComponent<Image>();
                if (i <= CurrentStamina - 1)
                {
                    image.sprite = fullStaminaImage;
                }
                else
                {
                    image.sprite = emptyStaminaImage;
                }
            }
        }

        if (CurrentStamina < maxStamina)
        {
            StopAllCoroutines();
            StartCoroutine(RefreshStaminaRoutine());
        }
    }
}
