using UnityEngine;
using TMPro;

public class PortaInterativa : MonoBehaviour
{
    [Header("Referências")]

    // Gerenciador responsável pelas coletas
    public ColetaveisManager gerenciador;

    // Texto que aparece na tela
    public TMP_Text textoInteracao;

    // Objeto que será girado para abrir a porta
    public Transform portaMovel;


    [Header("Configuração da Porta")]

    // Ângulo de abertura da porta
    public float anguloAbertura = 90f;

    // Velocidade da abertura e do fechamento
    public float velocidadeRotacao = 3f;


    // Verifica se o jogador está perto
    private bool jogadorPerto = false;

    // Verifica se a porta está aberta
    private bool aberta = false;

    // Guarda as rotações da porta
    private Quaternion rotacaoFechada;
    private Quaternion rotacaoAberta;
    private Quaternion rotacaoAlvo;

    // Mantém apenas uma porta ativa para interação por vez
    private static PortaInterativa portaAtual;


    void Start()
    {
        // Guarda a posição de rotação inicial da porta
        if (portaMovel != null)
        {
            rotacaoFechada = portaMovel.localRotation;

            // Calcula a rotação para abrir 90 graus
            rotacaoAberta = rotacaoFechada *
                            Quaternion.Euler(0f , 0f , anguloAbertura);

            // Começa com a porta fechada
            rotacaoAlvo = rotacaoFechada;
        }

        // Esconde o texto no início
        if (textoInteracao != null)
        {
            textoInteracao.gameObject.SetActive(false);
        }
    }


    void Update()
    {
        // Gira a porta suavemente até a posição desejada
        if (portaMovel != null)
        {
            portaMovel.localRotation = Quaternion.Slerp(
                portaMovel.localRotation,
                rotacaoAlvo,
                Time.deltaTime * velocidadeRotacao
            );
        }

        // Só a porta próxima do jogador pode receber interação
        if (!jogadorPerto || portaAtual != this)
        {
            return;
        }

        if (gerenciador == null || textoInteracao == null)
        {
            return;
        }

        // Mostra o texto de interação
        textoInteracao.gameObject.SetActive(true);

        // Se ainda faltam coletáveis, a porta continua trancada
        if (!gerenciador.portasDesbloqueadas)
        {
            textoInteracao.text =
                "Porta trancada! Colete todos os Guarda-Chuvas.";

            return;
        }

        // Atualiza o texto conforme o estado da porta
        if (aberta)
        {
            textoInteracao.text = "Clique E para fechar";
        }
        else
        {
            textoInteracao.text = "Clique E para abrir";
        }

        // Verifica se o jogador apertou E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Inverte o estado da porta
            aberta = !aberta;

            // Escolhe a rotação de abertura ou fechamento
            if (aberta)
            {
                rotacaoAlvo = rotacaoAberta;
                textoInteracao.text = "Clique E para fechar";
            }
            else
            {
                rotacaoAlvo = rotacaoFechada;
                textoInteracao.text = "Clique E para abrir";
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem entrou na área foi o jogador
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            portaAtual = this;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // Verifica se o jogador saiu da área
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;

            // Esconde o texto se esta era a porta em interação
            if (portaAtual == this)
            {
                portaAtual = null;

                if (textoInteracao != null)
                {
                    textoInteracao.gameObject.SetActive(false);
                }
            }
        }
    }
}