using UnityEngine;
using TMPro;

// Classe que representa o cartão de vitória
public class VictoryCard : MonoBehaviour
{
    
    [SerializeField]
    private TMP_Text totalTime, totalAttempts;
    
    public void SetSummary(float time, int attempts) { // Inicia os valores do fim do jogo

        totalTime.text = ((int)time).ToString();
        totalAttempts.text = attempts.ToString();

    }

}