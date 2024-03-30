using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;

public class NetworkPlayer : NetworkBehaviour
{
    Button[] buttons = new Button[9];

    NetworkBoardController boardController;

    [SyncVar]
    public int playerID;

    [SyncVar]
    string playerName;

    // Start is called before the first frame update
    void Start()
    {
        boardController = GameObject.Find("NetworkBoardController").GetComponent<NetworkBoardController>();
    }

    public void RegisterPlayer(int id)
    {
        playerID = id;
        playerName = "Player " + id;

        Debug.Log("Player " + id + " registered");

        StartCoroutine(WaitAndDo(() => RpcSetupPlayerOnClient(id)));
    }

    [ClientRpc]
    public void RpcSetupPlayerOnClient(int id)
    {
        playerID = id;
        playerName = "Player " + id;
        if (id < 2)
        {
            for (int i = 0; i < 9; i++)
            {
                int index = i;
                buttons[i] = GameObject.Find("Button" + i).GetComponent<Button>();
                buttons[i].onClick.AddListener(() => OnButtonClick(index));
                buttons[i].onClick.AddListener(() => Debug.Log("Trying move on index" + index));
            }
        }
        SendPlayerIDToUI();
    }

    [ClientCallback]
    void SendPlayerIDToUI()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("NetworkUIManager").GetComponent<NetworkUIManager>().SetPlayerID(playerID);
        }
    }

    [ClientCallback]
    void OnButtonClick(int index)
    {
        if (isLocalPlayer)
        {
            CmdMakeMove(index);
        }
    }

    [Command]
    void CmdMakeMove(int index)
    {
        boardController.MakeMove(index, playerID);
    }

    public int GetPlayerID()
    {
        return playerID;
    }

    IEnumerator WaitAndDo(Action move)
    {
        yield return null;
        move();
    }

}
