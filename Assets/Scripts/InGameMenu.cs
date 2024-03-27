using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public GameObject menu;

    public Button resumeButton;

    public Button backToMainButton;

    public Button quitButton;

    public Button restartButton;

    public BoardController boardController;

    public bool gameEnded = false;

    void Start()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ToggleMenu);
        }
        if (backToMainButton != null)
        {
            backToMainButton.onClick.AddListener(LoadMenu);
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        menu.SetActive(false);

        // Subscribe to the EndGame event
        if (boardController != null)
        {
            boardController.EndGame += () =>
            {
                gameEnded = true;
                StartCoroutine(EndGame());
            };
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("OfflineGame");
    }

    public void LoadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }

    // Allow player to toggle the menu by pressing the escape key
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded)
        {
            ToggleMenu();
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3f);
        ToggleMenu();
    }
}
