
using TMPro;
using UnityEngine;

public class HumanPlayer : Player
{
    [SerializeField] TextMeshProUGUI _updateText;
    [SerializeField]private GameManager gameManager;

    protected  void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    public void OnCardChosen(Card chosenCard)
    {
        //if (chosenCard == null || !hand.Contains(chosenCard)) return;
        if (chosenCard == null) return;
        Debug.Log(chosenCard+"iS EXECUting perfeectly");
        
        gameManager.OnCardPlayed(this, chosenCard);
        chosenCard.gameObject.SetActive(false);
         RemoveCardFromHand(chosenCard);
         gameManager.WaitingForHuman = false;

    }
}

  