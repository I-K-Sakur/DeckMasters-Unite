
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "DeckManager", menuName = "Scriptable Objects/DeckManager")]
public class DeckManager : ScriptableObject
{
    public List<Card> deck=new List<Card>() ;
    private int _randomNum;
    private bool _divisionStage;
    public List<List<Card>> Players = new List<List<Card>>();
    private List<Card>_shuffled = new List<Card>();

    public void ActivatingAllCards()
    {
        foreach (Card card in deck)
        {
            if (!card.gameObject.activeSelf)
            {
                card.gameObject.SetActive(true);
            }
        }
    }

    public void ResetDeck()
    {
        _shuffled.Clear();
        _shuffled = new List<Card>(deck);
        foreach (var card in _shuffled)
        {
            card.gameObject.SetActive(false);
        }
        CreateDeck();
        CardDivision();
    }
    public void CreateDeck()
    {
        _shuffled = new List<Card>(deck);
       // deck.Clear();
       for (int i = 0; i < _shuffled.Count; i++)
       {
           int randomIndex = Random.Range(i, _shuffled.Count);
           // Swap i and randomIndex
           Card temp = _shuffled[i];
           _shuffled[i] = _shuffled[randomIndex];
           _shuffled[randomIndex] = temp;
       }
       //Debug.Log(Shuffled.Count+"the deck is created");
    }
    public void CardDivision(int playerCount=4)
    {
        Players.Clear(); 
        for (int k = 0; k < 4; k++)
        {
            Players.Add(new List<Card>());
        }
    
        for (int j = 0; j < _shuffled.Count; j++)
        {
            //round Robin
            int roundRobin = j % playerCount;
            Card prefab = _shuffled[j];
            Card cardInstance = Instantiate(prefab);
            cardInstance.gameObject.SetActive(false);
            Players[roundRobin].Add(_shuffled[j]);
            // Debug.Log(players[j]);
        }
        for (int i = 0; i < Players.Count; i++)
        {
            Debug.Log($"Player {i + 1} has {Players[i].Count} cards.");
        }

        //Debug.Log(Players.Count+"the deck is created");
    }

}
