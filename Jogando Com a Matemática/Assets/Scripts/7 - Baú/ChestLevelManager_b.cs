using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestLevelManager_b : MonoBehaviour
{
    public static ChestLevelManager_b Instance;

   
    int totalChests ; // Ajuste esse valor conforme o nível

    private int chestsOpened = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Tenta carregar o total de baús salvos, se já existir no PlayerPrefs
            string sceneName = SceneManager.GetActiveScene().name;
            int savedTotalChests = PlayerPrefs.GetInt("TotalChests_" + sceneName, -1); // "-1" significa que não encontrou o valor
            if (savedTotalChests != -1)
            {
                totalChests = savedTotalChests;
            }

            // Verifica se já foi salvo o número de baús abertos
            chestsOpened = PlayerPrefs.GetInt("ChestsOpened_" + sceneName, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Chame este método quando um baú for aberto corretamente.
    /// </summary>
    public void ChestOpened(string topic)
    {
        // Incrementa o número de baús abertos
        chestsOpened++;

        // Salva o progresso
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("ChestsOpened_" + sceneName, chestsOpened);
        PlayerPrefs.Save();

        Debug.Log("Baú aberto! Total de baús abertos: " + chestsOpened + " de " + totalChests);

        if (chestsOpened >= totalChests)
        {
            LevelCompleted(topic);
        }
    }

    /// <summary>
    /// Método chamado quando todos os baús do nível foram abertos.
    /// Aqui você pode disparar a transição para o próximo nível e atualizar o progresso.
    /// </summary>
    private void LevelCompleted(string topic)
    {
        Debug.Log("Todos os baús foram abertos! Nível concluído para o tópico " + topic);

        // Aqui, por exemplo, você pode chamar o método que salva o progresso no servidor
        // (supondo que você já tenha um ProgressManager na cena)
        ProgressManager progressManager = FindObjectOfType<ProgressManager>();
        if (progressManager != null)
        {
            progressManager.NivelConcluido();
        }
        else
        {
            Debug.LogError("ProgressManager não encontrado na cena!");
        }
    }

    // Método para configurar o número total de baús do nível
    public void SetTotalChests(int newTotal)
    {
        chestsOpened = 0;
        totalChests = newTotal;
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("TotalChests_" + sceneName, newTotal); // Salva o valor de baús
        PlayerPrefs.Save();
    }
}
