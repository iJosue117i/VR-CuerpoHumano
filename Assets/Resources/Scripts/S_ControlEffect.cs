using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class S_ControlEffect : MonoBehaviour
{
    public XRController control;    

    public void Vibracion()
    {
        StartCoroutine(vibrar());
    }
    IEnumerator vibrar()
    {
        yield return new WaitForSeconds(0.1f);
        control.SendHapticImpulse(0.9f, 1.5f);

    }
    public void VibracionPersonalizada(float fuerza)
    {
        StartCoroutine("vibrarPersonalizado",fuerza);
    }
    IEnumerator vibrarPersonalizado(float fuerza)
    {
        yield return new WaitForSeconds(0.1f);
        control.SendHapticImpulse(fuerza, 0.5f);

    }
}
