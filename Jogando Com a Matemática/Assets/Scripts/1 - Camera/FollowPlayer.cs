using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour // Faz a c�mera seguir o jogador "Player_Boy" na cena usando Cinemachine.
{
    private void Start()
    {
        // Encontra o objeto Player_Boy na cena
        GameObject player = GameObject.Find("Player_Boy");

        if (player != null)
        {
            // Acessa o componente CinemachineVirtualCamera
            CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();

            if (virtualCamera != null)
            {
                // Define o Player_Boy como alvo de Follow
                virtualCamera.Follow = player.transform;
            }
            else
            {
                Debug.LogWarning("CinemachineVirtualCamera n�o foi encontrado neste GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player_Boy n�o foi encontrado na cena!");
        }
    }
}
