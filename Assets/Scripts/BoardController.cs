using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    int[] board = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public Button[] buttons;
    public Slot[] slots;
    bool isPlayerTurn = true;
    int round = 0;
    public Action AINextMove;
    public Action<bool> SwitchTurnUI;
    public Action<int> ShowGameResult;
    public Action EndGame;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => MakeMove(index));
        }
    }

    void SwitchTurn()
    {
        if (isPlayerTurn)
        {
            isPlayerTurn = false;

            // Disable all button
            for (int i = 0; i < 9; i++)
            {
                buttons[i].interactable = false;
            }
            AINextMove();
        }
        else
        {
            isPlayerTurn = true;

            // Enable buttons if not drawn
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                {
                    buttons[i].interactable = true;
                }
            }
        }
        SwitchTurnUI(isPlayerTurn);
    }

    public void MakeMove(int index)
    {
        // Debug.Log(index + "  " + board.Length);
        if (board[index] == 0)
        {
            board[index] = isPlayerTurn ? 1 : 2;
            
            slots[index].DrawSign(board[index]);
            round++;
            CheckWin();
            
        }
    }

    public void MakeRandomMove()
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
        MakeMove(emptySlots[randomIndex]);
    }

    void CheckWin()
    {
        if (board[0] == board[1] && board[1] == board[2] && board[0] != 0)
        {
            HandleWin(0, 1, 2);
        }
        else if (board[3] == board[4] && board[4] == board[5] && board[3] != 0)
        {
            HandleWin(3, 4, 5);
        }
        else if (board[6] == board[7] && board[7] == board[8] && board[6] != 0)
        {
            HandleWin(6, 7, 8);
        }
        else if (board[0] == board[3] && board[3] == board[6] && board[0] != 0)
        {
            HandleWin(0, 3, 6);
        }
        else if (board[1] == board[4] && board[4] == board[7] && board[1] != 0)
        {
            HandleWin(1, 4, 7);
        }
        else if (board[2] == board[5] && board[5] == board[8] && board[2] != 0)
        {
            HandleWin(2, 5, 8);
        }
        else if (board[0] == board[4] && board[4] == board[8] && board[0] != 0)
        {
            HandleWin(0, 4, 8);
        }
        else if (board[2] == board[4] && board[4] == board[6] && board[2] != 0)
        {
            HandleWin(2, 4, 6);
        } else if (round == 9)
        {
            HandleDraw();
        } else
        {
            SwitchTurn();
        }
    }

    void HandleDraw()
    {
        Debug.Log("Draw!");
        ShowGameResult(0);
        EndGame();
    }

    void HandleWin(int slot1, int slot2, int slot3)
    {
        slots[slot1].ShowWinAnim();
        slots[slot2].ShowWinAnim();
        slots[slot3].ShowWinAnim();

        Debug.Log("Player " + board[slot1] + " wins!");

        ShowGameResult(board[slot1]);

        for (int i = 0; i < 9; i++)
        {
            buttons[i].interactable = false;
        }
        EndGame();
    }

    public int[] GetBoard()
    {
        return board;
    }
}
