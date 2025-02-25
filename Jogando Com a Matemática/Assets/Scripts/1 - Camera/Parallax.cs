using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour // Aplica efeito de parallax no fundo com base no movimento da câmera.
{
    [SerializeField] private float parallaxOffeset = -0.15f;

    private Camera cam;
    private Vector2 startPos;
    private Vector2 travel => (Vector2)cam.transform.position - startPos;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        startPos = transform.position;
    }


    private void FixedUpdate()
    {
        transform.position = startPos + travel * parallaxOffeset;
    }
}
