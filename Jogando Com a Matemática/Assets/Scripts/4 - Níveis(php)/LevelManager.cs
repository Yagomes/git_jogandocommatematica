using UnityEngine;

public class LevelManager : MonoBehaviour // Script que controla os n�veis do jogo, configurando os valores de dificuldade
                                          // (f�cil, m�dio e dif�cil) com base nos valores m�nimo e m�ximo definidos no PlayerPrefs.
                                         // Ele tamb�m gerencia a transi��o entre cenas, carregando a cena correspondente ao t�pico
                                         // (soma, multiplica��o, etc.) e � dificuldade selecionada.

{
    private int minValor;
    private int maxValor;

    public PlayerManager player;

    private string topicoEscolhido;

    //private SpriteRenderer playerSpriteRenderer;


    private void Start()
    {
        // recarrega as moedas caso for um novo login
        EconomyManager.Instance.CurrentGold();

        // Carrega o t�pico escolhido salvo no PlayerPrefs
        topicoEscolhido = PlayerPrefs.GetString("TopicoEscolhido", "");

        if (string.IsNullOrEmpty(topicoEscolhido))
        {
            Debug.LogError("T�pico n�o foi configurado em PlayerPrefs!");
            return;
        }

        // Carrega os valores m�nimo e m�ximo do PlayerPrefs
        minValor = PlayerPrefs.GetInt(topicoEscolhido + "_min", 0);
        maxValor = PlayerPrefs.GetInt(topicoEscolhido + "_max", 0);

        if (minValor == 0 && maxValor == 0)
        {
            Debug.LogError($"Os valores m�nimos e m�ximos para o t�pico '{topicoEscolhido}' n�o foram configurados!");
        }

        Debug.Log($"T�pico: {topicoEscolhido}, Min: {minValor}, Max: {maxValor}");
    }

    public void OnClickF�cil()
    {
        EstatisticasManager.instance.AdicionarJogo(); // jogou uma partida

        CameraController.Instance.SetPlayerCameraFollow();
        ChestLevelManager_b.Instance.SetTotalChests(1); // numero de baus do nivel facil

        int intervalo = (maxValor - minValor) / 3;
        PlayerPrefs.SetInt("NivelMin", minValor);
        PlayerPrefs.SetInt("NivelMax", minValor + intervalo);

        // Reseta o nome da transi��o ao iniciar um novo jogo
        SceneManagement.Instance.SetTransitionName("");


        CarregarCena("F");
        player.isResetNeeded = true;


      /*  // Procura todos os GameObjects na cena, incluindo DontDestroyOnLoad
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();


        foreach (GameObject obj in allObjects)
        {
            // Verifica se o nome � "Active Inventory"
            if (obj.name == "Active Inventory")
            {

                ActiveInventory script = obj.GetComponent<ActiveInventory>();
                if (script != null)
                {
                    script.enabled = true;
                    Debug.Log("Script 'Active Inventory' ativado com sucesso!");
                }
                else
                {
                    Debug.LogError("Script 'Active Inventory' n�o encontrado.");
                }

                break; // Interrompe o loop ap�s encontrar o objeto
            }
        }*/

        }

    public void OnClickM�dio()
    {
        EstatisticasManager.instance.AdicionarJogo(); // jogou uma partida

        CameraController.Instance.SetPlayerCameraFollow();

        ChestLevelManager_b.Instance.SetTotalChests(2); // numero de baus do nivel medio

        int intervalo = (maxValor - minValor) / 3;
        PlayerPrefs.SetInt("NivelMin", minValor + intervalo + 1);
        PlayerPrefs.SetInt("NivelMax", minValor + 2 * intervalo);
        
        // Reseta o nome da transi��o ao iniciar um novo jogo
        SceneManagement.Instance.SetTransitionName("");


        CarregarCena("M");
        player.isResetNeeded = true;


    }

    public void OnClickDif�cil()
    {
        EstatisticasManager.instance.AdicionarJogo(); // jogou uma partida


        CameraController.Instance.SetPlayerCameraFollow();

        ChestLevelManager_b.Instance.SetTotalChests(3); // numero de baus do nivel dificil

        int intervalo = (maxValor - minValor) / 3;
        PlayerPrefs.SetInt("NivelMin", minValor + 2 * intervalo + 1);
        PlayerPrefs.SetInt("NivelMax", maxValor);

        // Reseta o nome da transi��o ao iniciar um novo jogo
        SceneManagement.Instance.SetTransitionName("");


        CarregarCena("D");
        player.isResetNeeded = true;

      
    }

    public void Voltar() // serve para o ba�
    {

        player.isResetNeeded = true;

        if (GameManager_b.instance != null)
        {
            GameManager_b.instance.ResetChestStates();
        }

    }

    private void CarregarCena(string dificuldade)
    {
        if (GameManager_b.instance != null)
        {
            GameManager_b.instance.ResetChestStates();
        }

        // Define a cena com base no t�pico escolhido e na dificuldade
        string cena = "";

        if (topicoEscolhido.ToLower() == "soma")
        {
            //playerSpriteRenderer.enabled = true;
            cena = $"Scene1_S_{dificuldade}";


            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                // Verifica se o nome � "Active Inventory"
                if (obj.name == "tela_s")
                {
                    obj.SetActive(true);
                }
            }
        }
        else if (topicoEscolhido.ToLower() == "multiplicacao" || topicoEscolhido.ToLower() == "mult")
        {


            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                // Verifica se o nome � "Active Inventory"
                if (obj.name == "tela_m")
                {
                    obj.SetActive(true);
                }
            } 

            //  playerSpriteRenderer.enabled = true;
            cena = $"Scene1_M_{dificuldade}";

        }
        else if (topicoEscolhido.ToLower() == "subtracao" || topicoEscolhido.ToLower() == "sub")
        {


            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                // Verifica se o nome � "Active Inventory"
                if (obj.name == "tela_su")
                {
                    obj.SetActive(true);
                }
            }

            //  playerSpriteRenderer.enabled = true;
            cena = $"Scene1_Su_{dificuldade}";

        }
        else if (topicoEscolhido.ToLower() == "divisao" || topicoEscolhido.ToLower() == "div")
        {


            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                // Verifica se o nome � "Active Inventory"
                if (obj.name == "tela_d")
                {
                    obj.SetActive(true);
                }
            }

            //  playerSpriteRenderer.enabled = true;
            cena = $"Scene1_D_{dificuldade}";

        }
        else
        {
            Debug.LogError("T�pico desconhecido: " + topicoEscolhido);
            return;
        }

        Debug.Log($"Carregando cena: {cena}");
        UnityEngine.SceneManagement.SceneManager.LoadScene(cena);
    }
}

