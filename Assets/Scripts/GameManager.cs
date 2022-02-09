using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Classe principal
public class GameManager : MonoBehaviour
{
    
    #region JSON Values

    private List<Carta> cards; // cartas especificadas no JSON
    private float totalTime; // total de tempo no JSON
    private int totalAttempts = 0, successes = 0; // tentaivas a cada par virado | pares encontrados

    #endregion

    #region Cards Manipulation

    private List<Sprite> cardsLoaded = new List<Sprite>(); // Lista já com os pares

    private List<Card> cardsChecked = new List<Card>(); // Lista para comparar cartas viradas (máximo 2 cartas)

    #endregion

    #region Prefabs and Parents

    [SerializeField]
    private GameObject cardPrefab, playArea, winCard, loseCard, cardSlot;

    // cardPrefab: carta sem imagem
    // playArea: objeto pai para a cartas

    // winCard: carta mostrada no final quando ganha-se o jogo
    // loseCard: carta mostra no final quando perde-se o jogo
    // cardSlot: objeto pai para as cartas do game over

    #endregion

    #region UI
    
    [SerializeField]
    private TMP_Text time, attempts; // Contador de tempo | Contador de tentativas

    #endregion

    #region Instances

    [SerializeField]
    private ScreenManager screenManager; // Instacia do script de controle de tela

    #endregion

    void Update()
    {

        Timer();

    }

    void ReadSetup() { // Carrega os dados do JSON para as variaveis globais

        var configs = Resources.Load("json/configuracoes");
        
        var gameSetup = JsonUtility.FromJson<Config>(configs.ToString());

        cardsChecked.Clear();
        cardsLoaded.Clear();

        totalAttempts = 0;
        successes = 0;

        attempts.text = totalAttempts.ToString();

        cards = gameSetup.cartas;
        totalTime = gameSetup.tempoTotal;

    }

    void LoadCards() { // Inicia a lista de cartas para instancia

        foreach (var card in cards) {

            var cardSprite = Resources.Load<Sprite>("Textures/" + Path.GetFileNameWithoutExtension(card.imagemCarta));

            cardsLoaded.Add(cardSprite);
            cardsLoaded.Add(cardSprite);
            
        }
        
    }

    void SetCards() { // Instancia as cartas em ordem aleatória

        List<Sprite> orderedCards = new List<Sprite>();

        var cardsCount = cardsLoaded.Count;

        for (int i = 0; i < cardsCount; i++) {

            var pickIndex = Random.Range(0, cardsLoaded.Count);

            var card = Instantiate(cardPrefab, playArea.transform);
            card.GetComponent<Card>().SetSprite(cardsLoaded[pickIndex]);

            cardsLoaded.RemoveAt(pickIndex);

        }

    }

    void Timer() { // Contagem de tempo

        if (totalTime > 0) {

            totalTime -= Time.deltaTime;
            time.text = ((int)totalTime).ToString(); // Int para retirar a vírgula
            
        }
        else {

            EndGame();

        }

    }

    void IncrementAttempts() { // Acrescenta a tentativas e atualiza na UI

        totalAttempts++;

        attempts.text = totalAttempts.ToString();

    }

    void EndGame() { // Verifica as condiçoes para o fim do jogo e instancia a tela correta

        if (screenManager != null) {

            screenManager.ChangeScreen(screenManager.screens[2]);

            if (totalTime <= 0f) {

                Instantiate(loseCard, cardSlot.transform);

            }
            else {

                var gameOverCard = Instantiate(winCard, cardSlot.transform);
                var victoryCard = gameOverCard.GetComponent<VictoryCard>();

                victoryCard.SetSummary(totalTime, totalAttempts);

            }

        }
            

    }

    void AddCardToCompare(Card card) { // Adiciona a carta virada na lista de comparação (máximo 2 cartas)

        cardsChecked.Add(card);
        
        CheckCards();

    }

    void RemoveCardToCompare(Card card) { // Remove a carta escolhida da lista de comparação

        cardsChecked.Remove(card);

    }

    void CheckCards() { // Trata se as cartas são iguais ou não

        if (cardsChecked.Count == 2) {

            if (cardsChecked[0].cardImage.sprite == cardsChecked[1].cardImage.sprite) {
                
                successes++;

                Destroy(cardsChecked[1].gameObject);
                Destroy(cardsChecked[0].gameObject);

                RemoveCardToCompare(cardsChecked[1]);
                RemoveCardToCompare(cardsChecked[0]);

            }
            else {

                cardsChecked[1].TurnDown();
                cardsChecked[0].TurnDown();

            }

            IncrementAttempts();

        }

        if (successes == cards.Count) // Se o jogador descobrir todos os pares (sempre a metade da quantidade efetiva de cartas)
            EndGame();
        
    }

    void OnEnable() // Sempre que a tela do jogo aparecer, todos os valores serão atribuidos novamente apartir do JSON
    {

        ReadSetup();
        LoadCards();
        SetCards();

        Card.OnTurnCard -= ctx => AddCardToCompare(ctx);
        Card.OnUnturnCard -= ctx => RemoveCardToCompare(ctx);

        // ctx tem a referência da Carta (Card.cs)
        Card.OnTurnCard += ctx => AddCardToCompare(ctx); 
        Card.OnUnturnCard += ctx => RemoveCardToCompare(ctx);
        
    }

    void OnDisable()
    {
        
        // ctx tem a referência da Carta (Card.cs)
        Card.OnTurnCard -= ctx => AddCardToCompare(ctx);
        Card.OnUnturnCard -= ctx => RemoveCardToCompare(ctx);

    }

    void OnDestroy()
    {
        
        // ctx tem a referência da Carta (Card.cs)
        Card.OnTurnCard -= ctx => AddCardToCompare(ctx);
        Card.OnUnturnCard -= ctx => RemoveCardToCompare(ctx);

    }

}