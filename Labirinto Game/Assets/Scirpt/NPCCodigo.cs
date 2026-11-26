
using UnityEngine;

public class NPCCodigo : MonoBehaviour
{
    [Header("Configurações do Player")]
    public Transform player;

    [Header("Distância e Movimento")]
    public float detectionRadius = 8.0f;
    public float moveSpeed = 3.0f;
    public float rotationSpeed = 10.0f;

    [Header("Configuração de Layers")]
    // Permite selecionar uma ou mais Layers pelo Inspector.
    public LayerMask groundLayers;

    [Header("Componentes")]
    public Animator animator;
    public Rigidbody rb;

    // Indica se o NPC está no chão.
    private bool isGrounded = false;

    // Indica se o NPC está perseguindo o jogador.
    private bool isChasing = false;

    // Start é executado uma vez no início do jogo.
    void Start()
    {
        // Procura os componentes automaticamente caso não estejam preenchidos.
        if (animator == null)
            animator = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        // Procura o jogador pela Tag Player.
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    // Update verifica a distância do jogador.
    void Update()
    {
        // Se não encontrar o jogador, interrompe a perseguição.
        if (player == null)
        {
            isChasing = false;

            if (animator != null)
                animator.SetBool("attack", false);

            return;
        }

        // Calcula a distância entre o NPC e o jogador.
        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position
        );

        // Verifica se o jogador está dentro do raio de detecção.
        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;

            // Ativa a animação existente no Animator.
            if (animator != null)
                animator.SetBool("attack", true);
        }
        else
        {
            isChasing = false;

            // Desativa a animação quando o jogador se afasta.
            if (animator != null)
                animator.SetBool("attack", false);
        }
    }

    // FixedUpdate controla o movimento físico do NPC.
    void FixedUpdate()
    {
        // Verifica se o Rigidbody existe.
        if (rb == null)
            return;

        // Se não estiver perseguindo ou não estiver no chão, para.
        if (!isChasing || !isGrounded || player == null)
        {
            StopMovement();
            return;
        }

        // Calcula a direção do jogador.
        Vector3 moveDirection = player.position - rb.position;

        // Ignora a diferença de altura.
        moveDirection.y = 0f;

        // Evita erros quando o NPC estiver muito próximo do jogador.
        if (moveDirection.sqrMagnitude < 0.01f)
        {
            StopMovement();
            return;
        }

        // Normaliza a direção do movimento.
        moveDirection.Normalize();

        // Calcula a rotação para olhar na direção do jogador.
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        // Gira o NPC suavemente.
        rb.MoveRotation(
            Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            )
        );

        // Mantém a velocidade vertical e altera apenas X e Z.
        Vector3 currentVelocity = rb.linearVelocity;

        currentVelocity.x = moveDirection.x * moveSpeed;
        currentVelocity.z = moveDirection.z * moveSpeed;

        rb.linearVelocity = currentVelocity;
    }

    // Para o movimento horizontal sem alterar a gravidade.
    void StopMovement()
    {
        Vector3 currentVelocity = rb.linearVelocity;

        currentVelocity.x = 0f;
        currentVelocity.z = 0f;

        rb.linearVelocity = currentVelocity;
    }

    // Verifica continuamente se o NPC está encostando no chão.
    private void OnCollisionStay(Collision collision)
    {
        // Verifica se o objeto pertence a uma das Layers selecionadas.
        if ((groundLayers.value & (1 << collision.gameObject.layer)) == 0)
            return;

        // Verifica se o contato é com uma superfície de chão.
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    // Atualiza a condição quando o NPC deixa de tocar o chão.
    private void OnCollisionExit(Collision collision)
    {
        if ((groundLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            isGrounded = false;
        }
    }

    // Desenha o raio de detecção na aba Scene.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRadius
        );
    }
}