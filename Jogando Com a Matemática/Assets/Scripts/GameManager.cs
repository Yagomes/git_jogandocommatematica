using UnityEngine;
using UnityEngine.SceneManagement;  // Necessário para gerenciar as cenas no Unity

public class GameManager : Singleton<GameManager>
{
    public bool atu;


    // Método chamado quando o jogador pressiona o botão "Voltar" para retornar ao Menu
    public void BackToMenu()
    { atu = true;
            SceneManager.LoadScene("Menu");  // Troque "Menu" pelo nome da sua cena de menu
       
    }

    // Método chamado quando o jogador inicia o jogo novamente (após o Menu)
    public void StartNewGame()
    {
        atu = true;
        SceneManager.LoadScene("Scene1");  // Carrega a cena do jogo
    }
}
