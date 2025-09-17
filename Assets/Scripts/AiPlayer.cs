
using UnityEngine;
using System.Linq;

using TMPro;
public class AiPlayer : Player
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI[] aiTextPrefab;
    private Vector2 startpos;
    //private int callRound;
    protected  void Awake()
    {
         startpos = Vector2.zero;
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
        // if (handData == null || handData.Count == 0)
        // {
        //     Debug.Log("ai er hate card sesh");
        //     return null;
        //    
        // }
        
        // if (leadSuit != Suit.None)
        // {
        
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
            Debug.Log($"{playerName} chose {chosenCard.rank} of {chosenCard.type}");
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
    protected override void CreateVisualHand()
    {
        return;
    }
    public  void AiCreateVisualCharacter(Card card)
    {
       // foreach (var c in hand) if(c!=null) c.gameObject.SetActive(true);
    
        if (aiTextPrefab == null || aiTextPrefab.Length == 0) return;
        int aiIndex = playerNumber - 1;
        if (aiIndex < 0 || aiIndex >= aiTextPrefab.Length) return;
        var newText = aiTextPrefab[aiIndex];
        newText.text = $"{playerName} has played {card.rank} of {card.type}";
       // Debug.Log($"{playerName} has played {card.rank} of {card.type}");
    }
}
