using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(GameTimer))]
public class BonusSpawner : NetworkBehaviour {

    [Header("Locations")]
    public Transform[] bonusSpawns;
    public GameObject[] bonusses;
    [Header("Info")]
    public int spawnTime = 30;
    bool spawned = true;

    [Header("Other")]
    LobbyManager timer;

    private void Start()
    {
        timer = GetComponent<LobbyManager>();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        if(timer.startTime <= 0)
        {
            if (isServer)
            {
                yield return new WaitForSeconds(spawnTime);
                spawned = false;
                summonBonus();
                StartCoroutine(Spawn());
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(Spawn());
        }


    }

    private void summonBonus()
    {

        if (!spawned)
        {
            Vector3 loc = bonusSpawns[Random.Range(0, bonusSpawns.Length)].position;
            Vector3 finalLos = new Vector3(loc.x + Random.Range(-2, 2), loc.y, loc.z + Random.Range(-2, 2));
            spawned = true;
            GameObject bonus = Instantiate(bonusses[Random.Range(0, bonusses.Length)], finalLos, Quaternion.Euler(0,0,0));
            NetworkServer.Spawn(bonus);
        }

    }




}
