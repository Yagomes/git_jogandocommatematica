using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic; // Adicione esta linha


// Classe que representa cada tópico recebido do PHP
[System.Serializable]
public class Topico
{
    public string topico_nome;
    public int topico_num_min;
    public int topico_num_max;
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
    public Button btnSubtracao;
    public Button btnDivisao;

    private void Start()
    {
        StartCoroutine(FetchTopicos());
    }

    private IEnumerator FetchTopicos()
    {
        string url = "http://localhost/projeto/get_topicos.php";

        // Cria o formulário e adiciona o ID da turma salvo em PlayerPrefs
        WWWForm form = new WWWForm();
        int turma_id = PlayerPrefs.GetInt("turma_id", 0);
        form.AddField("turma_id", turma_id);

        Debug.Log("Enviando ID da turma: " + turma_id); // Log para depuração

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

            HashSet<string> topicosUnicos = new HashSet<string>();

            foreach (Topico topico in topicoWrapper.topicos)
            {
                topicosUnicos.Add(topico.topico_nome.ToLower()); // Adiciona ao HashSet para garantir unicidade
            }

            // Salva a quantidade de tópicos únicos
            PlayerPrefs.SetInt("TotalTopicos", topicosUnicos.Count);
            Debug.Log("Total de tópicos únicos recebidos: " + topicosUnicos.Count);


            // Desabilita os botões inicialmente
            btnSoma.interactable = false;
            btnMultiplicacao.interactable = false;
            btnSubtracao.interactable = false;
            btnDivisao.interactable = false;

            // Habilita os botões com base nos tópicos recebidos
            foreach (Topico topico in topicoWrapper.topicos)
            {

                Debug.Log($"Tópico encontrado: {topico.topico_nome}");

                // Salva os valores no PlayerPrefs
                PlayerPrefs.SetInt(topico.topico_nome + "_min", topico.topico_num_min);
                PlayerPrefs.SetInt(topico.topico_nome + "_max", topico.topico_num_max);

                // Habilita os botões correspondentes
                if (topico.topico_nome.ToLower() == "soma")
                    btnSoma.interactable = true;

                if (topico.topico_nome.ToLower() == "mult" || topico.topico_nome.ToLower() == "multiplicacao")
                    btnMultiplicacao.interactable = true;

                if (topico.topico_nome.ToLower() == "sub" || topico.topico_nome.ToLower() == "subtracao")
                    btnSubtracao.interactable = true;

                if (topico.topico_nome.ToLower() == "div" || topico.topico_nome.ToLower() == "divisao")
                    btnDivisao.interactable = true;
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
        PlayerPrefs.SetString("TopicoEscolhido", "multiplicacao");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_M"); // Vai para a tela de multiplicação
    }

    public void OnClickSubtracao()
    {
        PlayerPrefs.SetString("TopicoEscolhido", "subtracao");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_Su"); // Vai para a tela de subtração
    }

    public void OnClickDivisao()
    {
        PlayerPrefs.SetString("TopicoEscolhido", "divisao");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_D"); // Vai para a tela de divisão
    }
}
