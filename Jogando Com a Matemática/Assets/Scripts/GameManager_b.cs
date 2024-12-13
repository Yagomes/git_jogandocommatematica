using System.Collections.Generic;
using UnityEngine;

public class GameManager_b : MonoBehaviour
{
    public static GameManager_b instance;

    // Dicionário para armazenar o estado dos baús
    private Dictionary<string, bool> chestStates = new Dictionary<string, bool>();

    private void Awake()
    {
        // Verifica se já existe uma instância do GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Mantém o GameManager ao mudar de cena
        }
        else
        {
            Destroy(this.gameObject); // Destrói instâncias duplicadas
        }
    }

    // Salva o estado de um baú
    public void SetChestState(string chestID, bool isOpen)
    {
        chestStates[chestID] = isOpen;
    }

    // Recupera o estado de um baú
    public bool GetChestState(string chestID)
    {
        return chestStates.ContainsKey(chestID) && chestStates[chestID];
    }
}
