
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;


public class GameManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
     public List<Player> players=new List<Player>();
     public TextMeshProUGUI textHumanToPlay;
     [SerializeField] private GameObject callRoundUi;
     private Suit _leadSuit;
     private int _highestValue= -1;
     private List<(Player player,Card card, int score)> _currentTrickPlays = new List<(Player, Card,int score)>();
    private Dictionary<string,int> _totalScore = new Dictionary<string, int>();
    private int _tricksPlayedThisRound;
    private int _roundNumber =1;
    private int _totalRounds = 5;
    private int _currentTurn;
    private Player CurrentPlayer => players[_currentTurn];
    private bool _isHuman;
    private AiPlayer _aiPlayer;
    private Dictionary<string,int> _roundScore = new Dictionary<string, int>();
    private Player _roundWinnerPlayer;
    [SerializeField] private TextMeshProUGUI humanTimeRemainingText;
    private bool _waitingForHuman;
    private bool _humanTurn;
    [SerializeField] TextMeshProUGUI winnerOfGameText,winnerOfRoundText;
    private int _plaThisTrick;
    public bool HumanTurn
    {
        get { return _humanTurn; }
        set { _humanTurn = value; }
    }

    public Suit GetSuit { get => _leadSuit;  }
    public bool WaitingForHuman
    {
        get
        {
            return _waitingForHuman;
        }
        set
        {
            _waitingForHuman = value;
        }
    }
    void Start()
    { 
        deckManager.ActivatingAllCards();
        SetupGame();
        _currentTurn = 0;
       // winnerOfRoundText.gameObject.SetActive(false);
        winnerOfGameText.gameObject.SetActive(false);
    }
    
    void SetupGame()
    {
        _roundScore.Clear();
        foreach (var p in players)
        {
            if (string.IsNullOrEmpty(p.PlayerName))
            {
                Debug.LogWarning("!player has no name set!");
            }
            _roundScore[p.PlayerName] = 0; 
            if(!_totalScore.ContainsKey(p.PlayerName))
                _totalScore[p.PlayerName] = 0;
            
        }

        foreach (var v in _totalScore)
        {
            Debug.Log($"- {v.Key} with score {v.Value}");
        }
        deckManager.CreateDeck();
        deckManager.CardDivision(players.Count);
        foreach (var p in players)
        {
            p.InitPlayer(deckManager);

        }
        Debug.Log("Bidding Phase er ag muhurto");
        StartCoroutine(StartBiddingPhase());

    }
    
    private IEnumerator StartBiddingPhase()
    {
        ShowHumanTurnUI(false);
       if(callRoundUi!=null)  callRoundUi.SetActive(true);
        if (players.Count>0 && players[0] is HumanPlayer)
        {
            yield return new WaitUntil(() => players[0].Call> 0);
            Debug.Log($"{players[0].name} called {players[0].Call}");
        }
        for(int i = 1; i < players.Count; i++)
        {
            if ( players[i] is AiPlayer aiPlayer)
            {
              int   temround = aiPlayer.AiPlayerCallRound();
              players[i].Call = temround;
                //trickWinner.Add((currentPlayer.name, 0, temround));
                Debug.Log(aiPlayer.name+temround);
            } 
            if(callRoundUi!=null)  callRoundUi.SetActive(false);
            if (_roundWinnerPlayer != null)
                _currentTurn = Mathf.Clamp(players.IndexOf(_roundWinnerPlayer), 0, Mathf.Max(0, players.Count - 1));
            else _currentTurn = 0;
            _leadSuit = Suit.None;
            _highestValue = -1;
            _currentTrickPlays.Clear();
            _tricksPlayedThisRound = 0;
        }
        StartCoroutine(GameLoop());
        //StartRound();
    }
    
    private IEnumerator GameLoop()
    { 
        while (_roundNumber <= _totalRounds) 
        { 
            Debug.Log($"starting round{_roundNumber}");
        //2nd round theke execute hobe
        if(_roundNumber>=2)
        { 
            deckManager.CreateDeck();
            deckManager.CardDivision(players.Count);
            foreach (var p in players)
            {
                p.InitPlayer(deckManager);
                p.Call = 0;
            }
            if (callRoundUi != null) callRoundUi.SetActive(true);
            if (players[0] is HumanPlayer)
                yield return new WaitUntil(() => players[0].Call > 0);
            for (int i = 1; i < players.Count; i++)
            {
                if (players[i] is AiPlayer aiPlayer)
                {
                    int aicall = aiPlayer.AiPlayerCallRound();
                    players[i].Call = aicall;
                }
            }
            if (callRoundUi != null) callRoundUi.SetActive(false);
            _tricksPlayedThisRound = 0;
            _currentTrickPlays.Clear();
            _leadSuit = Suit.None;
            _highestValue = -1;
            // _winnerOfGameText.gameObject.SetActive(false);
            _roundScore.Clear();
            _currentTurn = 0;
         
        }
        //SetupGame();
        while (_tricksPlayedThisRound < 13)
        {
            _plaThisTrick = 0;
                _currentTrickPlays.Clear();
                _humanTurn = false;
                _leadSuit = Suit.None;
                _highestValue = -1;
              // currentTurn = Mathf.Clamp(currentTurn,0,Mathf.Max(0,players.Count - 1));
              while(_plaThisTrick<players.Count)
              {
                  Player current = players[_currentTurn];
                   // int index = (currentTurn+i+players.Count) % players.Count;
                    if (current == null) yield break;
                    if (current is HumanPlayer)
                    {
                        HumanTurn = true;
                        ShowHumanTurnUI(true);
                        _waitingForHuman = true;
                        yield return new WaitUntil(()=>!WaitingForHuman);
                        ShowHumanTurnUI(false);
                        HumanTurn = false;
                    }
                    else if (current is AiPlayer aiPlayer )
                    {
                       // Debug.Log("this is ai ,kaj korteche ");
                        yield return new WaitForSeconds(0.5f);
                        Card card = aiPlayer.PlayerToChoseCard(aiPlayer.PlayerName, _leadSuit, _highestValue);
                        Debug.Log($"{aiPlayer.PlayerName} called {card}");
                        if (card != null)
                        {
                            OnCardPlayed(aiPlayer, card);
                        }
                    }
                    // if (playThisTrick < players.Count)
                    _currentTurn = (_currentTurn+1) % players.Count;
                    _plaThisTrick++;
                    yield return new WaitForSeconds(0.5f);
                    //yield return null;
                }
                _humanTurn = false;
                EndTrick();
        } 
        EndRound();
        _roundNumber++;
        } 
        EndGame();
        
    }
    //each trick
    public void OnCardPlayed(Player player, Card card)
    {
        if (player == null || card == null) return;
        if (player != CurrentPlayer) return;
        Debug.Log($"{player.name} played {card.type} {card.rank}");
        if (_currentTrickPlays.Count == 0)
        {
            _leadSuit = card.type;
            _highestValue = -1;
            winnerOfRoundText.text = $"Lead Suit: {_leadSuit}";
        }
        int poiint = card.PointFromCard();
        if (card.type == _leadSuit || card.type == Suit.Spade)
        {
            if(poiint >= _highestValue)  _highestValue = poiint;
        }
        _currentTrickPlays.Add((player,card,poiint));
            // player.RemoveCardFromHand(card);
        // currentTurn = (currentTurn + 1) % players.Count;

    }
    private void EndTrick()
    {
        if(_currentTrickPlays ==null || _currentTrickPlays.Count == 0)
            return;
        var spadesPlayed = _currentTrickPlays
            .Where(t => t.card.type == Suit.Spade)
            .OrderByDescending(t => t.card.PointFromCard())
            .ToList();
        var leadsuiPlays = _currentTrickPlays
            .Where(t => t.card.type == _leadSuit)
            .OrderByDescending(t => t.card.PointFromCard())
            .ToList();
        (Player player, Card card, int score) winner;
        if (spadesPlayed.Any())
        {
            winner = spadesPlayed.First();
        }
        else if(leadsuiPlays.Any())
        {
                winner = leadsuiPlays.First();
        }
        else
        {
            winner = _currentTrickPlays.First();
        }
        var winnerPlayer = winner.player;
        if(!_roundScore.ContainsKey(winnerPlayer.PlayerName))
            _roundScore[winnerPlayer.PlayerName] = 0;
        _roundScore[winnerPlayer.PlayerName] += 1;

        _currentTurn = players.IndexOf(winnerPlayer);
        _tricksPlayedThisRound++;
       // _leadSuit = winner;
        _currentTrickPlays.Clear();
        //winnerOfRoundText.text = $"The Lead Suit is {_leadSuit}";
        _highestValue = -1;
        _leadSuit = Suit.None;
        // if(_currentTrickPlays.Count ==0) return;
        // var eligible = _currentTrickPlays.Where(t => t.card.type ==_leadSuit || t.card.type == Suit.Spade)
        //     .OrderByDescending(t => t.score).ToList();
        // var winner = eligible.First();
        // var winnerPlayer = winner.player;
        // //name checker
        // if (!_roundScore.ContainsKey(winnerPlayer.PlayerName))
        //     _roundScore[winnerPlayer.PlayerName] = 0;
        // _roundScore[winnerPlayer.PlayerName] += 1;
        // _currentTurn = players.IndexOf(winnerPlayer);
        // _tricksPlayedThisRound++;
        // _currentTrickPlays.Clear();
        // winnerOfRoundText.text = $"The Lead Suit is {_leadSuit}";
        // _highestValue = -1;
        // _leadSuit = Suit.None;
        // Debug.Log($"The winner of this trick is {winnerPlayer.PlayerName} = {_totalScore[winnerPlayer.PlayerName]}");
    }

    void EndRound()
    {
        if (_roundScore == null || _roundScore.Count == 0) return;
        foreach (var player in players)
        {
            int tricksWon = _roundScore.ContainsKey(player.PlayerName) ? _roundScore[player.PlayerName] : 0;
            int call = player.Call;
           // int roundPoints = tricksWon >= call ? call * 2 : -call;
           int roundPoints;
           if (tricksWon >= call)
           {
               if (call >= 0)
               {
                   roundPoints = 16;
               }
               else
               {
                   roundPoints = call * 2;
                   int extraTricks = tricksWon - call;
                   if (extraTricks > 0)
                   {
                       roundPoints += (int)(extraTricks*0.1);
                   }
               }
           }
           else
           {
               roundPoints = -call;
           }
            if(!_totalScore.ContainsKey(player.PlayerName))
                _totalScore[player.PlayerName] = 0;
            _totalScore[player.PlayerName] += roundPoints;
            Debug.Log($"{player.PlayerName} called {call} , tricks won ={tricksWon}, Round Points = {roundPoints}");
            player.ClearHand();
        }
        var rounWinner = _roundScore.OrderByDescending(t=>t.Value).First();
        _roundWinnerPlayer = players.FirstOrDefault(p => p.PlayerName == rounWinner.Key);
        if (winnerOfRoundText != null)
        {
            winnerOfRoundText.gameObject.SetActive(true);
            winnerOfRoundText.text = $"The round Winner is {rounWinner.Key} {rounWinner.Value} wins";
        }
        //ChoosingTrickWinner();
        _roundScore.Clear();
        _currentTrickPlays.Clear();
        _leadSuit = Suit.None;
        _highestValue = -1;
        _tricksPlayedThisRound = 0;
    }

    void EndGame()
    {
        if(_totalScore==null || _totalScore.Count == 0) return; 
        var winner = _totalScore.OrderByDescending(t=>t.Value).First();
        Debug.Log($"{winner.Key} won");
        if (winner.Key != null)
        {
            winnerOfGameText.gameObject.SetActive(true);
            winnerOfGameText.text = $"The winner of the game is {winner.Key} {winner.Value} wins";
            Debug.Log($"Game Over!Winner is: {winner.Key} with {winner.Value}");
        }
        else
        {
            Debug.Log("Game Over! No winner is determined");
        }
    }
    
    public void ShowHumanTurnUI(bool show)
    {
        if (textHumanToPlay != null)
        {
            textHumanToPlay.gameObject.SetActive(show);
            textHumanToPlay.text = $"Play A Valid Card";
            //humanTurn = true;
            //  Debug.Log($"Text UI Opened{show}");
        }
    }
    public void HumanPlayedCard(Card card)
    {
        if (CurrentPlayer is HumanPlayer human)
            OnCardPlayed(human, card);
    }

}
