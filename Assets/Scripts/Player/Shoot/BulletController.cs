using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private float speed = 50f;
    private float timeToDestroy = 3f;
    [SerializeField] private int damage;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

/*    private void OnEnable()
    {
        StartCoroutine(DestroyBulletAfterTime());
    }*/

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        //miss
        if (!hit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            PoolManager.Instance.bulletPool.ReturnObject(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];
        if (collision.gameObject.tag != "Enermy")
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab, contactPoint.point + contactPoint.normal * 0.01f,
                                    Quaternion.LookRotation(contactPoint.normal));
            Destroy(bulletHole, 5f);
        }
        else
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);

        }

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
