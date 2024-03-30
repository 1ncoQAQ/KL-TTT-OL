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

    public NetworkLobby networkLobby;

    public int playerID;

    private void Start()
    {
        boardController.SwitchTurnUI += RpcSetTurn;
        boardController.StartGameUI += RpcPrepareGame;
        boardController.ShowGameResultUI += RpcShowGameResult;
        boardController.ShowEndGameUI += RpcEndGameUI;
        networkLobby.InterruptGame += RpcInterruptGameUI;
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
        } else
        {
            gameStateUI.text = "已有玩家正在游戏，观战中...";
        }
        timer = 0f;
    }

    [ClientRpc]
    public void RpcPrepareGame()
    {
        StartCoroutine(WaitAndDo(0.02f, () => { gameStateUI.text = "玩家已加入，正在开始游戏"; }));
        StartCoroutine(WaitAndDo(3f, StartGame));
    }

    [ClientCallback]
    void StartGame()
    {
        gameStarted = true;
        timerUI.parent.gameObject.SetActive(true);
    }

    [ClientRpc]
    void RpcShowGameResult(int result)
    {
        gameStarted = false;
        if (result == 0)
        {
            gameStateUI.text = "平局!";
        } else
        {
            gameStateUI.text = result == (playerID + 1) ? "你胜利了!" : "对方获胜!";
        }
        timerUI.parent.gameObject.SetActive(false);
    }

    [ClientCallback]
    public void SetPlayerID(int id)
    {
        playerID = id;
        gameStateUI.text = "正在等待玩家以开始游戏...";
    }

    [ClientRpc]
    void RpcEndGameUI()
    {
        gameStarted = false;
        StartCoroutine(WaitAndDo(2.8f, () => 
        { 
            gameStateUI.text = "游戏结束，正在返回主菜单"; 
        }));
    }

    [ClientRpc]
    void RpcInterruptGameUI()
    {
        gameStarted = false;
        gameStateUI.text = "对方已离开游戏，正在返回主菜单";
        timerUI.parent.gameObject.SetActive(false);
    }

    [ClientCallback]
    void Update()
    {
        if (gameStarted)
        {
            timer += Time.deltaTime;
            timerUI.GetComponent<RectTransform>().offsetMax = new Vector2((-timer / timeLimit) * 500, 0);
        }
    }

    IEnumerator WaitAndDo(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
