using System.Collections.Generic;
using UnityEngine;

public class GameManager_b : MonoBehaviour
{
    public static GameManager_b instance;

    // Dicionário para armazenar o estado dos baús (aberto ou fechado)
    private Dictionary<string, bool> chestStates = new Dictionary<string, bool>();

    // Novo dicionário para armazenar o estado de interação (se o jogador pode interagir ou não com o baú)
    private Dictionary<string, bool> interactionStates = new Dictionary<string, bool>();

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Mantém o GameManager ao mudar de cena
        }
        else
        {
            Destroy(this.gameObject); // Destroys duplicate instances
        }
    }

    // Salva o estado de um baú (aberto ou fechado)
    public void SetChestState(string chestID, bool isOpen)
    {
        chestStates[chestID] = isOpen;
    }

    // Recupera o estado de um baú (aberto ou fechado)
    public bool GetChestState(string chestID)
    {
        return chestStates.ContainsKey(chestID) && chestStates[chestID];
    }

    // Salva o estado de interação de um baú (se o jogador pode interagir com ele)
    public void SetInteractionState(string chestID, bool canInteract)
    {
        interactionStates[chestID] = canInteract;
    }

    // Recupera o estado de interação de um baú (se o jogador pode interagir com ele)
    public bool GetInteractionState(string chestID)
    {
        return interactionStates.ContainsKey(chestID) && interactionStates[chestID];
    }

    // Redefine todos os baús para o estado "fechado" e interativo
    public void ResetChestStates()
    {
        var keys = new List<string>(chestStates.Keys);
        foreach (var key in keys)
        {
            chestStates[key] = false;
        }

        var interactionKeys = new List<string>(interactionStates.Keys);
        foreach (var key in interactionKeys)
        {
            interactionStates[key] = false; // Por padrão, o jogador pode interagir com o baú (exceto quando bloqueado)
        }
    }
}
