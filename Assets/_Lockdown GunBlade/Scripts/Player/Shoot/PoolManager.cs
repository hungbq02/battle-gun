using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public Pooler bulletHolePool;
    public Pooler bulletTrailPool; 

    private void Start()
    {
        bulletHolePool = GameObject.FindWithTag("BulletHolePool").GetComponent<Pooler>();
        bulletTrailPool = GameObject.FindWithTag("BulletTrailPool").GetComponent<Pooler>();  
    }
}
