using UnityEngine;
using TMPro;

public class ColetaveisManager : MonoBehaviour
{
    [Header("Configuração dos Coletáveis")]

    public string nomeColetavel = "Guarda-Chuvas";
    public int quantidadeNecessaria = 10;

    [Header("Interface")]

    public TMP_Text textoContador;
    public TMP_Text textoPorta;

    [Header("Portas")]

    // Permite configurar várias portas
    public GameObject[] portas;

    // Quantidade de coletáveis encontrados
    private int quantidadeColetada = 0;

    // Impede que a abertura aconteça mais de uma vez
    private bool portaAberta = false;


    void Start()
    {
        AtualizarContador();

        // Esconde a mensagem no início
        textoPorta.gameObject.SetActive(false);
    }


    public void Coletar()
    {
        // Adiciona um coletável
        quantidadeColetada++;

        // Atualiza o contador
        AtualizarContador();

        // Verifica se coletou todos os itens necessários
        if (quantidadeColetada >= quantidadeNecessaria && !portaAberta)
        {
            portaAberta = true;

            // Mostra a mensagem na tela
            textoPorta.gameObject.SetActive(true);
            textoPorta.text = "As portas foram abertas!";

            // Faz todas as portas desaparecerem
            for (int i = 0; i < portas.Length; i++)
            {
                if (portas[i] != null)
                {
                    portas[i].SetActive(false);
                }
            }

            // Esconde a mensagem depois de 3 segundos
            Invoke("EsconderMensagemPorta", 3f);
        }
    }


    void AtualizarContador()
    {
        textoContador.text = nomeColetavel + " " +
                             quantidadeColetada + "/" +
                             quantidadeNecessaria;
    }


    void EsconderMensagemPorta()
    {
        textoPorta.gameObject.SetActive(false);
    }
}