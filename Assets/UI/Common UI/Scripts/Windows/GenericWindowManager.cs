using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GenericWindowManager : MonoBehaviour
{
    public GenericWindow1[] genericWindows;

    // Start is called before the first frame update
    void Start()
    {
        ResetWindows();
    }

    public void ResetWindows()
    {
        genericWindows = GetComponentsInChildren<GenericWindow1>();
    }

    public virtual void OpenAllWindows()
    {
        if (genericWindows == null) { return; }
        foreach (GenericWindow1 genericWindow in genericWindows)
        {
            genericWindow.Open();
        }
    }

    public virtual void OpenAllWindowsAndCompleteAsync()
    {
        if (genericWindows == null) { return; }
        foreach (GenericWindow1 genericWindow in genericWindows)
        {
            genericWindow.OpenAndCompleteAsync();
        }
    }

    public virtual void CloseAllWindows()
    {
        if (genericWindows == null) { return; }
        foreach (GenericWindow1 genericWindow in genericWindows)
        {
            genericWindow.Close();
        }
    }

    public virtual void CloseAllWindowsHard()
    {
        if (genericWindows == null) { return; }
        foreach (GenericWindow1 genericWindow in genericWindows)
        {
            genericWindow.CloseHard();
        }
    }

    public void CloseAllWindowsAndCompleteAsync()
    {
        if (genericWindows == null) { return; }
        foreach (GenericWindow1 genericWindow in genericWindows)
        {
            genericWindow.CloseAndCompleteAsync();
        }
    }

}
