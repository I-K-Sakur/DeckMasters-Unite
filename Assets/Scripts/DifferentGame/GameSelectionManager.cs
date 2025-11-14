using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelectionManager : MonoBehaviour
{
  [SerializeField] private GameObject ComingSoonImage;
  [SerializeField] private GameObject BackButton;
  
  
  public void CallBridgeCard()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public void UnoCardGame()
  {
    ComingSoonImage.gameObject.SetActive(true);
    BackButton.SetActive(false);
  }
  public void PokemonCardGame()
  {
    ComingSoonImage.gameObject.SetActive(true);
    BackButton.SetActive(false);
  }

  public void Back()
  {
    ComingSoonImage.gameObject.SetActive(false);
    BackButton.SetActive(true);
  }

  public void GotoMenuScene()
  {
    SceneManager.LoadScene(0);
  }

}
