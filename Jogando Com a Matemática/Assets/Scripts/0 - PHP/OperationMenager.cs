using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic; // Adicione esta linha


// Classe que representa cada t�pico recebido do PHP
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

        // Cria o formul�rio e adiciona o ID da turma salvo em PlayerPrefs
        WWWForm form = new WWWForm();
        int turma_id = PlayerPrefs.GetInt("turma_id", 0);
        form.AddField("turma_id", turma_id);

        Debug.Log("Enviando ID da turma: " + turma_id); // Log para depura��o

        // Faz a requisi��o ao servidor
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        // Verifica se a requisi��o foi bem-sucedida
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

            // Salva a quantidade de t�picos �nicos
            PlayerPrefs.SetInt("TotalTopicos", topicosUnicos.Count);
            Debug.Log("Total de t�picos �nicos recebidos: " + topicosUnicos.Count);


            // Desabilita os bot�es inicialmente
            btnSoma.interactable = false;
            btnMultiplicacao.interactable = false;
            btnSubtracao.interactable = false;
            btnDivisao.interactable = false;

            // Habilita os bot�es com base nos t�picos recebidos
            foreach (Topico topico in topicoWrapper.topicos)
            {

                Debug.Log($"T�pico encontrado: {topico.topico_nome}");

                // Salva os valores no PlayerPrefs
                PlayerPrefs.SetInt(topico.topico_nome + "_min", topico.topico_num_min);
                PlayerPrefs.SetInt(topico.topico_nome + "_max", topico.topico_num_max);

                // Habilita os bot�es correspondentes
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
            Debug.LogError("Erro ao buscar t�picos: " + www.error);
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_M"); // Vai para a tela de multiplica��o
    }

    public void OnClickSubtracao()
    {
        PlayerPrefs.SetString("TopicoEscolhido", "subtracao");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_Su"); // Vai para a tela de subtra��o
    }

    public void OnClickDivisao()
    {
        PlayerPrefs.SetString("TopicoEscolhido", "divisao");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_D"); // Vai para a tela de divis�o
    }
}
