using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Shoot))]
[RequireComponent(typeof(Health))]
public class BonusCollector : NetworkBehaviour {


    private Shoot shoot;
    private Health health;

    private Bonus currentBonus;

    private void Start()
    {
        shoot = GetComponent<Shoot>();
        health = GetComponent<Health>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Bonus")
        {
            Debug.Log("Collided with" + collision.gameObject.name);
            currentBonus = collision.gameObject.GetComponent<Bonus>();
            CmdPickup();
            shoot.totalBullet += currentBonus.Ammo;
            health.health += currentBonus.Health;
            shoot.AmmoRefresh();
            health.Refresh();
        }
    }

    [Command]
    void CmdPickup()
    {
        if(!isServer)
        {
            shoot.totalBullet += currentBonus.Ammo;
            health.health += currentBonus.Health;
        }
        Destroy(currentBonus.gameObject);
        shoot.AmmoRefresh();
        health.Refresh();
    }

}
