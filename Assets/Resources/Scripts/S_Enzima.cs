using System.Collections;
using UnityEngine;

public class S_Enzima : MonoBehaviour
{
    public Rigidbody rb;
    public float time = 2;
    float velocidad = 1f;
    public AnimationCurve curvaVelocidad;
    public float finalPosY;

    public GameObject efectoPepsina;
    public Transform particulaHija;
    public Vector3 velocidadRotacion;
    public bool activateRotation = true;
    public float limitMax=25;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(PosicionarEnzima());
        velocidadRotacion=new Vector3(Random.Range(0, limitMax),Random.Range(0, limitMax), Random.Range(0, limitMax));
    }

    void Update()
    {
        if (particulaHija != null && activateRotation)
        {
            particulaHija.Rotate(velocidadRotacion * Time.deltaTime);
        }
    }

    IEnumerator ActivarGravedad()
    {
        if (!rb.useGravity)
        {
            yield return new WaitForSeconds(time);
            rb.useGravity = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Manos"))
        {
            StartCoroutine(ActivarGravedad());
            Vector3 direccion = (transform.position - collision.transform.position).normalized;
            float fuerzaEmpuje = 0.2f; // Ajusta este valor según el efecto deseado

            rb.AddForce(direccion * fuerzaEmpuje, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("Estomago"))
        {
            activateRotation = false;
            efectoPepsina.SetActive(true);
        }
    }

    IEnumerator PosicionarEnzima()
    {
        Vector3 posicionFinal=new Vector3(transform.position.x,finalPosY,transform.position.z);
        while (Mathf.Abs(transform.position.y - finalPosY) > 0.01f)
        {
            float velocityMove = curvaVelocidad.Evaluate(Mathf.Abs(transform.position.y-finalPosY))*velocidad;
            transform.position = Vector3.MoveTowards(transform.position, posicionFinal, velocityMove * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        transform.position = posicionFinal;
    }
}

