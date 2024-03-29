using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkLobby : NetworkManager
{
    public static new NetworkLobby singleton => (NetworkLobby)NetworkManager.singleton;

    List<NetworkPlayer> playerLists = new List<NetworkPlayer>();

    public NetworkBoardController boardController;

    public override void OnStartServer()
    {
        base.OnStartServer();
        playerLists.Clear();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        // Register the player
        NetworkPlayer player = conn.identity.GetComponent<NetworkPlayer>();
        playerLists.Add(player);
        // Player ID start with 0
        player.RegisterPlayer(playerLists.IndexOf(player));

        if (playerLists.Count > 1)
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
            Debug.Log("Game Stoped");
            // StopGame();
        }
        playerLists.Remove(player);

        base.OnServerDisconnect(conn);
    }

    void StartGame()
    {
        Debug.Log("Game Started");
        boardController.StartGame();
    }

    void StopGame()
    {
        foreach (NetworkPlayer player in playerLists)
        {
            player.connectionToClient.Disconnect();
        }

        playerLists.Clear();

        // Reset board on server
        boardController.ResetBoard();
    }
}
