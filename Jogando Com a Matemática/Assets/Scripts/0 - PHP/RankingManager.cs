using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking; // Usar UnityWebRequest
using Newtonsoft.Json;

public class RankingManager : MonoBehaviour
{
    public Transform content; // Referência ao painel da ScrollView
    public GameObject rowPrefab; // Prefab da linha do ranking
    private string url = "http://localhost/projeto/ranking.php"; // Altere para o seu servidor

    void Start()
    {
        int turma_id = PlayerPrefs.GetInt("turma_id", 0); // ID da turma salvo no PlayerPrefs
        StartCoroutine(GetRanking(turma_id));
    }

    IEnumerator GetRanking(int turma_id)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url + "?id_turma=" + turma_id))
        {
            Debug.Log("Resposta do servidor: " + www.downloadHandler.text);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Resposta do servidor: " + www.downloadHandler.text);

                List<RankingEntry> ranking = JsonConvert.DeserializeObject<List<RankingEntry>>(www.downloadHandler.text);
                DisplayRanking(ranking);
            }
            else
            {
                Debug.LogError("Erro ao obter ranking: " + www.error);
            }
        }
    }

    void DisplayRanking(List<RankingEntry> ranking)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in ranking)
        {
            GameObject row = Instantiate(rowPrefab, content);
            Text[] columns = row.GetComponentsInChildren<Text>();
            columns[0].text = entry.aluno_nome;
            columns[1].text = entry.estatistica_moedas_acumuladas.ToString();
            columns[2].text = entry.estatistica_acertos.ToString();
            columns[3].text = entry.estatistica_inimigos_derrotados.ToString();
            columns[4].text = entry.estatistica_niveis_desbloqueados.ToString();
        }
    }
}

[System.Serializable]
public class RankingEntry
{
    public string aluno_nome;
    public int estatistica_moedas_acumuladas;
    public int estatistica_acertos;
    public int estatistica_inimigos_derrotados;
    public int estatistica_niveis_desbloqueados;
}

