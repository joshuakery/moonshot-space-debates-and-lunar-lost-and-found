using System.Collections;
using UnityEngine;

public class RefreshTextHelper : MonoBehaviour
{
    private FlexibleUIText[] flexUITexts;

    private void Awake()
    {
        //SetFlexUITexts();
        //RefreshFlexUITexts();
    }

    public void OpenAllWindows()
    {
        GenericWindow1[] windows = GetComponentsInChildren<GenericWindow1>();
        foreach (GenericWindow1 window in windows)
        {
            window.Open();
        }
    }

    public void RefreshWindows()
    {
        StartCoroutine(RefreshWindowsCoroutine());
    }

    public IEnumerator RefreshWindowsCoroutine()
    {
        CanvasGroup[] canvasGroups = GetComponentsInChildren<CanvasGroup>();
        bool[] cgActiveStates = new bool[canvasGroups.Length];
        Canvas[] canvases = GetComponentsInChildren<Canvas>();
        bool[] canvasActiveStates = new bool[canvases.Length];

        for (int i=0; i<canvasGroups.Length; i++)
        {
            CanvasGroup canvasGroup = canvasGroups[i];
            canvasGroup.alpha = 1;

            cgActiveStates[i] = canvasGroup.gameObject.activeInHierarchy;
            canvasGroup.gameObject.SetActive(false);
        }
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            Canvas canvas = canvases[i];
            canvas.enabled = true;

            canvasActiveStates[i] = canvas.gameObject.activeInHierarchy;
            canvas.gameObject.SetActive(false);
        }

        yield return null;

        for (int i = 0; i < canvasGroups.Length; i++)
        {
            CanvasGroup canvasGroup = canvasGroups[i];
            canvasGroup.alpha = 0;

            canvasGroup.gameObject.SetActive(cgActiveStates[i]);
        }
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            Canvas canvas = canvases[i];
            canvas.enabled = false;

            canvas.gameObject.SetActive(canvasActiveStates[i]);
        }
    }

    public void SetFlexUITexts()
    {
        flexUITexts = GetComponentsInChildren<FlexibleUIText>();
    }

    public void RefreshFlexUITexts()
    {
        StartCoroutine(RefreshFlexUITextsCoroutine());
    }

    private IEnumerator RefreshFlexUITextsCoroutine()
    {
        for (int i = 0; i < flexUITexts.Length; i++)
        {
            if (flexUITexts[i] != null)
            {
                flexUITexts[i].gameObject.SetActive(false);
            }
        }

        yield return null;

        for (int i = 0; i < flexUITexts.Length; i++)
        {
            if (flexUITexts[i] != null)
            {
                flexUITexts[i].gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetFlexUITexts();
            Debug.Log(flexUITexts.Length);
            RefreshFlexUITexts();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            OpenAllWindows();
        }
    }
}
