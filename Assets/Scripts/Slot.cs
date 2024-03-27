using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject crossPrefab;

    GameObject sign;

    public void DrawSign(int i)
    {
        if (i == 1)
        {
            sign = Instantiate(circlePrefab, transform);
        }
        else if (i == 2)
        {
            sign = Instantiate(crossPrefab, transform);
        }
    }

    public void ShowWinAnim()
    {
        sign.GetComponent<Animator>().Play("WinAnim");
    }
}
