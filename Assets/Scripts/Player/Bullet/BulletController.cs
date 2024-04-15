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
        Destroy(gameObject, timeToDestroy);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if(!hit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            Destroy(gameObject);
        }    
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.contacts[0];
        GameObject.Instantiate(bulletHolePrefab, contactPoint.point + contactPoint.normal * 0.0001f, Quaternion.LookRotation(contactPoint.normal));
        Destroy(gameObject);
    }
}
