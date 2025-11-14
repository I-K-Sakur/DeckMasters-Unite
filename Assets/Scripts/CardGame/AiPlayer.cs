
using UnityEngine;
using System.Linq;

using TMPro;
public class AiPlayer : Player
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI[] aiTextPrefab;
    private Card _cardClone;
   // private Vector2 startpos;
    //private int callRound;
    protected  void Awake()
    {
        
         //startpos = Vector2.zero;
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
    
    }
    
    //bidding for ai
    public int AiPlayerCallRound()
    {
        return Random.Range(2, 8);
    }

    public override Card PlayerToChoseCard(string name, Suit leadSuit, int highestScore)
    {
        
        Debug.Log($"AI {playerName} has {handData.Count} cards at Start()");
        
            var sameSuit = handData.Where(card => card.type == leadSuit).ToList();
            Card chosenCard = null;
            if (sameSuit.Count > 0)
            {
                chosenCard = sameSuit.OrderByDescending(c => c.value).FirstOrDefault();
            }
            //no leadsuit,play spade
            //Debug.Log("ai kaj korteche");
            if (chosenCard == null)
            {
                var spades = handData.Where(card => card.type == Suit.Spade).ToList();
                if (spades.Count > 0)
                    chosenCard = spades.OrderBy(c => c.value).First();
            }
       
            //No spade== lowest khela lagbe
            if(chosenCard == null)
                chosenCard = handData.OrderBy(c=>c.value).First();
        //RemoveCardFromHand(chosenCard);
        if (chosenCard != null)
        {
            Debug.Log($" {playerName} chose {chosenCard.rank} of {chosenCard.type}");
            handData.Remove(chosenCard);
            RemoveCardFromHand(chosenCard);
           AiCreateVisualCharacter(chosenCard);
        } 
        // if(hasPlayed) return null
    // hasPlayed = true;
        return chosenCard;
    }

    protected override void RemoveCardFromHand(Card card)
    {
        handData.Remove(card);
    }

    private Vector2 Position(int playerNumber)
    {
        Vector2 pos = Vector2.zero;
        switch (playerNumber)
        {
            case 1:
                 pos = new Vector2(transform.position.x-15f, transform.position.y+3f);
                break;
            case 2:
                 pos = new Vector2(transform.position.x, transform.position.y-15f);
                break;
            case 3:
                 pos = new Vector2(transform.position.x+15f, transform.position.y+5f);
                break; 
        }

        return pos;
    }
    protected override void CreateVisualHand()
    {
        return;
    }
    public  void AiCreateVisualCharacter(Card cardPref)
    {
        if(_cardClone!=null)
            Destroy(_cardClone.gameObject);
        if (cardPref == null) return;
        Vector3 spawnPosition = Position(playerNumber);
       // foreach (var c in hand) if(c!=null) c.gameObject.SetActive(true);
       _cardClone= Instantiate(cardPref,spawnPosition,Quaternion.identity);
       //newCard.transform.localScale = Vector3.one;
        if (aiTextPrefab == null || aiTextPrefab.Length == 0) return;
        int aiIndex = playerNumber - 1;
        if (aiIndex < 0 || aiIndex >= aiTextPrefab.Length) return;
        var newText = aiTextPrefab[aiIndex];
        newText.text = $"{playerName} Score: {score} + {PlayerName} has played {cardPref.rank} of {cardPref.type}";
        // Debug.Log($"{playerName} has played {card.rank} of {card.type}");
    }
}
