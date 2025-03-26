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
        UpdateVisibility(); // Verifica a visibilidade ao iniciar
 
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

    public void UpdateVisibility()
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