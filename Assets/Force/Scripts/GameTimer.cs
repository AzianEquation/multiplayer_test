using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(ScoreManager))]
public class GameTimer : NetworkBehaviour {

    [Tooltip("Game time in seconds")]
    [SyncVar]
    public float GameTime = 300;

    public Text timerText;

    public bool Started = false;

    ScoreManager scoreMan;

    private void Start()
    {
        scoreMan = GetComponent<ScoreManager>();

    }

    private void Update()
    {
        if (isServer)
        {
            if(Started)
            {
                GameTime -= 1 * Time.deltaTime;
            }
            if(GameTime <= 0)
            {
                scoreMan.Finish();
            }

            string minSec = string.Format("{0}:{1:00}", (int)GameTime / 60, (int)GameTime % 60);
            timerText.text = minSec;
            RpcSettime(GameTime);
        }
    }
    [ClientRpc]
    private void RpcSettime(float i)
    {
        string minSec = string.Format("{0}:{1:00}", (int)i / 60, (int)i % 60);
        timerText.text = minSec;
    }
}
