using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDisplays : MonoBehaviour
{
    public Canvas primaryCanvas;
    public Canvas secondaryCanvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (primaryCanvas != null && secondaryCanvas != null)
            {
                //get values
                RenderMode primaryRenderMode = primaryCanvas.renderMode;
                int primaryTargetDisplay = primaryCanvas.targetDisplay;
                Camera primaryWorldCamera = primaryCanvas.worldCamera;

                RenderMode secondaryRenderMode = secondaryCanvas.renderMode;
                int secondaryTargetDisplay = secondaryCanvas.targetDisplay;
                Camera secondaryWorldCamera = secondaryCanvas.worldCamera;

                //reassign
                primaryCanvas.renderMode = secondaryRenderMode;
                primaryCanvas.targetDisplay = secondaryTargetDisplay;
                primaryCanvas.worldCamera = secondaryWorldCamera;

                secondaryCanvas.renderMode = primaryRenderMode;
                secondaryCanvas.targetDisplay = primaryTargetDisplay;
                secondaryCanvas.worldCamera = primaryWorldCamera;
            }
            else
            {
                Debug.Log("Unable to switch canvases because they are null.");
            }

        }
    }
}
