using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_ChangeScene : MonoBehaviour
{
    private AsyncOperation nextSceneOp;
    ///OLD
    /*public void CambiarEscena(int indiceEscena)
    {
        SceneManager.LoadScene(indiceEscena);
    }*/

    public void CambiarEscena(int indiceEscena)
    {
        if (nextSceneOp == null)
            StartCoroutine(PreloadAndActivate(indiceEscena));
        //else
            //Debug.LogWarning("Ya hay una precarga en curso.");
    }

    public void IniciarSimulacion(float delay)
    {
        StartCoroutine(DelayedSceneChange(delay));
    }

    IEnumerator DelayedSceneChange(float delay)
    {
        yield return new WaitForSeconds(delay);
        CambiarEscena(1);
    }

    private IEnumerator PreloadAndActivate(int indiceEscena)
    {
        if (indiceEscena < 0 || indiceEscena >= SceneManager.sceneCountInBuildSettings)
        {
            //Debug.LogError($"El índice de escena {indiceEscena} no es válido.");
            yield break;
        }

        nextSceneOp = SceneManager.LoadSceneAsync(indiceEscena);
        nextSceneOp.allowSceneActivation = false;

        // Espera hasta que la escena esté casi cargada
        while (nextSceneOp.progress < 0.9f)
        {
            yield return null;
        }

        // Aquí puedes poner una transición o esperar una condición antes de activar
        // Por ejemplo, esperar 0.2 segundos para simular una transición
        yield return new WaitForSeconds(0.2f);

        nextSceneOp.allowSceneActivation = true;
        nextSceneOp = null;
    }
}
