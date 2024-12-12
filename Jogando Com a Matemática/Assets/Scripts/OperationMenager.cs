using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

// Classe que representa cada tópico recebido do PHP
[System.Serializable]
public class Topico
{
    public string Nome_topico;
    public int Num_Min_topico;
    public int Num_Max_topico;
}

// Classe wrapper para suportar arrays no JsonUtility
[System.Serializable]
public class TopicoWrapper
{
    public Topico[] topicos;
}

public class OperationMenager : MonoBehaviour // Controla o botao de cada operacao.  
{
    public Button btnSoma;
    public Button btnMultiplicacao;

    private void Start()
    {
        StartCoroutine(FetchTopicos());
    }

    private IEnumerator FetchTopicos()
    {
        string url = "http://localhost/projeto/get_topicos.php";

        // Cria o formulário e adiciona o ID da turma salvo em PlayerPrefs
        WWWForm form = new WWWForm();
        int idTurma = PlayerPrefs.GetInt("id_turma", 0);
        form.AddField("id_turma", idTurma);

        Debug.Log("Enviando ID da turma: " + idTurma); // Log para depuração

        // Faz a requisição ao servidor
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        // Verifica se a requisição foi bem-sucedida
        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Resposta do servidor: " + jsonResponse); // Log da resposta do servidor

            // Processa a resposta JSON
            TopicoWrapper topicoWrapper = JsonUtility.FromJson<TopicoWrapper>(jsonResponse);

            // Desabilita os botões inicialmente
            btnSoma.interactable = false;
            btnMultiplicacao.interactable = false;

            // Habilita os botões com base nos tópicos recebidos
            foreach (Topico topico in topicoWrapper.topicos)
            {
                Debug.Log($"Tópico encontrado: {topico.Nome_topico}");

                // Salva os valores no PlayerPrefs
                PlayerPrefs.SetInt(topico.Nome_topico + "_Min", topico.Num_Min_topico);
                PlayerPrefs.SetInt(topico.Nome_topico + "_Max", topico.Num_Max_topico);

                // Habilita os botões correspondentes
                if (topico.Nome_topico.ToLower() == "soma")
                    btnSoma.interactable = true;

                if (topico.Nome_topico.ToLower() == "mult")
                    btnMultiplicacao.interactable = true;
            }
        }
        else
        {
            Debug.LogError("Erro ao buscar tópicos: " + www.error);
        }
    }

    public void OnClickSoma()
    {
        PlayerPrefs.SetString("TopicoEscolhido", "soma");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_S"); // Vai para a tela de soma
    }

    public void OnClickMultiplicacao()
    {
        PlayerPrefs.SetString("TopicoEscolhido", "mult");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_M"); // Vai para a tela de multiplicação
    }
}
