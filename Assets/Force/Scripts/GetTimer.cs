using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Shoot))]
public class GetTimer : MonoBehaviour {

    private GameTimer gTimer;
    private GameObject gameManager;
    private Health health;
    private Shoot shoot;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private Text killsText;

    [SerializeField]
    private Text deathsText;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager");
        gTimer = gameManager.GetComponent<GameTimer>();
        health = GetComponent<Health>();
        shoot = GetComponent<Shoot>();
        refresh();
        StartCoroutine(timer());
    }

    private void Update()
    {
        float i = gTimer.GameTime;
        string minSec = string.Format("{0}:{1:00}", (int)i / 60, (int)i % 60);
        timerText.text = minSec;

    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(1);
        refresh();
        StartCoroutine(timer());
    }
    private void refresh()
    {
        deathsText.text = health.Deaths.ToString();
        killsText.text = shoot.kills.ToString();
    }
}
