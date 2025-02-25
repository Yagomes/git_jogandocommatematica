using UnityEngine;

public class AreaEntrance : MonoBehaviour // Controla a entrada do jogador em uma área, iniciando transições e posicionamento.
{
    [SerializeField] private string transitionName;

    private void Start()
    {
        if (!string.IsNullOrEmpty(SceneManagement.Instance.SceneTransitionName) &&
            transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToClear();
        }
    }
}