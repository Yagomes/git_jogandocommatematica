using UnityEngine;
using UnityEngine.SceneManagement;  // Necess�rio para gerenciar as cenas no Unity

public class GameManager : Singleton<GameManager> // Gerencia a troca de cenas e a��es do jogo, como destruir o jogador e retornar ao menu.
{
    public bool atu;

    public void DestroyPlayer()
    {
        if (PlayerHealth.Instance != null)
        {
            Destroy(PlayerHealth.Instance.gameObject);
        }
    }


    // M�todo chamado quando o jogador pressiona o bot�o "Voltar" para retornar ao Menu
    public void BackToMenu_M()
    {
        EstatisticasManager.instance.SalvarEstatisticas();

        atu = true;
        DestroyPlayer(); // Destroi o jogador antes de carregar a cena
        SceneManager.LoadScene("Tela_Nivel_M");
    }

    public void BackToMenu_S()
    {
        EstatisticasManager.instance.SalvarEstatisticas();

        atu = true;
        DestroyPlayer(); // Destroi o jogador antes de carregar a cena
        SceneManager.LoadScene("Tela_Nivel_S");
    }

    public void BackToMenu_D()
    {
        EstatisticasManager.instance.SalvarEstatisticas();

        atu = true;
        DestroyPlayer(); // Destroi o jogador antes de carregar a cena
        SceneManager.LoadScene("Tela_Nivel_D");
    }

    public void BackToMenu_Su()
    {
        EstatisticasManager.instance.SalvarEstatisticas();

        atu = true;
        DestroyPlayer(); // Destroi o jogador antes de carregar a cena
        SceneManager.LoadScene("Tela_Nivel_Su");
    }


    // M�todo chamado quando o jogador inicia o jogo novamente (ap�s o Menu)
    public void StartNewGame()
    {
        atu = true;
    }




}
