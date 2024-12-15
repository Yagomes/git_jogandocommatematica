using System.Collections.Generic;
using UnityEngine;

public class GameManager_b : MonoBehaviour
{
    public static GameManager_b instance;

    // Dicion�rio para armazenar o estado dos ba�s (aberto ou fechado)
    private Dictionary<string, bool> chestStates = new Dictionary<string, bool>();

    // Novo dicion�rio para armazenar o estado de intera��o (se o jogador pode interagir ou n�o com o ba�)
    private Dictionary<string, bool> interactionStates = new Dictionary<string, bool>();

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Mant�m o GameManager ao mudar de cena
        }
        else
        {
            Destroy(this.gameObject); // Destroys duplicate instances
        }
    }

    // Salva o estado de um ba� (aberto ou fechado)
    public void SetChestState(string chestID, bool isOpen)
    {
        chestStates[chestID] = isOpen;
    }

    // Recupera o estado de um ba� (aberto ou fechado)
    public bool GetChestState(string chestID)
    {
        return chestStates.ContainsKey(chestID) && chestStates[chestID];
    }

    // Salva o estado de intera��o de um ba� (se o jogador pode interagir com ele)
    public void SetInteractionState(string chestID, bool canInteract)
    {
        interactionStates[chestID] = canInteract;
    }

    // Recupera o estado de intera��o de um ba� (se o jogador pode interagir com ele)
    public bool GetInteractionState(string chestID)
    {
        return interactionStates.ContainsKey(chestID) && interactionStates[chestID];
    }

    // Redefine todos os ba�s para o estado "fechado" e interativo
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
            interactionStates[key] = false; // Por padr�o, o jogador pode interagir com o ba� (exceto quando bloqueado)
        }
    }
}
