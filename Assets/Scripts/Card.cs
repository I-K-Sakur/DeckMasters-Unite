
using UnityEngine;

public enum Suit
{
    Spade,
    Diamond,
    Heart,
    Club,
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

  public Card(Suit suit, Rank rank)
  {
      this.type = suit;
      this.rank = rank;
      this.value = (int)rank;
  }

}
