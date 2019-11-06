using UnityEngine;
using UnityEngine.Networking;

public class Loadouts : NetworkBehaviour {

    [SerializeField]
    private Shoot _shoot;

    [SerializeField]
    private GameObject loadoutUI;

    [SerializeField]
    private GameObject WeaponList;




    [SerializeField]
    private PlayerController _control;

    
    public void Select(int i)
    {
        _shoot.WeaponID = i;
        _shoot.SetWeapon(i);
        loadoutUI.SetActive(false);
        _control.Paused = false;
        _shoot.Paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);

        WeaponList.transform.GetChild(i).gameObject.SetActive(true);
        _shoot.AmmoRefresh();
        _shoot.Setup();
    }


    private void Start()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        _shoot.Paused = true;
        _control.Paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = (true);
        loadoutUI.SetActive(true);

        for(int i = 0; i < WeaponList.transform.childCount; i++)
        {
            Transform child = WeaponList.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }


}
