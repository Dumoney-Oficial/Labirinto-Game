using UnityEngine;
using TMPro;

public class ColetaveisManager : MonoBehaviour
{
    [Header("Configuração dos Coletáveis")]

    // Nome que aparece no contador
    public string nomeColetavel = "Guarda-Chuvas";

    // Quantidade necessária para destrancar as portas
    public int quantidadeNecessaria = 10;

    [Header("Interface")]

    // Texto que mostra a quantidade coletada
    public TMP_Text textoContador;

    // Informa se as portas já podem ser abertas
    public bool portasDesbloqueadas = false;

    // Quantidade que o jogador já coletou
    private int quantidadeColetada = 0;


    void Start()
    {
        // As portas começam trancadas
        portasDesbloqueadas = false;

        // Atualiza o contador no início
        AtualizarContador();
    }


    public void Coletar()
    {
        // Impede que a contagem ultrapasse o necessário
        if (quantidadeColetada >= quantidadeNecessaria)
        {
            return;
        }

        // Adiciona um coletável
        quantidadeColetada++;

        // Atualiza o contador da tela
        AtualizarContador();

        // Verifica se já coletou todos os guarda-chuvas
        if (quantidadeColetada >= quantidadeNecessaria)
        {
            // Agora todas as portas podem ser abertas
            portasDesbloqueadas = true;
        }
    }


    void AtualizarContador()
    {
        // Mostra, por exemplo: Guarda-Chuvas 5/10
        textoContador.text = nomeColetavel + " " +
                             quantidadeColetada + "/" +
                             quantidadeNecessaria;
    }
}