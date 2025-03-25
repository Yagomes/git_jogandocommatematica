using System.Collections;
using UnityEngine;

/* 
 * 
 // Supondo que você tenha uma referência ao ProgressManager

ProgressManager progressManager = FindObjectOfType<ProgressManager>();
if (progressManager != null)
{
    progressManager.NivelConcluido();
}

*/

public class ProgressManager : MonoBehaviour
{
   
    

    [Header("Progresso")]
    public int nivelAtual = 1; // Nível atual concluído

    
    int alunoId; // Substitua pelo ID real do aluno
    string topico; // Pode ser "soma" ou "multiplicacao", conforme o contexto
    
    private string topicoEscolhido;

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
            topico = "soma";
        }
        if (topicoEscolhido.ToLower() == "multiplicacao" || topicoEscolhido.ToLower() == "mult")
        {
            topico = "multiplicacao";

        }
        if (topicoEscolhido.ToLower() == "div" || topicoEscolhido.ToLower() == "divisao")
        {
            topico = "divisao";
        }
        else if (topicoEscolhido.ToLower() == "sub" || topicoEscolhido.ToLower() == "subtracao")
        {
            topico = "subtracao";

        }
    }

        // Chama este método quando o jogador concluir um nível.
        public void SalvarProgresso()
    {
        Debug.Log("Salvando progresso: Aluno " + alunoId + " - Tópico: " + topico + " - Nível " + nivelAtual);
        StartCoroutine(ServerManager.PostProgresso(alunoId, nivelAtual, topico, OnProgressoSalvo));
    }

    // Callback para tratar a resposta do servidor
    private void OnProgressoSalvo(string response)
    {
        if (!string.IsNullOrEmpty(response))
        {
            Debug.Log("Resposta do servidor: " + response);

            // Converte a resposta JSON para um objeto
            var jsonResponse = JsonUtility.FromJson<ProgressoResponse>(response);

            if (jsonResponse.status == "sucesso")
            {
                Debug.Log("Progresso salvo com sucesso! Desbloqueando o próximo nível...");
                EstatisticasManager.instance.AdicionarNiveisDesbloqueados(); // desbloqueou um nível

                // Aqui você pode executar ações adicionais, como desbloquear o próximo nível

            }
            else if (jsonResponse.status == "ja_registrado")
            {
                Debug.Log("Esse nível já foi concluído anteriormente.");
                // Aqui você pode dar um feedback ao jogador, como mostrar uma mensagem na UI
                ;
            }
            else
            {
                Debug.LogError("Falha ao salvar o progresso!");
                
            }
        }
        else
        {
            Debug.LogError("Falha ao se comunicar com o servidor!");
        }
    }

    // Classe auxiliar para mapear a resposta do servidor
    [System.Serializable]
    public class ProgressoResponse
    {
        public string status;
    }

  

    // Exemplo de método para simular a conclusão de um nível (pode ser chamado, por exemplo, ao abrir o último baú)
    public void NivelConcluido()
    {
        // Atualiza o nível concluído (ajuste conforme sua lógica de progressão)
        nivelAtual++;
        // Salva o progresso no servidor
        SalvarProgresso();
    }
}
