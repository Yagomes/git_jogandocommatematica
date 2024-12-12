using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour // Posicao correta do player
{
    public Vector3 initialPosition; // Posição inicial do nível
    private Vector3 savedPosition;  // Posição salva ao trocar de subcenário
    public bool isResetNeeded = true;
 
    void Awake()
    {
        // Salva a posição inicial (posição do Player ao iniciar o jogo)
        initialPosition = transform.position;
    }

    void OnEnable()
    {
        // Subcreve ao evento que detecta quando uma cena é carregada
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Remove a assinatura do evento ao desativar o Player
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Salva a posição atual do Player (antes de trocar de cena).
    /// </summary>
    public void SavePosition()
    {
        savedPosition = transform.position;
    }

    /// <summary>
    /// Reseta o Player para a posição inicial.
    /// </summary>
    public void ResetToInitialPosition()
    {
        transform.position = initialPosition;
        isResetNeeded = false; // Indica que o reset foi realizado
    }

    /// <summary>
    /// Método chamado automaticamente ao carregar uma cena.
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isResetNeeded)
        {
            // Reseta o Player se for necessário
            ResetToInitialPosition();
        }
        else
        {
            // Caso não precise de reset, usa a posição salva
            transform.position = savedPosition;
        }
    }
}
