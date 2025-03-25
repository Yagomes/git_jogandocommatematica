using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic; // Adicione esta linha


// Classe que representa cada t�pico recebido do PHP
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
        int idTurma = PlayerPrefs.GetInt("id_turma", 0);
        form.AddField("id_turma", idTurma);

        Debug.Log("Enviando ID da turma: " + idTurma); // Log para depura��o

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
                topicosUnicos.Add(topico.Nome_topico.ToLower()); // Adiciona ao HashSet para garantir unicidade
            }

            // Salva a quantidade de t�picos �nicos
            PlayerPrefs.SetInt("TotalTopicos", topicosUnicos.Count);
            Debug.Log("Total de t�picos �nicos recebidos: " + topicosUnicos.Count);


            // Desabilita os bot�es inicialmente
            btnSoma.interactable = false;
            btnMultiplicacao.interactable = false;

            // Habilita os bot�es com base nos t�picos recebidos
            foreach (Topico topico in topicoWrapper.topicos)
            {

                Debug.Log($"T�pico encontrado: {topico.Nome_topico}");

                // Salva os valores no PlayerPrefs
                PlayerPrefs.SetInt(topico.Nome_topico + "_Min", topico.Num_Min_topico);
                PlayerPrefs.SetInt(topico.Nome_topico + "_Max", topico.Num_Max_topico);

                // Habilita os bot�es correspondentes
                if (topico.Nome_topico.ToLower() == "soma")
                    btnSoma.interactable = true;

                if (topico.Nome_topico.ToLower() == "mult")
                    btnMultiplicacao.interactable = true;

                if (topico.Nome_topico.ToLower() == "sub")
                    btnSubtracao.interactable = true;

                if (topico.Nome_topico.ToLower() == "div")
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
        PlayerPrefs.SetString("TopicoEscolhido", "mult");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_M"); // Vai para a tela de multiplica��o
    }

    public void OnClickSubtracao()
    {
        PlayerPrefs.SetString("TopicoEscolhido", "sub");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_Su"); // Vai para a tela de subtra��o
    }

    public void OnClickDivisao()
    {
        PlayerPrefs.SetString("TopicoEscolhido", "div");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tela_Nivel_D"); // Vai para a tela de divis�o
    }
}
