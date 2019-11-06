using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour {

    public GameObject winnerPanel;
    public GameObject lobbyPanel;
    public Text winnerText;
    public Text scoreText;

    public int scorePerKill = 100;
    public int scorePerDeath = -50;

    [SyncVar]
    GameObject highestPlayer;
    [SyncVar]
    int highestScore = -5000;

    GameObject[] players;

    private void Start()
    {
        winnerPanel.SetActive(false);
    }

    public void Finish()
    {

            players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++)
            {
                int tempScore;
                tempScore = (players[i].GetComponent<Health>().Deaths * scorePerDeath) + (players[i].GetComponent<Shoot>().kills * scorePerKill);

                if (tempScore > highestScore)
                {
                    highestScore = (players[i].GetComponent<Health>().Deaths * scorePerDeath) + (players[i].GetComponent<Shoot>().kills * scorePerKill);
                    highestPlayer = players[i];

                    winnerText.text = players[i].GetComponent<UserInfo>().username;
                    scoreText.text = players[i].GetComponent<Shoot>().kills.ToString() + " Kills & " + players[i].GetComponent<Health>().Deaths.ToString() + " Deaths";

                }


            }
            winnerPanel.SetActive(true);
            lobbyPanel.SetActive(false);

        if (isServer)
        {
            RpcFinish();
        }
        StartCoroutine(StopGame());
    }
    [ClientRpc]
    void RpcFinish()
    {
        winnerPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        winnerText.text = highestPlayer.GetComponent<UserInfo>().username;
        scoreText.text = highestPlayer.GetComponent<Shoot>().kills.ToString() + " Kills & " + highestPlayer.GetComponent<Health>().Deaths.ToString() + " Deaths";
        

        StartCoroutine(StopGame());


    }
    IEnumerator StopGame()
    {
        yield return new WaitForSeconds(10);
        MatchInfo matchInfo = NetworkManager.singleton.matchInfo;
        NetworkManager.singleton.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, NetworkManager.singleton.OnDropConnection);
        NetworkManager.singleton.StopHost();
    }
   

}
