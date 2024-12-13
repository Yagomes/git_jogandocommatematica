using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScriptBau : MonoBehaviour
{
    public string chestID; // Identificador �nico para o ba�

    private bool cartaAberta = false;
    private bool bauBloqueado = false;
    private int resultadoUniversal;

    [SerializeField] private Animator bauAnimator;

    // Define o LayerMask para o ba�
    [SerializeField] private LayerMask chestLayer;

    // Prefabs para recria��o, caso necess�rio
    [SerializeField] private GameObject cartaPrefab;
    [SerializeField] private GameObject chaveButtonPrefab;

    // Refer�ncias din�micas para objetos no DontDestroyOnLoad
    private GameObject carta;
    private Button[] chaveButtons;
    private TMP_Text perguntaText;
    private TMP_Text expressaoText; // Para o texto da express�o matem�tica

    [SerializeField] public GameObject[] ferramentasDisponiveis; // Lista de ferramentas poss�veis

    private void Start()
    {
        bauAnimator = GetComponent<Animator>();

        // Carregar o estado do ba� no in�cio
        if (GameManager_b.instance != null)
        {
            bauBloqueado = GameManager_b.instance.GetChestState(chestID);
            if (bauBloqueado)
            {
                cartaAberta = true;
                if (bauAnimator != null) bauAnimator.SetBool("Aberto", true);
            }
        }


       

        if (carta != null) carta.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !bauBloqueado && carta!= null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, chestLayer);


            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
            
                if (distance <= 2f && !cartaAberta)
                {
                    if (carta != null)
                    {
                        carta.SetActive(true);
                        cartaAberta = true;
                        Debug.Log("Ba� aberto!");
                        GerarOperacao();
                    }
                    else
                    {

                        Debug.LogError("Carta n�o encontrada ou foi destru�da!");
                    }
                }
                else if (distance > 2f)
                {
                    Debug.Log("Voc� precisa estar mais perto do ba� para abri-lo.");
                }
            }
        }

        // Verifica se a carta ainda existe antes de us�-la
        if (carta != null)
        {
           
            float dist = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
            if (dist > 2f && cartaAberta && carta.activeSelf)
            {
                carta.SetActive(false);
                cartaAberta = false;
                Debug.Log("Voc� se afastou do ba� e a carta foi fechada.");
            }
        }
        else if(carta==null)
        {
           // Debug.Log("Carta criada.");
          BuscarOuCriarObjetos();
        }
    }


    private void BuscarOuCriarObjetos()
    {
        carta = GameObject.Find("Carta");
        Destroy(carta);
        if (carta == null && cartaPrefab != null)
        {
            GameObject canvas = GameObject.Find("UICanvas");
            if (canvas != null)
            {
                carta = Instantiate(cartaPrefab, canvas.transform);
                carta.name = "Carta";

                // Definir posi��o da carta no canvas
                Vector3 posicaoCarta = new Vector3(0, 0, 0); // Posi��o desejada (ajuste conforme necess�rio)
                carta.transform.localPosition = posicaoCarta;

                Debug.Log("Carta criada com sucesso!");
            }
            else
            {
                Debug.LogError("Canvas 'UICanvas' n�o encontrado! Certifique-se de que est� configurado corretamente.");
                return;
            }
        }

        // Verifica os componentes de texto da carta
        if (perguntaText == null && carta != null)
        {
            perguntaText = carta.GetComponentInChildren<TMP_Text>(); // Componente de pergunta
            if (perguntaText == null)
                Debug.LogError("Componente TMP_Text (PerguntaText) n�o encontrado na carta!");
        }

        // Buscar o componente de express�o (caso tenha um campo de texto separado para isso)
        if (expressaoText == null && carta != null)
        {
            expressaoText = carta.transform.Find("ExpressaoText")?.GetComponent<TMP_Text>(); // Ajuste o nome conforme o seu prefab
            if (expressaoText == null)
                Debug.LogError("Componente TMP_Text (ExpressaoText) n�o encontrado na carta!");
        }

        // Criar bot�es de resposta, se necess�rio
        if ((chaveButtons == null || chaveButtons.Length < 3) && chaveButtonPrefab != null)
        {
            chaveButtons = new Button[3];
            if (carta != null) // Certifique-se de que a carta foi criada corretamente
            {
                // Posi��es para as chaves dentro da carta
                Vector3[] posicoesChaves = new Vector3[]
                {
                new Vector3(-100, 50, 0), // Posi��o da primeira chave
                new Vector3(0, 50, 0),    // Posi��o da segunda chave
                new Vector3(100, 50, 0)   // Posi��o da terceira chave
                };

                for (int i = 0; i < 3; i++)
                {
                    GameObject button = Instantiate(chaveButtonPrefab, carta.transform); // Definir a carta como pai
                    chaveButtons[i] = button.GetComponent<Button>();
                    button.name = $"Chave{i + 1}";

                    // Ajustar a posi��o do bot�o dentro da carta
                    button.transform.localPosition = posicoesChaves[i];

                    Debug.Log($"Bot�o {i + 1} criado com sucesso e posicionado!");
                }
            }
            else
            {
                Debug.LogError("Carta n�o encontrada para criar bot�es.");
            }
        }
    }
    void GerarOperacao()
    {
        if (chaveButtons == null || chaveButtons.Length < 3)
        {
            Debug.LogError("chaveButtons n�o configurado ou possui menos de 3 bot�es!");
            return;
        }

        if (perguntaText == null || expressaoText == null)
        {
            Debug.LogError("perguntaText ou expressaoText n�o foi atribu�do no Inspector!");
            return;
        }

        int min = PlayerPrefs.GetInt("NivelMin");
        int max = PlayerPrefs.GetInt("NivelMax");

        int num1 = Random.Range(min, max + 1);
        int num2 = Random.Range(min, max + 1);

        int resultado;
        string operacao;
        string expressao;

        if (PlayerPrefs.GetString("TopicoEscolhido").ToLower() == "soma")
        {
            resultado = num1 + num2;
            operacao = $"{num1} + {num2}";
            expressao = operacao; // Aqui voc� pode definir a express�o matem�tica
        }
        else
        {
            resultado = num1 * num2;
            operacao = $"{num1} x {num2}";
            expressao = operacao; // Aqui tamb�m a express�o
        }

        resultadoUniversal = resultado;

        int[] respostas = new int[3];
        respostas[0] = resultado;
        respostas[1] = resultado + Random.Range(1, 5);
        respostas[2] = resultado - Random.Range(1, 5);

        // Embaralhar respostas
        for (int i = 0; i < respostas.Length; i++)
        {
            int randomIndex = Random.Range(0, respostas.Length);
            int temp = respostas[i];
            respostas[i] = respostas[randomIndex];
            respostas[randomIndex] = temp;
        }

        // Atualizar os textos dos bot�es
        for (int i = 0; i < chaveButtons.Length; i++)
        {
            chaveButtons[i].GetComponentInChildren<TMP_Text>().text = respostas[i].ToString();
            int resposta = respostas[i];
            chaveButtons[i].onClick.RemoveAllListeners();
            chaveButtons[i].onClick.AddListener(() => OnButtonClicked(resposta));
        }

        // Atualizar o texto da pergunta e da express�o
        perguntaText.text = "Qual � a opera��o?";
        expressaoText.text = expressao;

        Debug.Log($"Opera��o gerada: {operacao} = {resultado}");
    }

    public void OnButtonClicked(int resposta)
    {
        VerificarResposta(resposta);
    }

    public void VerificarResposta(int respostaSelecionada)
    {
        if (respostaSelecionada == resultadoUniversal)
        {
            DroparItens();
            if (carta != null)
            {
                carta.SetActive(false);
            }
            cartaAberta = false;
            bauBloqueado = true;

            if (bauAnimator != null) bauAnimator.SetBool("Aberto", true);

            if (GameManager_b.instance != null)
            {
                GameManager_b.instance.SetChestState(chestID, bauBloqueado);
            }

            Debug.Log("Resposta correta! Carta aberta.");
        }
        else
        {
            if (carta != null)
            {
                carta.SetActive(false);
            }
            cartaAberta = false;
            Debug.Log("Resposta incorreta! Carta fechada.");
        }
    }


    void DroparItens()
    {
        if (ferramentasDisponiveis == null || ferramentasDisponiveis.Length == 0)
        {
            Debug.LogError("Lista de ferramentas est� vazia! Adicione ferramentas no Inspector.");
            return;
        }

        foreach (GameObject item in ferramentasDisponiveis)
        {
            item.SetActive(true);
        }
    }
}
