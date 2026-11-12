using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    void Start()
    {
        // Mission Fail scene-e asar sathe sathe Mouse Cursor-ke visible ebong unlock korbe
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // Main game scene-er exact name "game"
        SceneManager.LoadScene("game");
    }
}