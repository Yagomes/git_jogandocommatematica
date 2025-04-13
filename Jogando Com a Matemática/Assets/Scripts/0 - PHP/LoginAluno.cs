using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LoginResponse
{
    public int aluno_id;
    public int turma_id;
    public string aluno_genero;
    public string status;
    public string erro;
}


public class LoginAluno : MonoBehaviour // Verifica o login do aluno atraves de PHP.
{
    public InputField inputMatricula; // Campo de entrada para matrícula
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

        // Criando o formulário com as credenciais
        WWWForm form = new WWWForm();
        form.AddField("aluno_matricula", inputMatricula.text);
        form.AddField("aluno_senha", inputSenha.text);

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
                        Debug.Log($"Login bem-sucedido! ID Aluno: {response.aluno_id}, ID Turma: {response.turma_id}, Gênero: {response.aluno_genero}");

                        PlayerPrefs.SetInt("turma_id", response.turma_id);
                        PlayerPrefs.SetInt("aluno_id", response.aluno_id);
                        PlayerPrefs.SetString("aluno_genero", response.aluno_genero);

                        EstatisticasManager.instance.aluno_id = PlayerPrefs.GetInt("aluno_id", 0);
                        EstatisticasManager.instance.BuscarEstatisticas();

                        StartCoroutine(CarregarCenaComDelay(2f)); // Aguarda 2 segundos antes de trocar de cena
                    }
                

                    else
                    {
                        mensagemErro.text = response.erro;
                        Debug.Log("Login inválido: " + response.erro);
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
                Debug.LogError("Erro na requisição: " + request.error);
                mensagemErro.text = "Erro ao conectar ao servidor. Verifique sua conexão.";
            }
        }
    }

    private IEnumerator CarregarCenaComDelay(float delay)
    {
       
         yield return new WaitForSeconds(delay);
        EconomyManager.Instance.currentGold = EstatisticasManager.instance.estatistica_moedas_acumuladas;


        SceneManager.LoadScene("Operacao");
    }
}
