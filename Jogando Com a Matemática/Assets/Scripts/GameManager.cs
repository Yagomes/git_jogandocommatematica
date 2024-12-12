using UnityEngine;
using UnityEngine.SceneManagement;  // Necessário para gerenciar as cenas no Unity

public class GameManager : Singleton<GameManager> // Gerencia a troca da tela do jogo para tela de nivel vise-versa
{
    public bool atu;


    // Método chamado quando o jogador pressiona o botão "Voltar" para retornar ao Menu
    public void BackToMenu_M()
    { atu = true;
            SceneManager.LoadScene("Tela_Nivel_M");  // Troque "Menu" pelo nome da sua cena de menu
       
    }

    public void BackToMenu_S()
    {
        atu = true;
        SceneManager.LoadScene("Tela_Nivel_S");  // Troque "Menu" pelo nome da sua cena de menu

    }

    // Método chamado quando o jogador inicia o jogo novamente (após o Menu)
    public void StartNewGame()
    {
        atu = true;
    }




}
