using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float runSpeed = 18f; // Velocidade de corrida quando Shift é pressionado
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform cameraTransform; // Referência à transformação da câmera
    public Transform groundCheck; // Referência ao ponto de verificação no chão
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Certificar-se de que groundCheck esteja atribuído antes de usar
        if (groundCheck == null)
        {
            Debug.LogError("Ground check transform não foi atribuído no Inspector!");
        }

        // Se a câmera não estiver definida, usar a câmera principal
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Verificar se o jogador está no chão
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Resetar a velocidade vertical quando estiver no chão para evitar acumulação
            }
        }
        else
        {
            Debug.LogError("Ground check transform não foi atribuído no Inspector!");
        }

        // Capturar entrada do jogador para movimento horizontal e vertical
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Determinar a direção de movimento com base na rotação da câmera
        Vector3 move = Vector3.zero;
        if (cameraTransform != null)
        {
            // Obter a direção forward da câmera, projetado no plano XZ (sem movimento vertical)
            Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            move = cameraForward * z + cameraTransform.right * x;
        }
        else
        {
            move = transform.forward * z + transform.right * x;
        }

        // Determinar a velocidade atual baseada na tecla Shift
        float currentSpeed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? runSpeed : speed;

        // Aplicar movimento
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Lógica para pular
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicar a gravidade
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
