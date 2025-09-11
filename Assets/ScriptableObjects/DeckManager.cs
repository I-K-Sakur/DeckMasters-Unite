
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckManager", menuName = "Scriptable Objects/DeckManager")]
public class DeckManager : ScriptableObject
{
    public List<Card> deck = new List<Card>();
    private int randomNum;

    public void CreateDeck()
    {
        deck.Clear();
        for(int i = 0; i < deck.Count; i++)
        {
            randomNum = Random.Range(i, deck.Count);
            Card newCard = deck[randomNum];
            deck[i] = deck[randomNum];
            deck[randomNum] = newCard;
        }
    }
}
