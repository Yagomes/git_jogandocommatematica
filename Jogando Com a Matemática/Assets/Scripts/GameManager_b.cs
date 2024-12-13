using System.Collections.Generic;
using UnityEngine;

public class GameManager_b : MonoBehaviour
{
    public static GameManager_b instance;

    // Dicion�rio para armazenar o estado dos ba�s
    private Dictionary<string, bool> chestStates = new Dictionary<string, bool>();

    private void Awake()
    {
        // Verifica se j� existe uma inst�ncia do GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Mant�m o GameManager ao mudar de cena
        }
        else
        {
            Destroy(this.gameObject); // Destr�i inst�ncias duplicadas
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
}
