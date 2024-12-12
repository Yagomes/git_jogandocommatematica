using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour // Posicao correta do player
{
    public Vector3 initialPosition; // Posi��o inicial do n�vel
    private Vector3 savedPosition;  // Posi��o salva ao trocar de subcen�rio
    public bool isResetNeeded = true;
 
    void Awake()
    {
        // Salva a posi��o inicial (posi��o do Player ao iniciar o jogo)
        initialPosition = transform.position;
    }

    void OnEnable()
    {
        // Subcreve ao evento que detecta quando uma cena � carregada
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Remove a assinatura do evento ao desativar o Player
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Salva a posi��o atual do Player (antes de trocar de cena).
    /// </summary>
    public void SavePosition()
    {
        savedPosition = transform.position;
    }

    /// <summary>
    /// Reseta o Player para a posi��o inicial.
    /// </summary>
    public void ResetToInitialPosition()
    {
        transform.position = initialPosition;
        isResetNeeded = false; // Indica que o reset foi realizado
    }

    /// <summary>
    /// M�todo chamado automaticamente ao carregar uma cena.
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isResetNeeded)
        {
            // Reseta o Player se for necess�rio
            ResetToInitialPosition();
        }
        else
        {
            // Caso n�o precise de reset, usa a posi��o salva
            transform.position = savedPosition;
        }
    }
}
