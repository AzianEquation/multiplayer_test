using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Shoot : NetworkBehaviour {

    [Header("Main")]

    [SerializeField]
    public Weapon weapon;
    [SerializeField]
    private GameObject WeaponList;

    [SerializeField]
    private Transform cam;


    [Header("UI")]

    [SerializeField]
    private Text ammoText;


    [SerializeField]
    private Weapon[] allWeapons;



    [Header("Extra")]
    [SerializeField]
    private Transform spawnPoint;

    [SyncVar]
    public int kills;

    [SyncVar]
    private int inMagBullet;
    [SyncVar]
    public int totalBullet;
    private Animator anim;
    private bool reloading;

    private PlayerController contrl;

    private bool Zoomed = false;

    private float lastShot = 0f;

    [SyncVar]
    public int WeaponID = 0;

    [HideInInspector]
    public bool Paused = false;
    private float finalSpread = 0;

    private float defaultFOV;
    private float defaultSensivity;
    private void Start()
    {
        contrl = GetComponent<PlayerController>();
        Setup();
        AmmoRefresh();
        defaultFOV = cam.gameObject.GetComponent<Camera>().fieldOfView;
        defaultSensivity = contrl.sensitivity;
    }
    public void Setup()
    {
        inMagBullet = weapon.magSize;
        totalBullet = weapon.defBulletAmount;
        if (!isServer)
        {
            CmdSetup();
        }

    }
    [Command]
    public void CmdSetup()
    {
        inMagBullet = weapon.magSize;
        totalBullet = weapon.defBulletAmount;
    }
    public void SetWeapon(int i)
    {
        WeaponID = i;
        weapon = allWeapons[WeaponID];
        if(!isServer)
        {
            CmdSetWeapon(i);
        }

        if(isServer)
        {
            RpcSetWeapon(i);
        }
    }
    [Command]
    private void CmdSetWeapon(int i)
    {
        weapon = allWeapons[i];
        for (int _i = 0; _i < WeaponList.transform.childCount; _i++)
        {
            Transform child = WeaponList.transform.GetChild(_i);
            child.gameObject.SetActive(false);
        }
        WeaponList.transform.GetChild(i).gameObject.SetActive(true);
    }
    [ClientRpc]
    private void RpcSetWeapon(int i)
    {
        weapon = allWeapons[i];
        for (int _i = 0; _i < WeaponList.transform.childCount; _i++)
        {
            Transform child = WeaponList.transform.GetChild(_i);
            child.gameObject.SetActive(false);
        }
        WeaponList.transform.GetChild(i).gameObject.SetActive(true);
    }


    private void Update()
    {
        if (isLocalPlayer && !Paused)
        {
            lastShot += 1 * Time.deltaTime;
            if (inMagBullet >= 1)
            {

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (lastShot >= weapon.shotWait && !reloading)
                    {

                        if(!isServer)
                        {
                            inMagBullet--;
                        }
                        AmmoRefresh();
                        CmdSpawn();
                        lastShot = 0f;
                    }

                }
            }
            if (Input.GetKeyDown(KeyCode.R) && totalBullet >= 1)
            {

                if (inMagBullet != weapon.magSize)
                {
                    ammoText.text = "Reloading...";
                    anim.SetBool("Reload", true);
                    StartCoroutine(reload());
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Zoom();
            }

        }

    }
    IEnumerator reload()
    {
        reloading = true;
        yield return new WaitForSeconds(weapon.reloadTime);
        reloading = false;
        anim.SetBool("Reload", false);
        CmdfillMag();
        fillMag();



    }
    void fillMag()
    {
        if (totalBullet >= 1)
        {
            if (inMagBullet < weapon.magSize)
            {
                totalBullet--;
                inMagBullet++;
                AmmoRefresh();
                fillMag();
            }
            AmmoRefresh();
        }
        AmmoRefresh();
    }
    [Command]
    void CmdfillMag()
    {
        if (totalBullet >= 1)
        {
            if (inMagBullet < weapon.magSize)
            {
                totalBullet--;
                inMagBullet++;
                AmmoRefresh();
                CmdfillMag();
            }

        }
        AmmoRefresh();

    }

    public void AmmoRefresh()
    {
        ammoText.text = inMagBullet + " / " + totalBullet;
        anim = WeaponList.transform.GetChild(weapon.WeaponID).gameObject.GetComponent<Animator>();
    }


    void Zoom()
    {

        Zoomed = !Zoomed;
        if(Zoomed)
        {
            anim.SetBool("Scope", true);
            if(weapon.zoomFOV != 0 )
            {
                cam.gameObject.GetComponent<Camera>().fieldOfView = weapon.zoomFOV;
                contrl.sensitivity = defaultSensivity / 3;
            }
            
        }
        else
        {
            anim.SetBool("Scope", false);
            if (weapon.zoomFOV != 0)
            {
                cam.gameObject.GetComponent<Camera>().fieldOfView = defaultFOV;
                contrl.sensitivity = defaultSensivity;
            }
        }
    }

    [Command]
    void CmdSpawn()
    {
        if(inMagBullet >= 1)
        {
            inMagBullet--;
            AmmoRefresh();
            Debug.Log("Spawning a bomb...");

            if (Zoomed)
            {
                finalSpread = (weapon.bulletSpread / 3);
            }
            else
            {
                finalSpread = weapon.bulletSpread;
            }

            for (int i = 0; i < weapon.bulletAmount; i++)
            {
                GameObject bomb = Instantiate(weapon.Bullet, spawnPoint.position, Quaternion.Euler(cam.rotation.eulerAngles.x + Random.Range(-finalSpread, finalSpread), cam.rotation.eulerAngles.y + Random.Range(-finalSpread, finalSpread), cam.rotation.eulerAngles.z + Random.Range(-finalSpread, finalSpread)));
                bomb.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * weapon.Power);
                //Destroy(bomb, 10f);
                bomb.GetComponent<Bomb>().shooter = gameObject;
                NetworkServer.Spawn(bomb);
            }
        }




    }
}
