using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkUIManager : NetworkBehaviour
{
    public TMP_Text gameStateUI;
    public Transform timerUI;

    public float timeLimit = 8f;
    float timer = 0f;

    public bool gameStarted = false;

    public NetworkBoardController boardController;

    public int playerID;

    private void Start()
    {
        boardController.SwitchTurnUI += RpcSetTurn;
    }

    [ClientRpc]
    public void RpcSetTurn(int movingPlayer)
    {
        if (playerID < 2)
        {
            if (movingPlayer == playerID)
            {
                gameStateUI.text = "我方行动";
            } else
            {
                gameStateUI.text = "对方行动";
            }
        } 
        timer = 0f;
    }

    [ClientCallback]
    public void SetPlayerID(int id)
    {
        playerID = id;
        gameStateUI.text = "正在等待玩家以开始游戏...";
    }

    void Update()
    {
        if (gameStarted)
        {
            timer += Time.deltaTime;
            timerUI.GetComponent<RectTransform>().offsetMax = new Vector2((-timer / timeLimit) * 500, 0);
        }
    }
}
