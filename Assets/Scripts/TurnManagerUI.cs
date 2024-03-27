using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManagerUI : MonoBehaviour
{
    public TMP_Text turn;

    public Transform timerBar;

    public float timeLimit = 10f;

    float timer = 0f;

    bool gameEnded = false;

    public BoardController boardController;

    void Start()
    {
        boardController.SwitchTurnUI = SetTurn;
        boardController.ShowGameResult = ShowGameRsult;
    }

    void SetTurn(bool isPlayerTurn)
    {
        turn.text = isPlayerTurn ? "�ҷ��ж�" : "�Է��ж�";
        timer = 0f;
    }

    void ShowGameRsult(int result)
    {
        gameEnded = true;
        switch (result)
        {
            case 1:
                turn.text = "�ҷ�ʤ��!";
                break;
            case 2:
                turn.text = "�Է�ʤ��!";
                break;
            case 0:
                turn.text = "ƽ��!";
                break;
        }

        timerBar.parent.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        timer += Time.deltaTime;
        timerBar.GetComponent<RectTransform>().offsetMax = new Vector2((-timer / timeLimit) * 500, 0);
        if (timer >= timeLimit)
        {
            boardController.MakeRandomMove();
        }
    }
}
