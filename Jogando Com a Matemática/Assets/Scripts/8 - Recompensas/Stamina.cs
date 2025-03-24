using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private int timeBetweenStaminaRefresh = 3;

    private Transform staminaContainer;
    private int startingStamina = 3;
    private int maxStamina;
    private const string STAMINA_CONTAINER_TEXT = "Stamina Container";
    private GameObject container;

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
        StartCoroutine(UpdateStaminaImagesDelay());
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
        StartCoroutine(FindStaminaAfterDelay());

        // Reseta a estamina ao carregar cenas específicas
        if (scene.name == "Tela_Nivel_S" || scene.name == "Scene1_S_F" ||
            scene.name == "Tela_Nivel_M" || scene.name == "Scene2_S_F" ||
            scene.name == "Scene1_M_F" || scene.name == "Scene2_M_F" || scene.name == "Tela_Nivel_Su" || scene.name == "Scene1_Su_F" ||
            scene.name == "Tela_Nivel_D" || scene.name == "Scene2_Su_F" ||
            scene.name == "Scene1_D_F" || scene.name == "Scene2_D_F" )
        {
            ResetStamina();
        }
    }

    private IEnumerator FindStaminaAfterDelay()
    {
        yield return null; // Aguarda um frame
        FindStaminaContainer();
    }

    private IEnumerator UpdateStaminaImagesDelay()
    {
        yield return new WaitForSeconds(2f);
        UpdateStaminaImages();
    }

    
    private void FindStaminaContainer()
    {




        // Primeiro tenta achar no contexto normal da cena
        GameObject pai = GameObject.Find("UICanvas");

        if (pai == null)
        {
            // Se não encontrar na cena atual, procura em todos os objetos da hierarquia incluindo os que estão no DontDestroyOnLoad
            GameObject[] rootObjects = FindObjectsOfType<GameObject>(true);
            foreach (GameObject obj in rootObjects)
            {
                if (obj.name == "UICanvas")
                {
                    pai = obj;
                    break;
                }
            }
        }

        if (pai != null)
        {
            // Agora procura o "Stamina Container" dentro do UICanvas
            Transform staminaTransform = pai.transform.Find("Stamina Container");
            if (staminaTransform != null)
            {
                container = staminaTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("Stamina Container não encontrado dentro do UICanvas.");
            }
        }
        else
        {
            Debug.LogWarning("UICanvas não encontrado nem na cena nem no DontDestroyOnLoad.");
        }



        if (container != null)
        {
            staminaContainer = container.transform;
        }
        else
        {
            Debug.Log($"'{STAMINA_CONTAINER_TEXT}' não foi encontrado na cena. Certifique-se de que existe um GameObject com esse nome.");
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
