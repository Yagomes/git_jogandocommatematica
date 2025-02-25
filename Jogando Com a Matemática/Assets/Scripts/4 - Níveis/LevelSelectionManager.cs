using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ProgressoResponse
{
    public int nivel_max; // Conforme o JSON retornado pelo buscar_progresso.php
}

public class LevelSelectionManager : MonoBehaviour
{
    [Header("Botões dos Níveis")]
    // Insira os botões dos níveis na ordem (Nível 1, Nível 2, etc.)
    public List<Button> levelButtons;


    private string topicoEscolhido;
 
    int alunoId;  // Defina conforme o aluno logado
    string topico; // Pode ser "soma" ou "multiplicacao"
    void Start()
    {

        alunoId = PlayerPrefs.GetInt("id_Aluno", 0); 

        // Carrega o tópico escolhido salvo no PlayerPrefs
        topicoEscolhido = PlayerPrefs.GetString("TopicoEscolhido", "");

        if (string.IsNullOrEmpty(topicoEscolhido))
        {
            Debug.LogError("Tópico não foi configurado em PlayerPrefs!");
            return;
        }


        if (topicoEscolhido.ToLower() == "soma")
        {
            topico =  "soma"; 
        }
        else if (topicoEscolhido.ToLower() == "multiplicacao" || topicoEscolhido.ToLower() == "mult")
        {
            topico = "multiplicacao";

        }

        // Ao iniciar, busca o progresso do aluno para o tópico específico
        StartCoroutine(ServerManager.GetProgresso(alunoId, topico, OnProgressoReceived));
    }

    private void OnProgressoReceived(string response)
    {
        if (!string.IsNullOrEmpty(response))
        {
            Debug.Log("Resposta do servidor: " + response);

            // Converte a resposta JSON para o objeto ProgressoResponse
            ProgressoResponse progresso = JsonUtility.FromJson<ProgressoResponse>(response);
            int nivelMax = progresso.nivel_max;
            Debug.Log("Nível máximo desbloqueado para " + topico + ": " + nivelMax);

            AtualizarBotoes(nivelMax);
        }
        else
        {
            Debug.LogError("Não foi possível obter o progresso do aluno.");
        }
    }

    /// <summary>
    /// Atualiza os botões de seleção de níveis conforme o progresso.
    /// Habilita os níveis cujo número é menor ou igual ao nivelMax e desabilita os demais.
    /// </summary>
    /// <param name="nivelMax">O maior nível desbloqueado.</param>
    private void AtualizarBotoes(int nivelMax)
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            int nivelBotao = i + 1; // Supondo que o primeiro botão representa o nível 1, e assim por diante.
            levelButtons[i].interactable = (nivelBotao <= nivelMax);
        }
    }
}
