using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerPanel : NetworkBehaviour {

    public GameObject player;
    public Text usernameText;
    public Text killsText;
    public Text deathsText;

    public void Start()
    {
        StartCoroutine(Refresh());
    }
    IEnumerator Refresh()
    {
        usernameText.text = player.GetComponent<UserInfo>().username;
        deathsText.text = player.GetComponent<Health>().Deaths.ToString();
        killsText.text = player.GetComponent<Shoot>().kills.ToString();
        if (isServer)
        {
            RpcRefresh();
        }


        yield return new WaitForSeconds(2f);
        StartCoroutine(Refresh());
    }
    [ClientRpc]
    void RpcRefresh()
    {
        usernameText.text = player.GetComponent<UserInfo>().username;
        deathsText.text = player.GetComponent<Health>().Deaths.ToString();
        killsText.text = player.GetComponent<Shoot>().kills.ToString();
    }
}
