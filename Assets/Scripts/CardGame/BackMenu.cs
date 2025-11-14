using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMenu : MonoBehaviour
{
 public void GoBackToDifferentGame()
 {
   SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
 }
}
