using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour // Faz a câmera seguir o jogador certo na cena usando Cinemachine.
{
    private void Start()
    {
        // Pegando o gênero salvo no PlayerPrefs
        string aluno_genero = PlayerPrefs.GetString("aluno_genero");

        // Busca por todos objetos mesmo desativados (incluindo DontDestroy)
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        GameObject player = null;

        foreach (GameObject obj in allObjects)
        {
            if (aluno_genero == "Masculino" && obj.name == "Player_Boy")
            {
                obj.SetActive(true);
                player = obj;
                break;
            }
            else if (aluno_genero == "Feminino" && obj.name == "Player_Girl")
            {
                obj.SetActive(true);
                player = obj;
                break;
            }
        }

        if (player != null)
        {
            // Acessa o componente CinemachineVirtualCamera
            CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();

            if (virtualCamera != null)
            {
                // Define o player certo como alvo de Follow
                virtualCamera.Follow = player.transform;
            }
            else
            {
                Debug.LogWarning("CinemachineVirtualCamera não foi encontrado neste GameObject.");
            }
        }
        else
        {
            Debug.LogError("Nenhum player encontrado com base no gênero salvo.");
        }
    }
}
