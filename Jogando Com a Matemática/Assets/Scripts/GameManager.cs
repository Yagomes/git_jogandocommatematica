using UnityEngine;
using UnityEngine.SceneManagement;  // Necess�rio para gerenciar as cenas no Unity

public class GameManager : Singleton<GameManager>
{
    public bool atu;


    // M�todo chamado quando o jogador pressiona o bot�o "Voltar" para retornar ao Menu
    public void BackToMenu()
    { atu = true;
            SceneManager.LoadScene("Menu");  // Troque "Menu" pelo nome da sua cena de menu
       
    }

    // M�todo chamado quando o jogador inicia o jogo novamente (ap�s o Menu)
    public void StartNewGame()
    {
        atu = true;
        SceneManager.LoadScene("Scene1");  // Carrega a cena do jogo
    }
}
