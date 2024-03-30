using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class NetworkLobby : NetworkManager
{
    public static new NetworkLobby singleton => (NetworkLobby)NetworkManager.singleton;

    List<NetworkPlayer> playerLists = new List<NetworkPlayer>();

    public NetworkBoardController boardController;

    public Action InterruptGame;

    public override void Start()
    {
        base.Start();
        if (IsServerMode())
        {
            StartServer();
        }
    }

    bool IsServerMode()
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-server")
            {
                return true;
            }
        }
        return false;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        playerLists.Clear();
        boardController.ServerEndGame += StopGame;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        // Register the player
        NetworkPlayer player = conn.identity.GetComponent<NetworkPlayer>();
        playerLists.Add(player);
        // Player ID start with 0
        player.RegisterPlayer(playerLists.IndexOf(player));

        if (playerLists.Count == 2)
        {
            StartGame();
        }

        Debug.Log("Player Joined");
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // Remove the player
        NetworkPlayer player = conn.identity.GetComponent<NetworkPlayer>();

        // Stop the game if player0 or player1 leave
        if (playerLists.IndexOf(player) < 2)
        {
            InterruptGame();
            boardController.EndGame();
        }
        playerLists.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        StartCoroutine(WaitAndDo(3f, () => { SceneManager.LoadScene("Menu"); Destroy(gameObject); }));
    }

    [ServerCallback]
    void StartGame()
    {
        Debug.Log("Game Started");
        boardController.StartGame();
    }

    [ServerCallback]
    void StopGame()
    {
        foreach (NetworkPlayer player in playerLists)
        {
            player.connectionToClient.Disconnect();
        }
        StopAllCoroutines();
        playerLists.Clear();
    }

    IEnumerator WaitAndDo(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
