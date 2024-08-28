using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkIfPaused : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float blinkInterval = 1f;
    private float lastPauseBlinkTime;

    void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        lastPauseBlinkTime = Time.time;
    }

    private void Update()
    {
        
        if (canvasGroup != null &&
            Client.instance != null &&
            Client.instance.missionState == MissionState.Paused)
        {
            if (blinkInterval > 0 && Time.time >= lastPauseBlinkTime + blinkInterval)
            {
                if (canvasGroup.alpha > 0)
                {
                    canvasGroup.alpha = 0;
                }
                else
                {
                    canvasGroup.alpha = 1;
                }

                lastPauseBlinkTime = Time.time;
            }
        }
        else
        {
            canvasGroup.alpha = 1;
        }
    }

}
