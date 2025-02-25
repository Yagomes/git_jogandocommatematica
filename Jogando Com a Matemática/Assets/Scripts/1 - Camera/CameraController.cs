using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController> // Controla a câmera para seguir o jogador usando Cinemachine.
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start()
    {
        SetPlayerCameraFollow();
    }


    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }
}

