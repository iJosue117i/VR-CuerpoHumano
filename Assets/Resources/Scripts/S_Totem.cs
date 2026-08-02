using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.XR.Interaction.Toolkit;

public class S_Totem : MonoBehaviour
{
    public PlayableDirector director;
    public bool animado = false;
    public bool tocando = false;
    public float timer = 0;

    public AudioSource audioSC;
    public AudioClip clip;

    public GameObject cartel;

    private Coroutine totemCoroutine;
    public void ActivacionTotem(HoverEnterEventArgs args)
    {
        if (!animado)
        {
            tocando = true;
            if (totemCoroutine != null)
                StopCoroutine(totemCoroutine);
            totemCoroutine = StartCoroutine(TotemEvento());
        }
    }

    public void DesactivacionTotem(HoverExitEventArgs args)
    {
        if (!animado)
        {
            tocando = false;
            timer = 0;
            if (totemCoroutine != null)
            {
                StopCoroutine(totemCoroutine);
                totemCoroutine = null;
            }
        }
    }

    IEnumerator TotemEvento()
    {
        timer = 0;
        while (tocando && timer < 2)
        {
            timer += Time.deltaTime;
            if (timer >= 2.0f)
            {
                director.Play();
                audioSC.PlayOneShot(clip);
                cartel.SetActive(false);
                animado = true;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
