using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScriptBau : MonoBehaviour
{
    public GameObject carta;  // A carta que ser� mostrada
    public Button[] chaveButtons; // Array contendo os bot�es das chaves
    public TMP_Text perguntaText;  // Texto para exibir a pergunta (opera��o matem�tica)
    private SpriteRenderer spriteRenderer;
    public Sprite spriteAberto;
    [SerializeField] public GameObject[] ferramentasDisponiveis; // Lista de ferramentas poss�veis
    [SerializeField] private Animator bauAnimator;
    private int resultadoUniversal;
    private bool cartaAberta = false;
    private bool bauBloqueado = false;

    // Define o LayerMask para o ba�
    [SerializeField] private LayerMask chestLayer;

    private void Start()
    {
        
        bauAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (carta == null)
        {
            if (bauAnimator != null) bauAnimator.SetBool("Aberto", true);
            
        }
        else
        {
        
        if (Input.GetMouseButtonDown(0) && !bauBloqueado && carta !=null)
        {
            // Captura a posi��o do mouse no mundo
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePosition.x, mousePosition.y);

            // Executa o Raycast filtrando pela camada do ba�
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, chestLayer);

            // Verifica se o clique foi no ba�
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                float distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

                if (distance <= 2f && !cartaAberta)
                {
                    

                    carta.SetActive(true);
                    cartaAberta = true;
                    Debug.Log("Ba� aberto!");
                    GerarOperacao();
                }
                else if (distance > 2f)
                {
                    Debug.Log("Voc� precisa estar mais perto do ba� para abri-lo.");
                }
            }
            }
        

        // Fecha a carta se o jogador se afastar
        float dist = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        if (dist > 2f && carta != null && carta.activeSelf)
        {
            carta.SetActive(false);
            cartaAberta = false;
            Debug.Log("Voc� se afastou do ba� e a carta foi fechada.");
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

        if (perguntaText == null)
        {
            Debug.LogError("perguntaText n�o foi atribu�do no Inspector!");
            return;
        }

        // Gera n�meros aleat�rios e define a opera��o matem�tica
        int min = PlayerPrefs.GetInt("NivelMin");
        int max = PlayerPrefs.GetInt("NivelMax");

        int num1 = Random.Range(min, max + 1);
        int num2 = Random.Range(min, max + 1);

        int resultado;
        string operacao;

        if (PlayerPrefs.GetString("TopicoEscolhido").ToLower() == "soma")
        {
            resultado = num1 + num2;
            operacao = $"{num1} + {num2}";
        }
        else 
        {
            resultado = num1 * num2;
            operacao = $"{num1} x {num2}";
        }

        resultadoUniversal = resultado;

        // Gera respostas falsas e embaralha
        int[] respostas = new int[3];
        respostas[0] = resultado;
        respostas[1] = resultado + Random.Range(1, 5);
        respostas[2] = resultado - Random.Range(1, 5);

        for (int i = 0; i < respostas.Length; i++)
        {
            int randomIndex = Random.Range(0, respostas.Length);
            int temp = respostas[i];
            respostas[i] = respostas[randomIndex];
            respostas[randomIndex] = temp;
        }

        // Atualiza os bot�es com as respostas
        for (int i = 0; i < chaveButtons.Length; i++)
        {
            chaveButtons[i].GetComponentInChildren<TMP_Text>().text = respostas[i].ToString();
            int resposta = respostas[i];
            chaveButtons[i].onClick.RemoveAllListeners();
            chaveButtons[i].onClick.AddListener(() => OnButtonClicked(resposta));
        }

        perguntaText.text = operacao;
        Debug.Log($"Opera��o gerada: {operacao} = {resultado}");
    }

    public void OnButtonClicked(int resposta)
    {
        VerificarResposta(resposta);
    }

    public void VerificarResposta(int respostaSelecionada)
    {
        if (carta == null)
        {

            Debug.LogError("Objeto 'carta' n�o foi atribu�do no Inspector!");
            return;
        }

        if (respostaSelecionada == resultadoUniversal)
        {
            DroparItens();
            carta?.SetActive(false);
            cartaAberta = false;
            bauBloqueado = true;
            Debug.Log("Resposta correta! Carta aberta.");

            if (spriteRenderer != null) spriteRenderer.sprite = spriteAberto;
            if (bauAnimator != null) bauAnimator.SetBool("Aberto", true);
            
        }
        else
        {
            carta.SetActive(false);
            cartaAberta = false;
            bauBloqueado = false;
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
