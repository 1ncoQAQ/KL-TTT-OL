using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinServerMenu : MonoBehaviour
{
    public Button JoinButton;
    public Button BackToMainButton;
    public GameObject MenuCanvas;
    public NetworkManager networkLobbyManager;

    // Start is called before the first frame update
    void Start()
    {
        JoinButton.onClick.AddListener(JoinServer);
        BackToMainButton.onClick.AddListener(BackToMain);
    }


    void JoinServer()
    {
        networkLobbyManager.networkAddress = "119.23.182.29";
        networkLobbyManager.StartClient();
        MenuCanvas.SetActive(false);
    }

    void BackToMain()
    {
        SceneManager.LoadScene("Menu");
    }
}
