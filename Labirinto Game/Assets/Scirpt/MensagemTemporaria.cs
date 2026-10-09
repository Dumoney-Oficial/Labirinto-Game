using UnityEngine;
using TMPro;

public class MensagemTemporaria : MonoBehaviour
{
    // Texto que aparecerá na tela
    public TMP_Text textoMensagem;

    // Tempo até a mensagem desaparecer
    public float tempoParaSumir = 5f;

    void Start()
    {
        // Ativa o texto
        textoMensagem.gameObject.SetActive(true);

        // Define a mensagem
        textoMensagem.text = "Colete todos os Guarda-Chuvas!!!";

        // Espera 5 segundos e esconde o texto
        Invoke("EsconderMensagem", tempoParaSumir);
    }

    void EsconderMensagem()
    {
        // Esconde a mensagem da tela
        textoMensagem.gameObject.SetActive(false);
    }
}