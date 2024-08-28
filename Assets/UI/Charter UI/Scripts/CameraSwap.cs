using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwap : MonoBehaviour
{
    public Canvas primaryCanvas;
    public Canvas primaryCanvasSubcanvas;
    public Canvas secondaryCanvas;

    public Camera primaryCamera;
    public Camera secondaryCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (secondaryCamera == null)
            FindSecondaryCamera();
    }

    private void FindSecondaryCamera()
    {
        foreach (Camera camera in Camera.allCameras)
        {
            if (camera != primaryCamera)
            {
                secondaryCamera = camera;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Canvas Swap
        if (Input.GetKeyDown(KeyCode.S))
        {
            SwapCanvasDisplays();
        }
        //Canvas Rotate
        else if (Input.GetKeyDown(KeyCode.A))
        {
            RotatePrimaryCanvasDisplay();
        }
    }

    private void SwapCanvasDisplays()
    {
        int aux = primaryCanvas.targetDisplay;
        primaryCanvas.targetDisplay = secondaryCanvas.targetDisplay;
        secondaryCanvas.targetDisplay = aux;

        int auxCam = primaryCamera.targetDisplay;
        primaryCamera.targetDisplay = secondaryCamera.targetDisplay;
        secondaryCamera.targetDisplay = auxCam;

        // int auxSortOrder = primaryCanvas.sortingOrder;
        // primaryCanvas.sortingOrder = secondaryCanvas.sortingOrder;
        // secondaryCanvas.sortingOrder = auxSortOrder;
    }

    private void RotatePrimaryCanvasDisplay()
    {
        RectTransform rt = primaryCanvasSubcanvas.GetComponent<RectTransform>();
        if (rt.eulerAngles.z == 0)
        {
            primaryCanvasSubcanvas.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else
        {
            primaryCanvasSubcanvas.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        Vector2 refRez = primaryCanvas.GetComponent<CanvasScaler>().referenceResolution;
        primaryCanvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(refRez.y, refRez.x);
    }
}
