using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour // controla a visibilidade do canvas e troca de cena.
{
   
    public GameObject canvas;
    

    private void Update()
    {
        // Chamado toda vez que uma nova cena é carregada
        SceneManager.sceneLoaded += OnSceneLoaded;
        teste();
    }

    void OnDestroy()
    {
        // Remove o evento ao destruir o objeto
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateVisibility(); // Atualiza visibilidade ao trocar de cena
    }

    private void UpdateVisibility()
    {
        // Verifica a cena atual
        string currentScene = SceneManager.GetActiveScene().name;

        // Lista de cenas onde Player e Canvas devem ser desativados
        if (currentScene == "Tela_Nivel_S" || currentScene == "Tela_Nivel_M" || currentScene == "Tela_Nivel_D" || currentScene == "Tela_Nivel_Su" || currentScene == "Operacao" || currentScene == "Login" || currentScene == "Ranking") // Substitua "MenuScene" pelo nome real da sua cena de menu
        {
            canvas.SetActive(false);

        }
        else
        {
            canvas.SetActive(true);
        }

        // Lista de cenas onde Player e Canvas devem ser desativados
        if ( currentScene == "Operacao") 
        {
    
        // Valor salvo no PlayerPrefs
        string aluno_genero = PlayerPrefs.GetString("aluno_genero");

        GameObject player = null;

        // Procurando mesmo se estiver desativado e em qualquer cena (inclusive DontDestroy)
        GameObject[] allPlayers = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in allPlayers)
        {
            if (go.name == "Player_Boy" && aluno_genero == "Masculino" )
            {
                player = go;
                    
                    if (player != null) { Destroy(player);
                    }

                       
            }
            else if (go.name == "Player_Girl" && aluno_genero == "Feminino")
            {
                player = go;
                    if (player != null)
                    {
                        Destroy(player);
                    }
                }
        }
            GameObject cameracontroller = null;

            // impede que os objeto do jogo persiste fora da cena dos jogos
            GameObject[] allteste = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject go in allteste)
            {
                if (go.name == "Camera Controller")
                {
                    cameracontroller = go;
                    
                    Destroy(cameracontroller);

                }
            }

         

        /*
        // Agora setando o Follow da câmera normalmente
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = player.transform;*/
        }
}

    private void teste()
    {
        // Verifica a cena atual
        string currentScene = SceneManager.GetActiveScene().name;
        
        GameObject cameracontroller = null;

            // impede que os objeto do jogo persiste fora da cena dos jogos
            GameObject[] allteste = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject go in allteste)
            {
                if (go.name == "Active Inventory")
                {
                    cameracontroller = go;

                // Lista de cenas onde Player e Canvas devem ser desativados
                if (currentScene == "Tela_Nivel_S" || currentScene == "Tela_Nivel_M" || currentScene == "Tela_Nivel_D" || currentScene == "Tela_Nivel_Su" || currentScene == "Operacao" || currentScene == "Login" || currentScene == "Ranking") // Substitua "MenuScene" pelo nome real da sua cena de menu
                {

                    go.SetActive(false);
                }
                else
                {
                    go.SetActive(true);
                }


            }
            }

        

    }




    public void LoadScene(string sceneName)
    {

        if (sceneName == "Login")
        {
            EstatisticasManager.instance.SalvarEstatisticas();

            SceneManager.LoadScene(sceneName);
        }
        else { SceneManager.LoadScene(sceneName);  }
    }
}