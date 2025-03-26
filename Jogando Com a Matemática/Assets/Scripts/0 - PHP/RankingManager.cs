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
        int idTurma = PlayerPrefs.GetInt("id_turma", 0); // ID da turma salvo no PlayerPrefs
        StartCoroutine(GetRanking(idTurma));
    }

    IEnumerator GetRanking(int idTurma)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url + "?id_turma=" + idTurma))
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
            columns[0].text = entry.Nome;
            columns[1].text = entry.moedas_acumuladas.ToString();
            columns[2].text = entry.acertos.ToString();
            columns[3].text = entry.inimigos_derrotados.ToString();
            columns[4].text = entry.niveis_desbloqueados.ToString();
        }
    }
}

[System.Serializable]
public class RankingEntry
{
    public string Nome;
    public int moedas_acumuladas;
    public int acertos;
    public int inimigos_derrotados;
    public int niveis_desbloqueados;
}

