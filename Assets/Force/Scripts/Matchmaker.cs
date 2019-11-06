using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class Matchmaker : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 8;

    private string roomName;

    private NetworkManager networkManager;

    public Text serverText;
    public string currentServerID;
    public List<MatchInfoSnapshot> roomList = new List<MatchInfoSnapshot>();

    public string Username;
    public InputField nameInput;
    public Text nameText;


    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();

            Username = PlayerPrefs.GetString("username");
            if(Username == "")
            {
                Username = "Player" + Random.Range(99, 9999);
            }

            nameText.text = Username;
        }
    }


    public void Play()
    {
        FindInternetMatch();
    }

    /// <summary>
    /// Joining
    /// </summary>

    public void FindInternetMatch()
    {
        NetworkManager.singleton.matchMaker.ListMatches(0, 50, "", true, 0, 0, OnInternetMatchList);
    }
    private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success)
        {
            if (matches.Count != 0)
            {
                //join the last server (just in case there are two...)
                if(matches[matches.Count -1].currentSize < matches[matches.Count -1].maxSize)
                {
                    NetworkManager.singleton.matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
                    Debug.Log("Joining server " + matches[matches.Count - 1].name);
                    currentServerID = matches[matches.Count - 1].name.ToString();
                }
                else
                {
                    Debug.Log("Match id" + matches[matches.Count-1].name + " is full. Creating room instead!");
                    CreateRoom();
                }
                
            }
            else
            {
                CreateRoom();
            }
        }
        else
        {
            Debug.Log("Could not join the game, try again");
        }

    }
    private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            //Debug.Log("Able to join a match");

            MatchInfo hostInfo = matchInfo;
            NetworkManager.singleton.StartClient(hostInfo);
            if(serverText == null)
            {
                RefreshStatus();
            }

        }
        else
        {
            Debug.Log("Could not join the game, try again");
        }
    }




    /// <summary>
    /// Hosting
    /// </summary>


    public void CreateRoom()
    {
        roomName = "Game" + Random.Range(9999, 9999999);
        if (roomName != "" && roomName != null)
        {
            Debug.Log("Creating room: " + roomName + " with room for" + roomSize + " players.");

            //Create room
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
            currentServerID = roomName;
            RefreshStatus();
        }
    }

    private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            //Debug.Log("Create match succeeded");

            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 9000);

            NetworkManager.singleton.StartHost(hostInfo);
            RefreshStatus();
        }
        else
        {
            Debug.LogError("Create match failed");
        }
    }


    public void RefreshStatus()
    {

       //if(serverText == null)
       // {
       //     serverText.text = currentServerID + " : " + networkManager.matchInfo.networkId;
       // }

    }

    public void SetName()
    {
        if(nameInput.text.Length >= 4)
        {
            Username = nameInput.text;
            nameText.text = Username;

            PlayerPrefs.SetString("username", Username);
            PlayerPrefs.Save();
        }
        Debug.Log("Name is too short");

    }
}
