using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear o cursor no centro da tela
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limitar a rotação do ângulo vertical entre -90 e 90 graus

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Aplicar rotação apenas no eixo local X da câmera

        playerBody.Rotate(Vector3.up * mouseX); // Rotacionar o corpo do jogador no eixo Y de acordo com o movimento do mouse
    }
}
