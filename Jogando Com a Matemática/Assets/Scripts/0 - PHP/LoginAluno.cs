using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LoginResponse
{
    public int id_Aluno;
    public int id_turma;
    public string genero;
    public string status;
    public string erro;
}


public class LoginAluno : MonoBehaviour // Verifica o login do aluno atraves de PHP.
{
    public InputField inputMatricula; // Campo de entrada para matr�cula
    public InputField inputSenha;    // Campo de entrada para senha
    public Text mensagemErro;        // Texto para exibir mensagens de erro

    public void Login()
    {
        EconomyManager.Instance.currentGold = 0;
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        string url = "http://localhost/projeto/login_aluno.php";

        // Criando o formul�rio com as credenciais
        WWWForm form = new WWWForm();
        form.AddField("matricula", inputMatricula.text);
        form.AddField("senha", inputSenha.text);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log("Resposta do servidor: " + json);

                try
                {
                    // Deserializando o JSON para um objeto LoginResponse
                    LoginResponse response = JsonUtility.FromJson<LoginResponse>(json);

                    if (response.status == "success")
                    {
                        Debug.Log($"Login bem-sucedido! ID Aluno: {response.id_Aluno}, ID Turma: {response.id_turma}, G�nero: {response.genero}");

                        PlayerPrefs.SetInt("id_turma", response.id_turma);
                        PlayerPrefs.SetInt("id_Aluno", response.id_Aluno);
                        PlayerPrefs.SetString("genero", response.genero);

                        EstatisticasManager.instance.idAluno = PlayerPrefs.GetInt("id_Aluno", 0);
                        EstatisticasManager.instance.BuscarEstatisticas();

                        StartCoroutine(CarregarCenaComDelay(2f)); // Aguarda 2 segundos antes de trocar de cena
                    }
                

                    else
                    {
                        mensagemErro.text = response.erro;
                        Debug.Log("Login inv�lido: " + response.erro);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Erro ao processar JSON: {e.Message}");
                    mensagemErro.text = "Erro ao processar a resposta. Tente novamente.";
                }
            }
            else
            {
                Debug.LogError("Erro na requisi��o: " + request.error);
                mensagemErro.text = "Erro ao conectar ao servidor. Verifique sua conex�o.";
            }
        }
    }

    private IEnumerator CarregarCenaComDelay(float delay)
    {
       
         yield return new WaitForSeconds(delay);
        EconomyManager.Instance.currentGold = EstatisticasManager.instance.moedasAcumuladas;


        SceneManager.LoadScene("Operacao");
    }
}
