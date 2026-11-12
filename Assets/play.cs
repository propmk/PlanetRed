using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Call this method from the Play Button's OnClick event
    public void PlayGame()
    {
        // Make sure "game" matches your scene file name exactly (case-sensitive)
        SceneManager.LoadScene("game");
    }
}
