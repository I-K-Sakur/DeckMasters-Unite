using UnityEngine;
using System.Collections.Generic;
public class Player : MonoBehaviour
{
    [SerializeField] protected string playerName;
    [SerializeField] protected int score;
    [SerializeField] protected int tempScore;
    protected List<Card> hand;
    protected List<Card> handData = new List<Card>();
    [SerializeField] protected int call;
    [SerializeField]protected DeckManager deckManager;
    [SerializeField] protected int playerNumber;
    protected float PosValueX=-8f;
    private Suit _tempSuit;
    //protected List<Card> tempCards;
    public List<Card> Hand => hand;
    public string PlayerName => playerName;
    public int Call
    {
        get { return call; } 
        set { call = value; } 
    }
    protected virtual void Start()
    {
        hand = new List<Card>();
        if (deckManager != null)
        {
            if (deckManager.Players == null || deckManager.Players.Count <= playerNumber)
            {
                return;
            }
            //hand = new List<Card>(deckManager.players[_playerNumber]);
            //AllCards();
        }
        else
        {
            Debug.LogError($"{gameObject.name} has not deck assigned");
        }
    }
    public void InitPlayer(DeckManager dec)
    {
        deckManager = dec;
        if(dec ==null || dec.Players==null) return;
        if(playerNumber<0 || playerNumber>=deckManager.Players.Count) return;
        deckManager = dec;
        //hand = new List<Card>(dec.players[_playerNumber]);
        hand = new List<Card>();
        foreach (var cardPref in deckManager.Players[playerNumber])
        {
            handData.Add(cardPref);
        }
      
        CreateVisualHand();
      
    }


    protected virtual void CreateVisualHand()
    {
        //foreach (var c in hand) if(c!=null) c.gameObject.SetActive(true);
        //handData.Clear();
        float spacing = 1.8f;
        Debug.Log("Total card in hand "+handData.Count);
        for (int i = 0; i < handData.Count; i++)
        {
            var prefab = handData[i];
            var pos = new Vector2(transform.position.x + PosValueX + (i * spacing), transform.position.y - 2.8f);
            var go = Instantiate(prefab, pos, Quaternion.identity);
            var cardCom= go.GetComponent<Card>();
            if (cardCom != null)
            {
                hand.Add(cardCom);
                if (this is HumanPlayer humanPlayer)
                {
                    cardCom.SetOwner(humanPlayer);
                   // Debug.Log("This is human");
                }

            }
            else
            {
                prefab.gameObject.SetActive(false);
            }


          
        }

    }
    protected virtual void RemoveCardFromHand(Card card)
    {
        //if (card == null) return;
        if (hand.Contains(card))
        {
            //handData.Remove(card);
            hand.Remove(card);
            card.gameObject.SetActive(false);
        }
    }

    public void ClearHand()
    {
        hand.Clear();
        handData.Clear();
    }
    public int TempScore
    {
        get { return tempScore; }
        set { tempScore = value; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public virtual Card  PlayerToChoseCard(string name, Suit leadsuit, int highestvalue)
    {
        return null;
    }


}
