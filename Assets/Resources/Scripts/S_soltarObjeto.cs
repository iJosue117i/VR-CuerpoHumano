using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class S_soltarObjeto : MonoBehaviour
{
    public bool activado = false;
    public float timer;
    public Animator animatorControl;

    XRGrabInteractable grabInteractable;
    public Transform puntoDestino; // Asigna el destino desde el Inspector
    public float velocidadMovimiento = 2f;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        StartCoroutine(SoltarTrasDosSegundosNoAgarrado());
        //grabInteractable.selectEntered.AddListener(OnSelectEntered);
    }
    /*
    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
    }*/

    public void AgarrarObjeto(SelectEnterEventArgs args)
    {
        activado = true;
        StartCoroutine(SoltarTrasDosSegundos(args.interactorObject as IXRSelectInteractor));
    }

    IEnumerator SoltarTrasDosSegundos(IXRSelectInteractor interactor)
    {
        yield return new WaitForSeconds(3f);

        // Soltar el objeto  
        if (grabInteractable.isSelected && grabInteractable.interactorsSelecting.Count > 0)
        {
            if (interactor is XRBaseInteractor baseInteractor && baseInteractor.interactionManager != null)
            {
                baseInteractor.interactionManager.SelectExit(interactor, grabInteractable);
            }
        }

        // Desactivar la capacidad de ser agarrado  
        grabInteractable.enabled = false;

        // Aquí puedes poner más lógica si lo necesitas  
        //Debug.Log("Objeto soltado y ya no se puede volver a agarrar.");
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        if (puntoDestino != null) StartCoroutine(MoverObjetoAPunto(puntoDestino.position));
    }

    IEnumerator SoltarTrasDosSegundosNoAgarrado()
    {
        yield return new WaitForSeconds(timer);
        if (!activado)
        {
            animatorControl.SetTrigger("Aparecer");
            yield return new WaitForSeconds(1.5f);
            activado = true;
            // Desactivar la capacidad de ser agarrado  
            grabInteractable.enabled = false;

            // Aquí puedes poner más lógica si lo necesitas  
            //Debug.Log("Objeto soltado y ya no se puede volver a agarrar.");
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            if (puntoDestino != null) StartCoroutine(MoverObjetoAPunto(puntoDestino.position));
        }
    }

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

}
