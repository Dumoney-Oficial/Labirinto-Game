
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [Header("Alvos")]
    // Personagem que a câmera irá seguir
    public Transform alvoPlayer;

    [Header("Configurações de Distância")]
    public float distanciaX_Z = 5f;
    public float alturaY = 3f;

    [Header("Suavidade")]
    public float suavidadeSeguimento = 5f;

    [Header("Colisão da Câmera")]
    public float distanciaMinima = 0.5f;
    public float margemColisao = 0.2f;

    // Permite selecionar várias Layers no Inspector
    public LayerMask camadasObstaculos;

    // Controla a visão atrás ou à frente do personagem
    private bool olhandoPorTras = true;

    void LateUpdate()
    {
        if (alvoPlayer == null) return;

        // Alterna a visão usando a tecla Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            olhandoPorTras = !olhandoPorTras;
        }

        // Calcula a posição desejada da câmera
        Vector3 posicaoDesejada;

        if (olhandoPorTras)
        {
            // Câmera atrás do personagem
            posicaoDesejada = alvoPlayer.position
                - (alvoPlayer.forward * distanciaX_Z)
                + (Vector3.up * alturaY);
        }
        else
        {
            // Câmera à frente do personagem
            posicaoDesejada = alvoPlayer.position
                + (alvoPlayer.forward * distanciaX_Z)
                + (Vector3.up * alturaY);
        }

        // Define o ponto que a câmera deve observar
        Vector3 pontoFoco = alvoPlayer.position
            + (Vector3.up * (alturaY * 0.5f));

        // Calcula a direção entre o jogador e a câmera
        Vector3 direcao = posicaoDesejada - pontoFoco;

        float distancia = direcao.magnitude;

        direcao.Normalize();

        // Verifica se existe alguma parede ou obstáculo
        RaycastHit hit;

        if (Physics.Raycast(
            pontoFoco,
            direcao,
            out hit,
            distancia,
            camadasObstaculos))
        {
            // Aproxima a câmera para evitar atravessar obstáculos
            posicaoDesejada = hit.point
                - (direcao * margemColisao);

            // Garante uma distância mínima
            Vector3 posicaoRelativa = posicaoDesejada - pontoFoco;

            if (posicaoRelativa.magnitude < distanciaMinima)
            {
                posicaoDesejada = pontoFoco
                    + direcao * distanciaMinima;
            }
        }

        // Move a câmera suavemente
        transform.position = Vector3.Lerp(
            transform.position,
            posicaoDesejada,
            suavidadeSeguimento * Time.deltaTime
        );

        // Faz a câmera olhar para o personagem
        transform.LookAt(pontoFoco);
    }
}
