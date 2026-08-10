using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_vasoFinal : MonoBehaviour
{
    public Transform parent;
    Vector3 posInitial;
    Vector3 rotInitial;

    public float timer;
    private bool escenaCargada = false;

    // Start is called before the first frame update
    void Start()
    {
        posInitial = parent.position;
        rotInitial = parent.eulerAngles;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            parent.position = posInitial;
            parent.eulerAngles = rotInitial;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Boca") && !escenaCargada) 
        {
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                escenaCargada = true;
                SceneManager.LoadScene(0);
            }

        }
    }
}
