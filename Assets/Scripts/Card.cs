using UnityEngine;
using UnityEngine.UI;

// Classe individual para cada carta
public class Card : MonoBehaviour
{

    #region Card Components

    [HideInInspector]
    public Animator cardAnimator;

    [HideInInspector]
    public Image cardImage;

    [SerializeField]
    private GameObject frontSide;

    #endregion

    #region Events

    public delegate void TurnCard(Card card);
    public static event TurnCard OnTurnCard;
    public static event TurnCard OnUnturnCard;

    #endregion

    void Awake()
    {
        
        cardAnimator = GetComponent<Animator>();
        cardImage = frontSide.GetComponent<Image>();

    }

    public void TurnUp() { // Chamado quando clicado sobre uma carta para baixo

        cardAnimator.Play("TurnUp");
        
    }

    public void AddCompareCard() { // Ao final da animação de virar da carta, este evento é disparado

        if (OnTurnCard == null)
            return;
            
        OnTurnCard(this);

    }

    public void TurnDown() { // Chamado quando clicado sobre uma carta para cima

        cardAnimator.Play("TurnDown");

    }

    public void RemoveCompareCard() { // Ao final da animação de virar da carta, este evento é disparado

        if (OnUnturnCard == null)
            return;

        OnUnturnCard(this);

    }

    public void SetSprite(Sprite sprite) { // Inicializar imagem da carta (prefab inicialmente não tem sprite)

        cardImage.sprite = sprite;

    }
}