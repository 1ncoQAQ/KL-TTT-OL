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
                gameStateUI.text = "�ҷ��ж�";
            } else
            {
                gameStateUI.text = "�Է��ж�";
            }
        } else
        {
            gameStateUI.text = "�������������Ϸ����ս��...";
        }
        timer = 0f;
    }

    [ClientRpc]
    public void RpcPrepareGame()
    {
        StartCoroutine(WaitAndDo(0.02f, () => { gameStateUI.text = "����Ѽ��룬���ڿ�ʼ��Ϸ"; }));
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
            gameStateUI.text = "ƽ��!";
        } else
        {
            gameStateUI.text = result == (playerID + 1) ? "��ʤ����!" : "�Է���ʤ!";
        }
        timerUI.parent.gameObject.SetActive(false);
    }

    [ClientCallback]
    public void SetPlayerID(int id)
    {
        playerID = id;
        gameStateUI.text = "���ڵȴ�����Կ�ʼ��Ϸ...";
    }

    [ClientRpc]
    void RpcEndGameUI()
    {
        gameStarted = false;
        StartCoroutine(WaitAndDo(2.8f, () => 
        { 
            gameStateUI.text = "��Ϸ���������ڷ������˵�"; 
        }));
    }

    [ClientRpc]
    void RpcInterruptGameUI()
    {
        gameStarted = false;
        gameStateUI.text = "�Է����뿪��Ϸ�����ڷ������˵�";
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
