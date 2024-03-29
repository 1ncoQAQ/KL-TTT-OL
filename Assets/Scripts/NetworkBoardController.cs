using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class NetworkBoardController : NetworkBehaviour
{
    int[] board = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public Button[] buttons;

    public NetworkSlot[] slots;

    int currentPlayer = 1;
    int round = 0;

    public Action<int> SwitchTurnUI;
    public Action<int> ShowGameResult;
    public Action EndGame;

    bool isGameStarted = false;

    float timer = 0;

    [ServerCallback]
    public void MakeMove(int index, int playerID)
    {
        // Debug.Log(index + "  " + board.Length);
        if (board[index] == 0 && playerID == currentPlayer && isGameStarted)
        {
            board[index] = currentPlayer == 0 ? 1 : 2;

            slots[index].RpcDrawSign(board[index]);
            round++;
            CheckWin();
            Debug.Log("Player " + playerID + " successfully move on index " + index);
        } else
        {
            Debug.Log("Player " + playerID + " tried to move on index " + index + " but rejected by server");
        }
    }

    [ServerCallback]
    void Update()
    {
        if (isGameStarted)
        {
            timer += Time.deltaTime;
            if (timer >= 8)
            {
                MakeRandomMove();
            }
        }
    }

    [ServerCallback]
    void CheckWin()
    {
        if (board[0] == board[1] && board[1] == board[2] && board[0] != 0)
        {
            RpcHandleWin(0, 1, 2);
        }
        else if (board[3] == board[4] && board[4] == board[5] && board[3] != 0)
        {
            RpcHandleWin(3, 4, 5);
        }
        else if (board[6] == board[7] && board[7] == board[8] && board[6] != 0)
        {
            RpcHandleWin(6, 7, 8);
        }
        else if (board[0] == board[3] && board[3] == board[6] && board[0] != 0)
        {
            RpcHandleWin(0, 3, 6);
        }
        else if (board[1] == board[4] && board[4] == board[7] && board[1] != 0)
        {
            RpcHandleWin(1, 4, 7);
        }
        else if (board[2] == board[5] && board[5] == board[8] && board[2] != 0)
        {
            RpcHandleWin(2, 5, 8);
        }
        else if (board[0] == board[4] && board[4] == board[8] && board[0] != 0)
        {
            RpcHandleWin(0, 4, 8);
        }
        else if (board[2] == board[4] && board[4] == board[6] && board[2] != 0)
        {
            RpcHandleWin(2, 4, 6);
        }
        else if (round == 9)
        {
            RpcHandleDraw();
        }
        else
        {
            SwitchTurn();
        }
    }

    [ServerCallback]
    void SwitchTurn()
    {
        if (currentPlayer == 0)
        {
            currentPlayer = 1;
        }
        else
        {
            currentPlayer = 0;
        }
        timer = 0f;
        SwitchTurnUI(currentPlayer);
    }

    [ServerCallback]
    void RpcHandleWin(int slot1, int slot2, int slot3)
    {
        slots[slot1].RpcShowWinAnim();
        slots[slot2].RpcShowWinAnim();
        slots[slot3].RpcShowWinAnim();

        Debug.Log("Player " + board[slot1] + " wins!");

        // ShowGameResult(board[slot1]);

        //EndGame();
    }

    [ServerCallback]
    void RpcHandleDraw()
    {
        Debug.Log("Draw!");
        // ShowGameResult(0);
        //EndGame();
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }

    [ServerCallback]
    public void StartGame()
    {
        isGameStarted = true;
        currentPlayer = 0;
        round = 0;
        // SwitchTurnUI(currentPlayer);
    }

    [ServerCallback]
    public void ResetBoard()
    {
        // Reset board
        for (int i = 0; i < 9; i++)
        {
            board[i] = 0;
        }
    }

    [ServerCallback]
    void MakeRandomMove()
    {
        var emptySlots = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (board[i] == 0)
            {
                emptySlots.Add(i);
            }
        }
        int randomIndex = UnityEngine.Random.Range(0, emptySlots.Count);
        MakeMove(randomIndex, currentPlayer);
    }
}
