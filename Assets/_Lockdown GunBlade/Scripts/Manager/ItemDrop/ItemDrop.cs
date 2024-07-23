using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Rigidbody rb;
    public float dropForce;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector2.up * dropForce, ForceMode.Impulse);
    }
}
