using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class S_directorCondition : MonoBehaviour
{
    public bool activate = false;
    public PlayableDirector director;
    // Start is called before the first frame update
    public void StartTimeline()
    {
        if (!activate)
        {
            activate = true;
            director.Play();
        }
    }
}
