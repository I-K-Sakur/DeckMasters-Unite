

using DG.Tweening;

using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public enum Suit
{
    Spade,
    Diamond,
    Heart,
    Club,
    None,
}

public enum Rank
{
    Two=2, Three=3, Four=4, Five=5, Six=-6, Seven=7, Eight=8, Nine=9, Ten=10, Jack=11, Queen=12, King=13, Ace=14
}
public class Card : MonoBehaviour
{
 
  public Suit type;
  public Rank rank;
  public int value;
 private Player _owner;
[SerializeField] private GameObject player;
private GameManager _gameManager;
private SpriteRenderer _sr;
  public HumanPlayer Owner { get; set; }
private HumanPlayer _humanPlayer;
private Vector3 _rotationVector = new Vector3(0f, 20f, 0);
  private void Awake()
  {
      value = (int)rank;
  }
  private void Start()
  {
      player = GameObject.FindGameObjectWithTag("Player");
      if (player != null)
          _humanPlayer = player.GetComponent<HumanPlayer>();
      _gameManager = FindFirstObjectByType<GameManager>();
      if(_gameManager ==null)
          Debug.LogError("GameManager nai");
  }

 
  private void Update()
  {
      CheckClicked();
  }
  

  public void PlayerToPlay()
  {
     //CheckClicked();
  }
 
  public void CheckClicked()
  {
      //if (Owner == null) return;
      if (Input.GetMouseButtonDown(0))
      {
          if (!_gameManager.HumanTurn) return;
          Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          Collider2D hit = Physics2D.OverlapPoint(mousePos);
  
          if (hit != null && hit.gameObject == gameObject)
          {
              if (_humanPlayer != null )
              {
                  GetComponent<Collider2D>().enabled = false;
                  Vector3 targetPos = Camera.main.ScreenToWorldPoint(
                      new Vector3(Screen.width/2,Screen.height / 2, 10f));
                  targetPos.z = 0f;
                  Sequence sequence = DOTween.Sequence();
                  sequence.Append(transform.DOMove(targetPos,0.8f).SetEase(Ease.OutBounce));
                  sequence.Join(transform.DORotate(_rotationVector, 0.8f, RotateMode.FastBeyond360));
                  sequence.Join(transform.DOScale(Vector3.zero, 0.8f));
                
                  //sequence.onComplete(()=>Destroy(gameObject));
                  //transform.DORotate(rotationVector, 1f, RotateMode.WorldAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
                  sequence.OnComplete(() =>
                      {
                          if (_humanPlayer != null && _gameManager.HumanTurn)
                          {
                              _humanPlayer.OnCardChosen(this);
                              _gameManager.HumanTurn = false;
                          }
                          else
                          {
                              GetComponent<Collider2D>().enabled = false;
                          }
                      }
                  );
                 
                  //gameManager.FirstTime = true;
                  Debug.Log("You clicked the card!");
              }
              else
              {
                  _gameManager.ShowHumanTurnUI(true);
              }
              // player.GetComponent<HumanPlayer>().OnCardChosen(this);
             
              OnClicked();
          }
      }
  
      
  }
  private void OnClicked()
  {
      Debug.Log("Clicked "+name);
     //int point =  PointFromCard();
  
    }

  public void SetOwner(HumanPlayer owner)
  {
      Owner = owner;
  }
  public int PointFromCard()
  {
      int suitBonus = 0;
      switch (type)
      {
          case Suit.Club: suitBonus = 1; break;
          case Suit.Diamond: suitBonus = 2; break;
          case Suit.Heart: suitBonus = 3; break;
          case Suit.Spade: suitBonus = 4; break;
      }

      return value+suitBonus;
  }
}
