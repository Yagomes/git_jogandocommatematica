using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController> // Controla a c�mera para seguir o jogador usando Cinemachine.
{
  
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start()
    {
        SetPlayerCameraFollow();
    }

    public void SetPlayerCameraFollow()
    {
        // Valor salvo no PlayerPrefs
        string aluno_genero = PlayerPrefs.GetString("aluno_genero");

        GameObject player = null;

        // Procurando mesmo se estiver desativado e em qualquer cena (inclusive DontDestroy)
        GameObject[] allPlayers = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in allPlayers)
        {
            if (go.name == "Player_Boy" && aluno_genero == "Masculino")
            {
                go.SetActive(true);
                player = go;
            }
            else if (go.name == "Player_Girl" && aluno_genero == "Feminino")
            {
                go.SetActive(true);
                player = go;
            }
        }

        if (player == null)
        {
            Debug.LogError("Player n�o encontrado!");
            return;
        }
        /*
        // Agora setando o Follow da c�mera normalmente
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = player.transform;*/
    }
}



