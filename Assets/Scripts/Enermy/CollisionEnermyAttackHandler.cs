using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnermyAttackHandler : MonoBehaviour
{
    BoxCollider boxCollider;
    private void Start()
    {
        boxCollider = GetComponentInChildren<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HealthSystem>().TakeDamage(50);
            
        }
    }

    public void EnableAttack()
    {
        boxCollider.enabled = true;
    }
    public void DisableAttack()
    {
        boxCollider.enabled = false;
    }

}
