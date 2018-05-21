using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody rb;
    private Collider _col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    public void TurnOnRagdoll()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (_col != null)
        {
            _col.isTrigger = false;
            _col.enabled = true;
        }
    }

    public void TurnOffRagdoll()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (_col != null)
        {
            _col.isTrigger = true;
            _col.enabled = false;
        }
    }
}