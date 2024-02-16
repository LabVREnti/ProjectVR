using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverTextura : MonoBehaviour
{
    public float velocidadX = 0.1f; // Velocidad de movimiento en el eje X
    public float velocidadY = 0.1f; // Velocidad de movimiento en el eje Y

    Renderer rend;
    float offsetX = 0f;
    float offsetY = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offsetX += velocidadX * Time.deltaTime;
        offsetY += velocidadY * Time.deltaTime;
        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}