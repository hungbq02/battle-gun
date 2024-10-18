using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Rigidbody rb;
    public float dropForce = 6;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector2.up * dropForce, ForceMode.Impulse);
        Invoke(nameof(KinematicItem),1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
    void KinematicItem()
    {
        rb.isKinematic = true;
    }
}
