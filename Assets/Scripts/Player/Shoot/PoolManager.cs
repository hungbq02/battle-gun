using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public Pooler bulletPool;

    private void Start()
    {
        bulletPool = GameObject.FindWithTag("BulletPool").GetComponent<Pooler>();
    }
}
