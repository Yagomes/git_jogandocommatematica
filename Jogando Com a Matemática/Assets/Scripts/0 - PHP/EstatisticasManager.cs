using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
/*
     EstatisticasManager.instance.AdicionarMoedas(10); // moedas
     EstatisticasManager.instance.AdicionarAcerto(); // acertou questao
     EstatisticasManager.instance.AdicionarErro(); // errou questão
     EstatisticasManager.instance.AdicionarInimigoDerrotado(); // matou um inimigo
     EstatisticasManager.instance.AdicionarJogo(); // jogou uma partida
     EstatisticasManager.instance.AdicionarNiveisDesbloqueados(); // desbloqueou um nível





 */
public class EstatisticasManager : MonoBehaviour
{
    public static EstatisticasManager instance;

    public int aluno_id;
    public int estatistica_total_jogado;
    public int estatistica_acertos;
    public int estatistica_erros;
    public int estatistica_inimigos_derrotados;
    public int estatistica_moedas_acumuladas;
    public int estatistica_niveis_desbloqueados;


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
        form.AddField("aluno_id", aluno_id);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/projeto/buscar_estatisticas.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var data = JsonUtility.FromJson<EstatisticasResponse>(www.downloadHandler.text);
                if (data.status == "success")
                {
                    estatistica_total_jogado = data.estatistica_total_jogado;
                    estatistica_acertos = data.estatistica_acertos;
                    estatistica_erros = data.estatistica_erros;
                    estatistica_inimigos_derrotados = data.estatistica_inimigos_derrotados;
                    estatistica_moedas_acumuladas = data.estatistica_moedas_acumuladas;
                    estatistica_niveis_desbloqueados = data.estatistica_niveis_desbloqueados;

                    Debug.Log("Estatísticas carregadas com sucesso");
                }
                else
                {
                    Debug.Log("Erro: " + data.erro);
                    estatistica_total_jogado = 0;
                    estatistica_acertos = 0;
                    estatistica_erros = 0;
                    estatistica_inimigos_derrotados = 0;
                    estatistica_moedas_acumuladas = 0;

                    yield return new WaitForSeconds(3f);

                    int totalTopicos = PlayerPrefs.GetInt("TotalTopicos", 0);

                    Debug.Log("Número total de tópicos: " + totalTopicos);

                    estatistica_niveis_desbloqueados = totalTopicos;
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
        form.AddField("aluno_id", aluno_id);
        form.AddField("estatistica_total_jogado", estatistica_total_jogado);
        form.AddField("estatistica_acertos", estatistica_acertos);
        form.AddField("estatistica_erros", estatistica_erros);
        form.AddField("estatistica_inimigos_derrotados", estatistica_inimigos_derrotados);
        form.AddField("estatistica_moedas_acumuladas", estatistica_moedas_acumuladas);
        form.AddField("estatistica_niveis_desbloqueados", niveis_desbloqueados);


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

    public void AdicionarNiveisDesbloqueados()
    {
        niveis_desbloqueados++;
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
    public int niveis_desbloqueados;
    public string erro;
}

[System.Serializable]
public class StatusResponse
{
    public string status;
    public string erro;
}

