using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_ChangeScene : MonoBehaviour
{
    public void CambiarEscena(int indiceEscena)
    {
        SceneManager.LoadScene(indiceEscena);
    }
}
