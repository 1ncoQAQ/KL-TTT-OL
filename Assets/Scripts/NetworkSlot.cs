using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSlot : NetworkBehaviour
{
    public GameObject circlePrefab;
    public GameObject crossPrefab;

    GameObject sign;

    [ClientRpc]
    public void RpcDrawSign(int i)
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

    [ClientRpc]
    public void RpcShowWinAnim()
    {
        sign.GetComponent<Animator>().Play("WinAnim");
    }
}
