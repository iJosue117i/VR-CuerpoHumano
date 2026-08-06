using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class S_SoltarAminoacidos : MonoBehaviour
{
    public bool activado = false;

    XRGrabInteractable grabInteractable;
    public Transform puntoDestino; // Asigna el destino desde el Inspector
    public float velocidadMovimiento = 2f;

    private void Awake() => grabInteractable = GetComponent<XRGrabInteractable>();


    #region Liberacion con VR
    public void SoltarAminoacido(SelectExitEventArgs args)
    {
        if (!activado)
        {
            activado = true;
            DetachParent();
            LiberarAminoacidos(args);
        }
    }

    public void LiberarAminoacidos(SelectExitEventArgs args)
    {
        StartCoroutine(SoltarAminoacido(args.interactorObject as IXRSelectInteractor));
    }

    IEnumerator SoltarAminoacido(IXRSelectInteractor interactor)
    {
        yield return new WaitForSeconds(1.5f);
        // Desactivar la capacidad de ser agarrado  
        grabInteractable.enabled = false;

        //gameObject.GetComponent<Rigidbody>().isKinematic = true;
        if (puntoDestino != null) StartCoroutine(MoverObjetoAPunto(puntoDestino.position));
    }
    #endregion


    #region Liberar con Timeline
    public void LiberarAminoacidosDirector()
    {
        // Fuerza el "drop" si está siendo sujetado
        if (grabInteractable.isSelected && grabInteractable.interactorsSelecting.Count > 0)
        {
            var interactor = grabInteractable.interactorsSelecting[0];
            if (interactor is XRBaseInteractor baseInteractor && baseInteractor.interactionManager != null)
            {
                baseInteractor.interactionManager.SelectExit(interactor, grabInteractable);
            }
        }
        DetachParent();
        StartCoroutine(SoltarAminoacidoDirector());
    }

    IEnumerator SoltarAminoacidoDirector()
    {
        yield return new WaitForSeconds(0.5f);
        if (!activado)
        {
            activado = true;
            // Desactivar la capacidad de ser agarrado  
            grabInteractable.enabled = false;

            //gameObject.GetComponent<Rigidbody>().isKinematic = true;
            if (puntoDestino != null) StartCoroutine(MoverObjetoAPunto(puntoDestino.position));
        }
    }
    #endregion


    IEnumerator MoverObjetoAPunto(Vector3 destino)
    {
        while (Vector3.Distance(transform.position, destino) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destino,
                velocidadMovimiento * Time.deltaTime
            );
            yield return null;
        }
        transform.position = destino; // Asegura la posición exacta al final
        gameObject.SetActive(false); // Desactiva el objeto al llegar al destino
    }

    public void DetachParent()
    {
        transform.parent = null;
    }
}
