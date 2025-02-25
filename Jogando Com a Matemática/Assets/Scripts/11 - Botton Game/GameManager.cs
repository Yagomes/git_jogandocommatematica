using UnityEngine;
using UnityEngine.SceneManagement;  // Necessário para gerenciar as cenas no Unity

public class GameManager : Singleton<GameManager> // Gerencia a troca de cenas e ações do jogo, como destruir o jogador e retornar ao menu.
{
    public bool atu;

    public void DestroyPlayer()
    {
        if (PlayerHealth.Instance != null)
        {
            Destroy(PlayerHealth.Instance.gameObject);
        }
    }


    // Método chamado quando o jogador pressiona o botão "Voltar" para retornar ao Menu
    public void BackToMenu_M()
    {
        atu = true;
        DestroyPlayer(); // Destroi o jogador antes de carregar a cena
        SceneManager.LoadScene("Tela_Nivel_M");
    }

    public void BackToMenu_S()
    {
        atu = true;
        DestroyPlayer(); // Destroi o jogador antes de carregar a cena
        SceneManager.LoadScene("Tela_Nivel_S");
    }


    // Método chamado quando o jogador inicia o jogo novamente (após o Menu)
    public void StartNewGame()
    {
        atu = true;
    }




}
