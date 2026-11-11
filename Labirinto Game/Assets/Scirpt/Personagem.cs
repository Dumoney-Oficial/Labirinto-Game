using UnityEngine;
using UnityEngine.InputSystem; // Importante para o novo Input System

[RequireComponent(typeof(Rigidbody))]
public class MoverCacto : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidadeMovimento = 5f;
    public float multiplicadorCorrida = 2f;
    public float velocidadeRotacao = 150f;

    [Header("Configurações do Pulo")]
    public float forcaPulo = 5f;
    public LayerMask layerChao;
    public Transform checadorChao;
    public float raioChecagem = 0.2f;

    [Header("Configurações do Disparo")]
    public GameObject prefabProjetil;
    public Transform pontoDeDisparo;
    public float forcaArremesso = 15f;
    public float tempoDeVidaProjetil = 3f;

    // Componentes
    private Animator meuAnimator;
    private Rigidbody rb;

    // Entradas
    private Vector2 inputsMovimento;
    private bool estaCorrendo;
    private bool estaNoChao;
    private bool querPular;

    void Start()
    {
        meuAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Congela rotações nos eixos X e Z para o objeto não tombar fisicamente
        //   rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // 1. CHECAGEM DO CHÃO
        if (checadorChao != null)
        {
            estaNoChao = Physics.CheckSphere(checadorChao.position, raioChecagem, layerChao);
        }

        // 2. CAPTURA DE ENTRADAS (Novo Input System via leitura direta do Keyboard e Mouse)
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        if (keyboard != null)
        {
            // Leitura dos eixos WASD / Setas
            float x = 0f;
            float y = 0f;

            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) y += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) y -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) x += 1f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) x -= 1f;

            inputsMovimento = new Vector2(x, y);

            // Corrida
            estaCorrendo = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;

            // Pulo
            if (keyboard.spaceKey.wasPressedThisFrame && estaNoChao)
            {
                querPular = true;
            }
        }

        // Tiro no clique esquerdo do mouse
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            if (meuAnimator != null)
            {
                meuAnimator.SetTrigger("Shot");
            }
        }

        // 3. CONTROLE DE ANIMAÇÕES
        bool estaMovendo = inputsMovimento.sqrMagnitude > 0.01f;
        if (meuAnimator != null)
        {
            meuAnimator.SetBool("isWalking", estaMovendo);
            meuAnimator.SetBool("isRunning", estaMovendo && estaCorrendo);
        }
    }

    void FixedUpdate()
    {
        // 1. Rotação Física
        float giro = inputsMovimento.x * velocidadeRotacao * Time.fixedDeltaTime;
        Quaternion mudancaRotacao = Quaternion.Euler(0f, giro, 0f);
        rb.MoveRotation(rb.rotation * mudancaRotacao);

        // 2. Movimentação Física com Zera de Atrito Lateral
        float multiplicador = estaCorrendo ? multiplicadorCorrida : 1f;
        Vector3 direcaoAlvo = transform.forward * (inputsMovimento.y * velocidadeMovimento * multiplicador);

        // Mantém a velocidade vertical da gravidade (Y) intocada
        Vector3 velocidadeFinal = new Vector3(direcaoAlvo.x, rb.linearVelocity.y, direcaoAlvo.z);

        // Força gravidade extra se estiver no ar para não "flutuar"
        if (!estaNoChao && rb.linearVelocity.y < 0.1f)
        {
            velocidadeFinal.y += Physics.gravity.y * 1.5f * Time.fixedDeltaTime;
        }

        rb.linearVelocity = velocidadeFinal;

        // 3. Pulo
        if (querPular)
        {
            // Zera a velocidade Y antes de pular para evitar somar forças estranhas
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
            querPular = false;
        }
    }

    public void DispararProjetil()
    {
        if (prefabProjetil == null)
        {
            Debug.LogWarning("Prefab do Projétil não foi atribuído no Inspector!");
            return;
        }

        Vector3 posicaoOrigem = pontoDeDisparo != null ? pontoDeDisparo.position : transform.position + transform.forward;
        Quaternion rotacaoOrigem = pontoDeDisparo != null ? pontoDeDisparo.rotation : transform.rotation;

        GameObject projetil = Instantiate(prefabProjetil, posicaoOrigem, rotacaoOrigem);

        Rigidbody rbProjetil = projetil.GetComponent<Rigidbody>();
        if (rbProjetil != null)
        {
            Vector3 direcao = pontoDeDisparo != null ? pontoDeDisparo.forward : transform.forward;
            rbProjetil.AddForce(direcao * forcaArremesso, ForceMode.Impulse);
        }

        Destroy(projetil, tempoDeVidaProjetil);
    }

    private void OnDrawGizmosSelected()
    {
        if (checadorChao != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(checadorChao.position, raioChecagem);
        }
    }
}