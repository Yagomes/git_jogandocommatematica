using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScriptBau : MonoBehaviour
{
    public string chestID; // Identificador único para o baú

    private bool cartaAberta = false;
    private bool bauBloqueado = false;
    private bool bauInterage = true;
    private int resultadoUniversal;

    [SerializeField] private Animator bauAnimator;

    // Define o LayerMask para o baú
    [SerializeField] private LayerMask chestLayer;

    // Prefabs para recriação, caso necessário
    [SerializeField] private GameObject cartaPrefab;
    [SerializeField] private GameObject chaveButtonPrefab;

    // Referências dinâmicas para objetos no DontDestroyOnLoad
    private GameObject carta;
    private Button[] chaveButtons;
    private TMP_Text perguntaText;
    private TMP_Text expressaoText; // Para o texto da expressão matemática

    [SerializeField] public GameObject[] ferramentasDisponiveis; // Lista de ferramentas possíveis

    private void Start()
    {
        bauAnimator = GetComponent<Animator>();

        // Carregar o estado do baú (aberto ou fechado) e de interação ao mudar de cena
        if (GameManager_b.instance != null)
        {
            bauBloqueado = GameManager_b.instance.GetChestState(chestID);
            bauInterage = GameManager_b.instance.GetInteractionState(chestID); // Recupera se o baú pode ser interagido

            Debug.Log(bauInterage);
            Debug.Log(bauBloqueado);
            
            if(bauInterage==true && !bauBloqueado)
            {
                cartaAberta = false;
                // Se o baú não foi bloqueado, ele permanece fechado
                if (bauAnimator != null) bauAnimator.SetBool("Aberto", false);
            }

            else if (bauBloqueado && bauInterage == true)
            {
                cartaAberta = true;
                // Se o baú foi aberto
                if (bauAnimator != null) bauAnimator.SetBool("Aberto", true);
            }

            
        }


    }
        private void Update()
    {

        if (Input.GetMouseButtonDown(0) && carta!= null && bauInterage==false)
        {
           // BuscarOuCriarObjetos();

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
                        GerarOperacao();
                    }                 
                }
                else if (distance > 2f)
                {
                    Debug.Log("Você precisa estar mais perto do baú para abri-lo.");
                }
            }
        }

        // Verifica se a carta ainda existe antes de usá-la
        if (carta != null)
        {
           // BuscarOuCriarObjetos();
            float dist = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
            if (dist > 2f && cartaAberta && carta.activeSelf)
            {
                carta.SetActive(false);
                cartaAberta = false;
                Debug.Log("Você se afastou do baú e a carta foi fechada.");
            }
        }
        else if(carta==null && !bauBloqueado)
        {
          BuscarOuCriarObjetos();
        }
    }


    private void BuscarOuCriarObjetos()
    {
        GameObject pai = GameObject.Find("UICanvas"); // Certifique-se de que o nome do pai está correto
        if (pai != null)
        {
            // Tenta encontrar o filho "Carta" dentro do pai
            Transform cartaTransform = pai.transform.Find("Carta");
            if (cartaTransform != null)
            {
                carta = cartaTransform.gameObject; // Referência ao GameObject "Carta"
                Debug.Log("Carta encontrada mesmo estando inativa!");
            }
            else
            {
                Debug.Log("Carta não encontrada como filha de UICanvas.");
            }
        }
        else
        {
            Debug.Log("UICanvas não encontrado.");
        }



        //carta = GameObject.Find("Carta");
       // Destroy(carta);
        if (carta == null && cartaPrefab != null && !bauBloqueado)
        {
            GameObject canvas = GameObject.Find("UICanvas");
            if (canvas != null)
            {
                carta = Instantiate(cartaPrefab, canvas.transform);
                carta.name = "Carta";

                // Definir posição da carta no canvas
                Vector3 posicaoCarta = new Vector3(0, 0, 0); // Posição desejada (ajuste conforme necessário)
                carta.transform.localPosition = posicaoCarta;

                Debug.Log("Carta criada com sucesso!");
            }
            else
            {
                Debug.LogError("Canvas 'UICanvas' não encontrado! Certifique-se de que está configurado corretamente.");
                return;
            }

        }

        // Verifica os componentes de texto da carta
        if (perguntaText == null && carta != null && !bauBloqueado)
        {
            perguntaText = carta.GetComponentInChildren<TMP_Text>(); // Componente de pergunta
            if (perguntaText == null)
                Debug.LogError("Componente TMP_Text (PerguntaText) não encontrado na carta!");
        }

        // Buscar o componente de expressão (caso tenha um campo de texto separado para isso)
        if (expressaoText == null && carta != null && !bauBloqueado)
        {
            expressaoText = carta.transform.Find("ExpressaoText")?.GetComponent<TMP_Text>(); // Ajuste o nome conforme o seu prefab
            if (expressaoText == null)
                Debug.LogError("Componente TMP_Text (ExpressaoText) não encontrado na carta!");
        }

        // Criar botões de resposta, se necessário
        if (chaveButtonPrefab != null && !bauBloqueado && carta != null)
        {
            chaveButtons = new Button[3]; // Array para armazenar os botões

            // Posições para as chaves dentro da carta
            Vector3[] posicoesChaves = new Vector3[]
            {
               new Vector3(-100, 50, 0), // Posição da primeira chave
               new Vector3(0, 50, 0),    // Posição da segunda chave
               new Vector3(100, 50, 0)   // Posição da terceira chave
            };

            for (int i = 0; i < 3; i++)
            {
                // Verificar se a chave já existe dentro da carta
                string chaveNome = $"Chave{i + 1}";
                Transform chaveExistente = carta.transform.Find(chaveNome);

                    if (chaveExistente != null)
                {
                    Debug.Log($"Chave {i + 1} já existe. Não será criada novamente.");
                    chaveButtons[i] = chaveExistente.GetComponent<Button>();
                    continue; // Pula para a próxima iteração
                }

                // Criar o botão caso ele não exista
                GameObject button = Instantiate(chaveButtonPrefab, carta.transform);
                chaveButtons[i] = button.GetComponent<Button>();
                button.name = chaveNome;

                // Ajustar a posição do botão dentro da carta
                button.transform.localPosition = posicoesChaves[i];

                Debug.Log($"Botão {i + 1} criado com sucesso e posicionado!");
            }
        }
    }

    void GerarOperacao()
    {
        if (chaveButtons == null || chaveButtons.Length < 3)
        {
            Debug.LogError("chaveButtons não configurado ou possui menos de 3 botões!");
            return;
        }

        if (perguntaText == null || expressaoText == null)
        {
            Debug.LogError("perguntaText ou expressaoText não foi atribuído no Inspector!");
            return;
        }

        int min = PlayerPrefs.GetInt("NivelMin");
        int max = PlayerPrefs.GetInt("NivelMax");

        int num1 = Random.Range(min, max + 1);
        int num2 = Random.Range(min, max + 1);

        int resultado;
        string operacao;
        string expressao;

        string topico = PlayerPrefs.GetString("TopicoEscolhido").ToLower();

        if (topico == "soma")
        {
            resultado = num1 + num2;
            operacao = $"{num1} + {num2}";
        }
        else if (topico == "sub" || topico == "subtracao")
        {
            // Garantir que num1 >= num2 para não gerar número negativo
            if (num1 < num2)
            {
                int temp = num1;
                num1 = num2;
                num2 = temp;
            }
            resultado = num1 - num2;
            operacao = $"{num1} - {num2}";
        }
        else if (topico == "div" || topico == "divisao")
        {
            // Evitar divisão com restos (resultado inteiro)
            num2 = Random.Range(min, max + 1);
            if (num2 == 0) num2 = 1; // evitar divisão por zero

            resultado = Random.Range(min, max + 1);
            num1 = num2 * resultado; // garante que num1 seja múltiplo de num2

            operacao = $"{num1} ÷ {num2}";
        }
        else // Multiplicação
        {
            resultado = num1 * num2;
            operacao = $"{num1} x {num2}";
        }

        expressao = operacao;
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

        // Atualizar os textos dos botões
        for (int i = 0; i < chaveButtons.Length; i++)
        {
            chaveButtons[i].GetComponentInChildren<TMP_Text>().text = respostas[i].ToString();
            int resposta = respostas[i];
            chaveButtons[i].onClick.RemoveAllListeners();
            chaveButtons[i].onClick.AddListener(() => OnButtonClicked(resposta));
        }

        // Atualizar o texto da pergunta e da expressão
        perguntaText.text = "Qual é a operação?";
        expressaoText.text = expressao;

        Debug.Log($"Operação gerada: {operacao} = {resultado}");
    }


    public void OnButtonClicked(int resposta)
    {
        VerificarResposta(resposta);
    }

    public void VerificarResposta(int respostaSelecionada)
    {
        if (respostaSelecionada == resultadoUniversal)
        {
            EstatisticasManager.instance.AdicionarAcerto(); // acertou questao

            DroparItens();
            if (carta != null)
            {
                carta.SetActive(false);
            }
            cartaAberta = false;
            bauInterage = true; // Desativa a interação após a resposta correta
            bauBloqueado = true;

            if (bauAnimator != null)
                bauAnimator.SetBool("Aberto", true);

            if (GameManager_b.instance != null)
            {
                GameManager_b.instance.SetChestState(chestID, bauBloqueado);
                GameManager_b.instance.SetInteractionState(chestID, bauInterage);
            }

            Debug.Log("Resposta correta! Carta aberta.");
            Destroy(carta);

            if (ChestLevelManager_b.Instance != null)
            {
                // Carrega o tópico escolhido salvo no PlayerPrefs
               string topicoEscolhido = PlayerPrefs.GetString("TopicoEscolhido", "");

                if (string.IsNullOrEmpty(topicoEscolhido))
                {
                    Debug.LogError("Tópico não foi configurado em PlayerPrefs!");
                    return;
                }


                if (topicoEscolhido.ToLower() == "soma")
                {
                   ChestLevelManager_b.Instance.ChestOpened("soma"); // Ou "Multiplicação", dependendo do tópico
                }
                if (topicoEscolhido.ToLower() == "multiplicacao" || topicoEscolhido.ToLower() == "mult")
                {
                    ChestLevelManager_b.Instance.ChestOpened("multiplicacao"); // Ou "Multiplicação", dependendo do tópico
                }
                if (topicoEscolhido.ToLower() == "div" || topicoEscolhido.ToLower() == "divisao")
                {
                    ChestLevelManager_b.Instance.ChestOpened("divisao"); // Ou "Multiplicação", dependendo do tópico
                }
                else if (topicoEscolhido.ToLower() == "sub" || topicoEscolhido.ToLower() == "subtracao")
                {
                    ChestLevelManager_b.Instance.ChestOpened("subtracao"); // Ou "Multiplicação", dependendo do tópico
                }





            }

        }
        else
        {
            EstatisticasManager.instance.AdicionarErro(); // errou questão

            if (carta != null)
            {
                carta.SetActive(false);
            }
            cartaAberta = false;
            bauInterage = true; // Bloqueia a interação após erro
            bauBloqueado = false; // Mantém o baú fechado

            if (bauAnimator != null)
            {
                bauAnimator.SetBool("Aberto", false);
            }

            if (GameManager_b.instance != null)
            {
                GameManager_b.instance.SetChestState(chestID, bauBloqueado);
                GameManager_b.instance.SetInteractionState(chestID, bauInterage);
            }

            Debug.Log("Resposta incorreta! O baú está agora bloqueado permanentemente");
        }
    }



    void DroparItens()
    {
        if (ferramentasDisponiveis == null || ferramentasDisponiveis.Length == 0)
        {
            Debug.LogError("Lista de ferramentas está vazia! Adicione ferramentas no Inspector.");
            return;
        }

        foreach (GameObject item in ferramentasDisponiveis)
        {
            item.SetActive(true);
        }
    }
}
