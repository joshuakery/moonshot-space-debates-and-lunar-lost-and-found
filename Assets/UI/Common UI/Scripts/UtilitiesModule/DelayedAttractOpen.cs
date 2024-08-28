using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAttractOpen : MonoBehaviour
{
    public AttractScreen attractScreen;
    public float delay;

    private void Awake()
    {
        if (attractScreen == null)
        {
            attractScreen = gameObject.GetComponent<AttractScreen>();
        }
    }

    void Start()
    {
        StartCoroutine(DelayedOpenAttract(delay));
    }

    private IEnumerator DelayedOpenAttract(float wait)
    {
        yield return null;
        yield return null;
        yield return null;
        attractScreen.OpenAttractScreen();
    }
}
