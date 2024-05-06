using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private float speed = 50f;
    private float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; } 

    private void OnEnable()
    {
        StartCoroutine(DestroyBulletAfterTime());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        //miss
        if(!hit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            PoolManager.Instance.bulletPool.ReturnObject(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];
/*        GameObject bulletHole = PoolManager.Instance.bulletHolePool.GetObject();
        bulletHole.transform.position = contactPoint.point + contactPoint.normal * 0.0001f;
       // Debug.Log(bulletHole.transform.position);
        bulletHole.transform.rotation = Quaternion.LookRotation(contactPoint.normal);
        bulletHole.SetActive(true);
        StartCoroutine(DestroyBulletHoleAfterTime(bulletHole));*/
        GameObject bulletHole = Instantiate(bulletHolePrefab, contactPoint.point + contactPoint.normal * 0.01f,
                                Quaternion.LookRotation(contactPoint.normal));
        Destroy(bulletHole, 5f);
        PoolManager.Instance.bulletPool.ReturnObject(gameObject);

    }

    IEnumerator DestroyBulletAfterTime()
    {
        yield return new WaitForSeconds(timeToDestroy);
        PoolManager.Instance.bulletPool.ReturnObject(gameObject);
    }      
/*    IEnumerator DestroyBulletHoleAfterTime(GameObject obj)
    {
        Debug.Log("COROUTINE");
        yield return new WaitForSeconds(timeToDestroy);
        PoolManager.Instance.bulletHolePool.ReturnObject(obj);
    }  */  
}
