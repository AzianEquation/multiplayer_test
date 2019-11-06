using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Shoot))]
public class PauseMenu : MonoBehaviour {

    private NetworkManager networkManager;
    private PlayerController controller;
    private Shoot shoot;

    [SerializeField]
    bool pauseMenu = false;


    [SerializeField]
    public GameObject UI;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        controller = GetComponent<PlayerController>();
        shoot = GetComponent<Shoot>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }
    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
    void ToggleMenu()
    {
        pauseMenu = !pauseMenu;
        if(pauseMenu)
        {
            UI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
            controller.Paused = true;
            shoot.Paused = true;
        }
        else
        {
            UI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (false);
            controller.Paused = false;
            shoot.Paused = false;
        }
    }

}
