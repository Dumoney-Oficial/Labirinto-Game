using UnityEngine;
using TMPro;

public class PlayerColeta : MonoBehaviour
{
    [Header("Configurações")]

    // Gerenciador que controla a quantidade coletada
    public ColetaveisManager gerenciador;

    // Texto que aparece quando o jogador está perto
    public TMP_Text textoColeta;

    // Guarda o coletável que está perto do jogador
    private Coletavel coletavelAtual;


    void Start()
    {
        // No começo o texto fica escondido
        textoColeta.gameObject.SetActive(false);
    }


    void Update()
    {
        // Se existe um coletável próximo
        // e o jogador apertar E
        if (coletavelAtual != null && Input.GetKeyDown(KeyCode.E))
        {
            // Adiciona 1 ao contador
            gerenciador.Coletar();

            // Esconde o objeto coletado
            coletavelAtual.gameObject.SetActive(false);

            // Limpa a referência
            coletavelAtual = null;

            // Esconde a mensagem de coleta
            textoColeta.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que o jogador encostou
        // possui o script Coletavel
        Coletavel coletavel = other.GetComponent<Coletavel>();

        if (coletavel != null)
        {
            // Guarda esse objeto como o coletável atual
            coletavelAtual = coletavel;

            // Mostra a mensagem na tela
            textoColeta.gameObject.SetActive(true);

            textoColeta.text = "Aperte a tecla E para coletar!";
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // Verifica se saiu de um coletável
        Coletavel coletavel = other.GetComponent<Coletavel>();

        if (coletavel != null && coletavel == coletavelAtual)
        {
            // Remove a referência
            coletavelAtual = null;

            // Esconde a mensagem
            textoColeta.gameObject.SetActive(false);
        }
    }
}