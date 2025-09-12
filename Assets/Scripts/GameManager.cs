using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        deckManager.CreateDeck();
        deckManager.CardDivision();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
