using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
/*
     EstatisticasManager.instance.AdicionarMoedas(10); // moedas
     EstatisticasManager.instance.AdicionarAcerto(); // acertou questao
     EstatisticasManager.instance.AdicionarErro(); // errou questão
     EstatisticasManager.instance.AdicionarInimigoDerrotado(); // matou um inimigo
     EstatisticasManager.instance.AdicionarJogo(); // jogou uma partida





 */
public class EstatisticasManager : MonoBehaviour
{
    public static EstatisticasManager instance;

    public int idAluno;
    public int totalJogado;
    public int acertos;
    public int erros;
    public int inimigosDerrotados;
    public int moedasAcumuladas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BuscarEstatisticas()
    {
        StartCoroutine(BuscarEstatisticasRoutine());
    }

    public void SalvarEstatisticas()
    {
        StartCoroutine(SalvarEstatisticasRoutine());
    }



    IEnumerator BuscarEstatisticasRoutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("id_Aluno", idAluno);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/projeto/buscar_estatisticas.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var data = JsonUtility.FromJson<EstatisticasResponse>(www.downloadHandler.text);
                if (data.status == "success")
                {
                    totalJogado = data.total_jogado;
                    acertos = data.acertos;
                    erros = data.erros;
                    inimigosDerrotados = data.inimigos_derrotados;
                    moedasAcumuladas = data.moedas_acumuladas;
                    Debug.Log("Estatísticas carregadas com sucesso");
                }
                else
                {
                    Debug.Log("Erro: " + data.erro);
                    totalJogado = 0;
                    acertos = 0;
                    erros = 0;
                    inimigosDerrotados = 0;
                    moedasAcumuladas = 0;
                }
            }
            else
            {
                Debug.Log("Erro na requisição: " + www.error);
            }
        }
    }

    IEnumerator SalvarEstatisticasRoutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("id_Aluno", idAluno);
        form.AddField("total_jogado", totalJogado);
        form.AddField("acertos", acertos);
        form.AddField("erros", erros);
        form.AddField("inimigos_derrotados", inimigosDerrotados);
        form.AddField("moedas_acumuladas", moedasAcumuladas);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/projeto/atualizar_estatisticas.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var data = JsonUtility.FromJson<StatusResponse>(www.downloadHandler.text);
                if (data.status == "success")
                {
                    Debug.Log("Estatísticas atualizadas com sucesso");
                }
                else
                {
                    Debug.Log("Erro: " + data.erro);
                }
            }
            else
            {
                Debug.Log("Erro na requisição: " + www.error);
            }
        }
    }
    public void AdicionarMoedas(int quantidade)
    {
        moedasAcumuladas += quantidade;
    }

    public void AdicionarAcerto()
    {
        acertos++;
    }

    public void AdicionarErro()
    {
        erros++;
    }

    public void AdicionarInimigoDerrotado()
    {
        inimigosDerrotados++;
    }

    public void AdicionarJogo()
    {
        totalJogado++;
    }

}

[System.Serializable]
public class EstatisticasResponse
{
    public string status;
    public int total_jogado;
    public int acertos;
    public int erros;
    public int inimigos_derrotados;
    public int moedas_acumuladas;
    public string erro;
}

[System.Serializable]
public class StatusResponse
{
    public string status;
    public string erro;
}

