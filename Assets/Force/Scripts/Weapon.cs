using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Force/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public int WeaponID = 0;
    public float Power = 3000f;
    public float shotWait = 0.5f;
    public GameObject Bullet;
    public float bulletSpread = 1f;
    public int bulletAmount = 1;
    public int magSize = 5;
    public int defBulletAmount = 50;
    public float reloadTime = 2f;
    public float zoomFOV;
}
