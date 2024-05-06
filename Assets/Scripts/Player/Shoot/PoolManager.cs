using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager: Singleton<PoolManager>
{
    public Pooler bulletPool;
    public Pooler bulletHolePool;

    private void Start()
    {
        bulletPool = GameObject.FindWithTag("BulletPool").GetComponent<Pooler>();
        bulletHolePool = GameObject.FindWithTag("BulletHolePool").GetComponent<Pooler>();
    }
}
