using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    public AIPlayer aiPlayer;
    public Button easy;
    public Button medium;
    public Button hard;

    public GameObject gameUI;

    // Start is called before the first frame update
    void Start()
    {
        easy.onClick.AddListener(() => SetDifficulty(1));
        medium.onClick.AddListener(() => SetDifficulty(4));
        hard.onClick.AddListener(() => SetDifficulty(20));
    }

    void SetDifficulty(int difficulty)
    {
        aiPlayer.difficulty = difficulty;
        transform.gameObject.SetActive(false);
        gameUI.SetActive(true);
    }
}
