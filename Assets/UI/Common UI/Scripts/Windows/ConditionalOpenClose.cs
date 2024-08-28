using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerHunt.GameStateModule;

public class ConditionalOpenClose : MonoBehaviour
{
    public GameEvent CloseAllSecondaryWindowsEvent;
    public GameEvent path1;
    public GameEvent path2;

    public GameState gameState;

    public GenericWindow1 targetWindow;

    public void ConditionalRaiseEvent()
    {
        if (targetWindow.isOpen)
        {
            if (gameState.found.Count >= gameState.target)
                path2.Raise();
            else
                path1.Raise();
        }
    }
}
