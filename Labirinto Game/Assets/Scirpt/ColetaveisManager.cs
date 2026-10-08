using UnityEngine;
using TMPro;

public class ColetaveisManager : MonoBehaviour
{
    [Header("Configuração dos Coletáveis")]

    // Nome que aparecerá na tela
    public string nomeColetavel = "Guarda-Chuvas";

    // Quantidade necessária para abrir a porta
    public int quantidadeNecessaria = 10;


    [Header("Interface")]

    // Texto que mostra, por exemplo:
    // Guarda-Chuvas 5/10
    public TMP_Text textoContador;

    // Texto da mensagem da porta
    public TMP_Text textoPorta;


    [Header("Porta")]

    // Arraste a porta para este campo no Inspector
    public GameObject porta;


    // Quantidade que o jogador já coletou
    private int quantidadeColetada = 0;


    void Start()
    {
        // Atualiza o texto no começo do jogo
        AtualizarContador();

        // Esconde a mensagem da porta
        textoPorta.gameObject.SetActive(false);
    }


    public void Coletar()
    {
        // Adiciona 1 coletável
        quantidadeColetada++;

        // Atualiza o texto
        AtualizarContador();


        // Verifica se chegou na quantidade necessária
        if (quantidadeColetada >= quantidadeNecessaria)
        {
            // Mostra a mensagem
            textoPorta.gameObject.SetActive(true);

            textoPorta.text = "A porta foi aberta!";

            // Faz a porta desaparecer
            porta.SetActive(false);
        }
    }


    void AtualizarContador()
    {
        // Atualiza o texto da tela
        textoContador.text = nomeColetavel + " " +
                             quantidadeColetada + "/" +
                             quantidadeNecessaria;
    }
}