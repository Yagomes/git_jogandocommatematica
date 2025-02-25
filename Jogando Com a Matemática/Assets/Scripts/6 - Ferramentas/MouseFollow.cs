using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private void Update()
    {
        FaceMouse();
    }

    private void FaceMouse()
    {
        // Pega a posi��o do mouse na tela
        Vector3 mousePosition = Input.mousePosition;

        // Verifique se a posi��o do mouse est� dentro dos limites da tela
        if (mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
            mousePosition.y >= 0 && mousePosition.y <= Screen.height)
        {
            // Converte a posi��o da tela para coordenadas do mundo
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Fixando a posi��o Z do mouse (normalmente voc� s� precisa se preocupar com X e Y no 2D)
            mousePosition.z = 0;

            // Calcula a dire��o entre o objeto e o mouse
            Vector2 direction = (Vector2)mousePosition - (Vector2)transform.position;

            // Calcula o �ngulo necess�rio para rotacionar o objeto
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Aplica a rota��o no eixo Z (em jogos 2D, usamos o eixo Z para rotacionar)
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
       
    }
}
