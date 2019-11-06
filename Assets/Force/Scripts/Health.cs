using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(UserInfo))]
public class Health : NetworkBehaviour {


    UserInfo uInfo;
    [SyncVar]
    public float health = 1000;

    bool dead = false;


    [SerializeField]
    Slider hpBar;

    [SyncVar]
    public int Deaths = 0;

    public GameObject lastDamager;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Death")
        {
            health -= 2000;
            Refresh();
        }
    }

    public void Refresh()
    {


        hpBar.value = health;

        if(health > 1000)
        {
            health = 1000;
        }
        if(health < -1)
        {

            if(isLocalPlayer)
            {
                health = -1;
                if(isServer)
                {
                    Die();
                }
                else
                {
                    CmdDie();
                    Die();
                }

                


            }



        }
    }
    private void Update()
    {
        if(dead && Input.GetKeyDown(KeyCode.Space))
        {
            Respawn();
        }
    }

    private void Start()
    {
        uInfo = GetComponent<UserInfo>();
        Refresh();
    }
    private void Die()
    {
        Deaths++;
        dead = true;
        if(isLocalPlayer)
        {
            Respawn();
        }
        if (lastDamager != gameObject)
        {
            lastDamager.GetComponent<Shoot>().kills++;
        }

    }
    [Command]
    private void CmdDie()
    {
        Deaths++;
        dead = true;
        if(lastDamager != gameObject)
        {
            lastDamager.GetComponent<Shoot>().kills++;
        }

    }
    private void Respawn()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        uInfo.MatchBegin();
        health = 1000;
        Refresh();
        dead = false;
        if(!isServer)
        {
            CmdRespawn();
        }
    }
    [Command]
    private void CmdRespawn()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        health = 1000;
        dead = false;

    }
    

}
