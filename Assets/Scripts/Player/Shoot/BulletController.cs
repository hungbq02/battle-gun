using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] int type; //ID type weapon
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private float speed = 50f;
    private float timeToDestroy = 0.5f;
    [SerializeField] private int damage;

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
        if (!hit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            PoolManager.Instance.GetBulletPool(type).ReturnObject(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];
        if (collision.gameObject.tag != "Enermy")
        {
            Debug.Log("Bullet collided with: " + collision.gameObject.name);
            Quaternion bulletHoleRotation = Quaternion.LookRotation(contactPoint.normal);
            GameObject bulletHole = Instantiate(bulletHolePrefab, contactPoint.point + contactPoint.normal * 0.01f, bulletHoleRotation) ;
                                    
            Destroy(bulletHole, 5f);
        }
        else
        {
            HealthSystem health = collision.gameObject.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

        }

        PoolManager.Instance.GetBulletPool(type).ReturnObject(gameObject);


    }



    IEnumerator DestroyBulletAfterTime()
    {
        yield return new WaitForSeconds(timeToDestroy);
        PoolManager.Instance.GetBulletPool(type).ReturnObject(gameObject);

    }
}
