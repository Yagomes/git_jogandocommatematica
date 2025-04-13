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
    private bool bauInterage = true;
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

        // Carregar o estado do ba� (aberto ou fechado) e de intera��o ao mudar de cena
        if (GameManager_b.instance != null)
        {
            bauBloqueado = GameManager_b.instance.GetChestState(chestID);
            bauInterage = GameManager_b.instance.GetInteractionState(chestID); // Recupera se o ba� pode ser interagido

            Debug.Log(bauInterage);
            Debug.Log(bauBloqueado);
            
            if(bauInterage==true && !bauBloqueado)
            {
                cartaAberta = false;
                // Se o ba� n�o foi bloqueado, ele permanece fechado
                if (bauAnimator != null) bauAnimator.SetBool("Aberto", false);
            }

            else if (bauBloqueado && bauInterage == true)
            {
                cartaAberta = true;
                // Se o ba� foi aberto
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
                    Debug.Log("Voc� precisa estar mais perto do ba� para abri-lo.");
                }
            }
        }

        // Verifica se a carta ainda existe antes de us�-la
        if (carta != null)
        {
           // BuscarOuCriarObjetos();
            float dist = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
            if (dist > 2f && cartaAberta && carta.activeSelf)
            {
                carta.SetActive(false);
                cartaAberta = false;
                Debug.Log("Voc� se afastou do ba� e a carta foi fechada.");
            }
        }
        else if(carta==null && !bauBloqueado)
        {
          BuscarOuCriarObjetos();
        }
    }


    private void BuscarOuCriarObjetos()
    {
        GameObject pai = GameObject.Find("UICanvas"); // Certifique-se de que o nome do pai est� correto
        if (pai != null)
        {
            // Tenta encontrar o filho "Carta" dentro do pai
            Transform cartaTransform = pai.transform.Find("Carta");
            if (cartaTransform != null)
            {
                carta = cartaTransform.gameObject; // Refer�ncia ao GameObject "Carta"
                Debug.Log("Carta encontrada mesmo estando inativa!");
            }
            else
            {
                Debug.Log("Carta n�o encontrada como filha de UICanvas.");
            }
        }
        else
        {
            Debug.Log("UICanvas n�o encontrado.");
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
        if (perguntaText == null && carta != null && !bauBloqueado)
        {
            perguntaText = carta.GetComponentInChildren<TMP_Text>(); // Componente de pergunta
            if (perguntaText == null)
                Debug.LogError("Componente TMP_Text (PerguntaText) n�o encontrado na carta!");
        }

        // Buscar o componente de express�o (caso tenha um campo de texto separado para isso)
        if (expressaoText == null && carta != null && !bauBloqueado)
        {
            expressaoText = carta.transform.Find("ExpressaoText")?.GetComponent<TMP_Text>(); // Ajuste o nome conforme o seu prefab
            if (expressaoText == null)
                Debug.LogError("Componente TMP_Text (ExpressaoText) n�o encontrado na carta!");
        }

        // Criar bot�es de resposta, se necess�rio
        if (chaveButtonPrefab != null && !bauBloqueado && carta != null)
        {
            chaveButtons = new Button[3]; // Array para armazenar os bot�es

            // Posi��es para as chaves dentro da carta
            Vector3[] posicoesChaves = new Vector3[]
            {
               new Vector3(-100, 50, 0), // Posi��o da primeira chave
               new Vector3(0, 50, 0),    // Posi��o da segunda chave
               new Vector3(100, 50, 0)   // Posi��o da terceira chave
            };

            for (int i = 0; i < 3; i++)
            {
                // Verificar se a chave j� existe dentro da carta
                string chaveNome = $"Chave{i + 1}";
                Transform chaveExistente = carta.transform.Find(chaveNome);

                    if (chaveExistente != null)
                {
                    Debug.Log($"Chave {i + 1} j� existe. N�o ser� criada novamente.");
                    chaveButtons[i] = chaveExistente.GetComponent<Button>();
                    continue; // Pula para a pr�xima itera��o
                }

                // Criar o bot�o caso ele n�o exista
                GameObject button = Instantiate(chaveButtonPrefab, carta.transform);
                chaveButtons[i] = button.GetComponent<Button>();
                button.name = chaveNome;

                // Ajustar a posi��o do bot�o dentro da carta
                button.transform.localPosition = posicoesChaves[i];

                Debug.Log($"Bot�o {i + 1} criado com sucesso e posicionado!");
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

        string topico = PlayerPrefs.GetString("TopicoEscolhido").ToLower();

        if (topico == "soma")
        {
            resultado = num1 + num2;
            operacao = $"{num1} + {num2}";
        }
        else if (topico == "sub" || topico == "subtracao")
        {
            // Garantir que num1 >= num2 para n�o gerar n�mero negativo
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
            // Evitar divis�o com restos (resultado inteiro)
            num2 = Random.Range(min, max + 1);
            if (num2 == 0) num2 = 1; // evitar divis�o por zero

            resultado = Random.Range(min, max + 1);
            num1 = num2 * resultado; // garante que num1 seja m�ltiplo de num2

            operacao = $"{num1} � {num2}";
        }
        else // Multiplica��o
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
            EstatisticasManager.instance.AdicionarAcerto(); // acertou questao

            DroparItens();
            if (carta != null)
            {
                carta.SetActive(false);
            }
            cartaAberta = false;
            bauInterage = true; // Desativa a intera��o ap�s a resposta correta
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
                // Carrega o t�pico escolhido salvo no PlayerPrefs
               string topicoEscolhido = PlayerPrefs.GetString("TopicoEscolhido", "");

                if (string.IsNullOrEmpty(topicoEscolhido))
                {
                    Debug.LogError("T�pico n�o foi configurado em PlayerPrefs!");
                    return;
                }


                if (topicoEscolhido.ToLower() == "soma")
                {
                   ChestLevelManager_b.Instance.ChestOpened("soma"); // Ou "Multiplica��o", dependendo do t�pico
                }
                if (topicoEscolhido.ToLower() == "multiplicacao" || topicoEscolhido.ToLower() == "mult")
                {
                    ChestLevelManager_b.Instance.ChestOpened("multiplicacao"); // Ou "Multiplica��o", dependendo do t�pico
                }
                if (topicoEscolhido.ToLower() == "div" || topicoEscolhido.ToLower() == "divisao")
                {
                    ChestLevelManager_b.Instance.ChestOpened("divisao"); // Ou "Multiplica��o", dependendo do t�pico
                }
                else if (topicoEscolhido.ToLower() == "sub" || topicoEscolhido.ToLower() == "subtracao")
                {
                    ChestLevelManager_b.Instance.ChestOpened("subtracao"); // Ou "Multiplica��o", dependendo do t�pico
                }





            }

        }
        else
        {
            EstatisticasManager.instance.AdicionarErro(); // errou quest�o

            if (carta != null)
            {
                carta.SetActive(false);
            }
            cartaAberta = false;
            bauInterage = true; // Bloqueia a intera��o ap�s erro
            bauBloqueado = false; // Mant�m o ba� fechado

            if (bauAnimator != null)
            {
                bauAnimator.SetBool("Aberto", false);
            }

            if (GameManager_b.instance != null)
            {
                GameManager_b.instance.SetChestState(chestID, bauBloqueado);
                GameManager_b.instance.SetInteractionState(chestID, bauInterage);
            }

            Debug.Log("Resposta incorreta! O ba� est� agora bloqueado permanentemente");
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
