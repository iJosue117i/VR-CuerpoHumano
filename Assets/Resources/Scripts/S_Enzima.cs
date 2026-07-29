using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Enzima : MonoBehaviour
{
    public Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator ActivarGravedad()
    {
        if (!rb.useGravity)
        {
            yield return new WaitForSeconds(3f);
            rb.useGravity = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Manos"))
        {
            StartCoroutine(ActivarGravedad());
        }
    }
}

