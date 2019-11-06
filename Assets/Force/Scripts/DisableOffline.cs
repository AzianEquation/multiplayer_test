using UnityEngine.Networking;
using UnityEngine;

public class DisableOffline : NetworkBehaviour {

    public Behaviour[] toDisable;
    public GameObject[] objDisable;
    public GameObject[] graphics;

    private void Start()
    {
        Disable();
    }
    public void Disable()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < toDisable.Length; i++)
            {
                toDisable[i].enabled = false;
            }
            for (int i = 0; i < objDisable.Length; i++)
            {
                objDisable[i].SetActive(false);
            }
            gameObject.layer = 10;
            for (int i = 0; i < graphics.Length; i++)
            {
                graphics[i].layer = 10;
            }
        }
        else
        {
            if(isLocalPlayer)
            {
                gameObject.layer = 9;
                for(int i = 0; i < graphics.Length; i++)
                {
                    graphics[i].layer = 13;
                }

            }


        }
    }
}
