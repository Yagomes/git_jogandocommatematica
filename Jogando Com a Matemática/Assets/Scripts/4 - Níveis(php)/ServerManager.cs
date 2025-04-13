using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{
    // Altere essa URL para o endere�o do seu servidor onde est�o os scripts PHP.
    private static string baseURL = "http://localhost/projeto/";

    /// <summary>
    /// Envia via POST os dados do progresso (alunoId, n�vel e t�pico) para o servidor.
    /// </summary>
    public static IEnumerator PostProgresso(int alunoId, int nivel, string topico, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("aluno_id", alunoId);
        form.AddField("progresso_nivel", nivel);
        form.AddField("progresso_topico", topico);

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "salvar_progresso.php", form))
        {
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
#else
            if (www.isNetworkError || www.isHttpError)
#endif
            {
                Debug.LogError("Erro ao salvar progresso: " + www.error);
                callback?.Invoke(null);
            }
            else
            {
                Debug.Log("Progresso salvo: " + www.downloadHandler.text);
                callback?.Invoke(www.downloadHandler.text);
            }
        }
    }

    /// <summary>
    /// Busca via GET o progresso do aluno para um determinado t�pico.
    /// </summary>
    public static IEnumerator GetProgresso(int aluno_id, string topico, System.Action<string> callback)
    {
        string url = baseURL + "buscar_progresso.php?aluno_id=" + aluno_id + "&progresso_topico=" + UnityWebRequest.EscapeURL(topico);
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
#else
            if (www.isNetworkError || www.isHttpError)
#endif
            {
                Debug.LogError("Erro ao buscar progresso: " + www.error);
                callback?.Invoke(null);
            }
            else
            {
                Debug.Log("Progresso recebido: " + www.downloadHandler.text);
                callback?.Invoke(www.downloadHandler.text);
            }
        }
    }
}
