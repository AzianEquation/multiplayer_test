using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(GameTimer))]
public class LobbyManager : NetworkBehaviour {

    [Header("Main")]
    public float startTime = 30;
    public Text toStartText;
    public int PlayersToStart = 2;

    bool started = false;

    [Header("Connections")]
    public GameObject UI;
    public GameObject lobbyCam;

    [SerializeField]
    Transform lobbyList;

    [SerializeField]
    GameObject lobbyListPrefab;


    GameObject[] players;
    GameTimer gameTimer;

    private void Start()
    {
        gameTimer = GetComponent<GameTimer>();
        StartCoroutine(Refresh());
    }
    
    IEnumerator Refresh()
    {
        if(started)
        {
            toStartText.text = "";
        }

        players = GameObject.FindGameObjectsWithTag("Player");

            foreach (Transform child in lobbyList)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < players.Length; i++)
            {

                GameObject listItem = Instantiate(lobbyListPrefab, lobbyList);
                listItem.GetComponent<PlayerPanel>().player = players[i];
            }
        yield return new WaitForSeconds(1);
        StartCoroutine(Refresh());
        
    }

    private void Update()
    {

        if(started)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                UI.SetActive(true);
            }
            if(Input.GetKeyUp(KeyCode.Tab))
            {
                UI.SetActive(false);
            }
        }

        if (isServer && startTime > 0 && players.Length >= PlayersToStart) 
        {
 

            startTime -= 1 * Time.deltaTime;

            toStartText.text = startTime.ToString("F0") + " Sec";
            if(isServer)
            {
                RpcTimer(startTime);
            }

        }
        if(startTime <= 0)
        {
            StartMatch();
            if(isServer)
            {
                RpcStart();

                for (int i = 0; i < players.Length; i++)
                {
                    if(!started)
                    {

                        players[i].GetComponent<UserInfo>().MatchBegin();
                    }

                }
                started = true;

            }

        }
    }


    [ClientRpc]
    private void RpcTimer(float i)
    {
        toStartText.text = i.ToString("F0") + " Sec";
    }

    private void StartMatch()
    {
        gameTimer.Started = true;



        for (int i = 0; i < players.Length; i++)
        {
            if (!started)
            {
                UI.SetActive(false);
                lobbyCam.SetActive(false);
                players[i].GetComponent<UserInfo>().MatchBegin();
            }
        }
        started = true;

    }


    [ClientRpc]
    private void RpcStart()
    {

        for (int i = 0; i < players.Length; i++)
        {

            if (!started)
            {
                UI.SetActive(false);
                lobbyCam.SetActive(false);
                players[i].GetComponent<UserInfo>().MatchBegin();
            }
        }
        started = true;
        //set all active

    }
    
}
