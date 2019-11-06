using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour {

    public int Ammo = 0;
    public int Health = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Death")
        {
            Destroy(gameObject);
        }
    }
}
