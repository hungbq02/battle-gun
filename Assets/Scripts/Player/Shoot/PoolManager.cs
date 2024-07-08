using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public Pooler bulletPoolPistol;
    public Pooler bulletPoolRiffle;
    public Pooler bulletPoolShotgun;


    private void Start()
    {
        bulletPoolPistol = GameObject.FindWithTag("BulletPoolPistol").GetComponent<Pooler>();
        bulletPoolRiffle = GameObject.FindWithTag("BulletPoolRiffle").GetComponent<Pooler>();
        bulletPoolShotgun = GameObject.FindWithTag("BulletPoolShotgun").GetComponent<Pooler>();

    }
    public Pooler GetBulletPool(int weaponType)
    {
        switch(weaponType)
        {
            case 0:
                return bulletPoolPistol;
            case 1:
                return bulletPoolRiffle;
            case 2:
                return bulletPoolShotgun;
            default:
                return bulletPoolPistol;
        }
    }
}
