using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button startOnlineButton;

    public Button startOfflineButton;

    public Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        if (startOnlineButton != null)
        {
            startOnlineButton.onClick.AddListener(StartOnlineGame);
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        if (startOfflineButton != null)
        {
            startOfflineButton.onClick.AddListener(StartGame);
        }
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("OfflineGame");
    }

    public void StartOnlineGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("OnlineGame");
    }

    public void LoadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
