using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    public BoardController boardController;

    public int difficulty = 20;

    void Start()
    {
        boardController.AINextMove = DoNextMove;
    }

    void DoNextMove()
    {
        int[] board = boardController.GetBoard();

        int[] scoreList = new int[9];

        // Set score for each possible move
        // higher the score, ore likely the AI will choose that move
        // 0 if slot is already occupied
        for (int i = 0; i < 9; i++)
        {
            if (board[i] == 0)
            {
                board[i] = 2;
                scoreList[i] = Minimax(board, 0, false);
                board[i] = 0;
            } else
            {
                scoreList[i] = 0;
            }
        }

        int nextMove = WeightedRandomSelect(scoreList, difficulty);

        StartCoroutine(WaitAndMakeMove(nextMove));
    }

    int Minimax(int[] board, int depth, bool isMaximizing)
    {
        int gameStage = CheckGameStage(board);
        if (gameStage != 0)
        {
            return gameStage;
        }
        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                {
                    board[i] = 2;
                    int score = Minimax(board, depth + 1, false);
                    board[i] = 0;
                    bestScore = Mathf.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                {
                    board[i] = 1;
                    int score = Minimax(board, depth + 1, true);
                    board[i] = 0;
                    bestScore = Mathf.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    }

    // Check Game Stage, return 0 if game is not over, 1 if player wins, 3 if AI wins, 2 if draw
    int CheckGameStage(int[] board)
    {
        if (board[0] == board[1] && board[1] == board[2] && board[0] != 0)
        {
            return board[0] == 1 ? 1 : 3;
        }
        if (board[3] == board[4] && board[4] == board[5] && board[3] != 0)
        {
            return board[3] == 1 ? 1 : 3;
        }
        if (board[6] == board[7] && board[7] == board[8] && board[6] != 0)
        {
            return board[6] == 1 ? 1 : 3;
        }
        if (board[0] == board[3] && board[3] == board[6] && board[0] != 0)
        {
            return board[0] == 1 ? 1 : 3;
        }
        if (board[1] == board[4] && board[4] == board[7] && board[1] != 0)
        {
            return board[1] == 1 ? 1 : 3;
        }
        if (board[2] == board[5] && board[5] == board[8] && board[2] != 0)
        {
            return board[2] == 1 ? 1 : 3;
        }
        if (board[0] == board[4] && board[4] == board[8] && board[0] != 0)
        {
            return board[0] == 1 ? 1 : 3;
        }
        if (board[2] == board[4] && board[4] == board[6] && board[2] != 0)
        {
            return board[2] == 1 ? 1 : 3;
        }
        for (int i = 0; i < 9; i++)
        {
            if (board[i] == 0)
            {
                return 0;
            }
        }
        return 2;
    }

    // Weighted Random Select from a list of scores, alpha is the weight factor
    int WeightedRandomSelect(int[] scoreList, int alpha)
    {
        float[] weightList = new float[9];
        float totalWeight = 0;
        for (int i = 0; i < 9; i++)
        {
            weightList[i] = Mathf.Pow(scoreList[i], alpha);
            totalWeight += weightList[i];
        }

        float target = Random.Range(0, totalWeight);

        for (int i = 0; i < 9; i++)
        {
            target -= weightList[i];
            if (target <= 0)
            {
                return i;
            }
        }

        return -1;
    }

    IEnumerator WaitAndMakeMove(int nextMove)
    {         
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        boardController.MakeMove(nextMove);
    }

}
