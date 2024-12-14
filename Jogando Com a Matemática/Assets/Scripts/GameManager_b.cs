using System.Collections.Generic;
using UnityEngine;
public class GameManager_b : MonoBehaviour
{
    public static GameManager_b instance;

    // Dicion�rio para armazenar o estado dos ba�s
    private Dictionary<string, bool> chestStates = new Dictionary<string, bool>();

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

    // Salva o estado de um ba�
    public void SetChestState(string chestID, bool isOpen)
    {
        chestStates[chestID] = isOpen;
    }

    // Recupera o estado de um ba�
    public bool GetChestState(string chestID)
    {
        return chestStates.ContainsKey(chestID) && chestStates[chestID];
    }

    // Redefine todos os ba�s para o estado "fechado"
    public void ResetChestStates()
    {
        var keys = new List<string>(chestStates.Keys);
        foreach (var key in keys)
        {
            chestStates[key] = false;
        }
    }
}
