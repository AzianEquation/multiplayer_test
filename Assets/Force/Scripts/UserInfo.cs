using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserInfo : NetworkBehaviour {

    [SyncVar]
    public string username;



    public Behaviour[] disableInLobby;
    public GameObject[] objDisableInLobby;

    [SerializeField]
    GameObject[] spawnPoints;


    private void Start()
    {

        if(isLocalPlayer)
        {
            username = PlayerPrefs.GetString("username");
            gameObject.name = username;
            CmdSetup(PlayerPrefs.GetString("username"));
            //gameObject.SetActive(false);

            spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
            if(!isServer)
            {
                CmdSP();
            }
            else
            {
                RpcSP();
            }
            
        }

        Disable();
    }
    [Command]
    private void CmdSP()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
    }
    [ClientRpc]
    private void RpcSP()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
    }
    public void Disable()
    {
        for (int i = 0; i > disableInLobby.Length; i++)
        {
            disableInLobby[i].enabled = false;
        }
        for (int i = 0; i < objDisableInLobby.Length; i++)
        {
            objDisableInLobby[i].SetActive(false);

        }
    }
    public void MatchBegin()
    {
        for (int i = 0; i < objDisableInLobby.Length; i++)
        {
            objDisableInLobby[i].SetActive(true);
        }
        if (isLocalPlayer)
        {

            for (int i = 0; i < disableInLobby.Length; i++)
            {
                disableInLobby[i].enabled = true;

            }
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        }
        GetComponent<DisableOffline>().Disable();

    }

    [Command]
    void CmdSetup(string _name)
    {
        username = _name;
        gameObject.name = _name;
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
    }

}
