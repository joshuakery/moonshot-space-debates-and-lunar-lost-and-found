using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharterRules.GameStateModule;

public class CallBallotsDownAudio : MonoBehaviour
{
    public GameState gameState;
    public GameEvent BallotsDownAudioEvent;

    public bool ballotsHaveComeIn = false;
    
    public void CallAudio()
    {
        if (ballotsHaveComeIn)
        {
            if (BallotsDownAudioEvent != null)
                BallotsDownAudioEvent.Raise();
        }
        else
        {
            ballotsHaveComeIn = true;
        }
    }

    public void Reset()
    {
        ballotsHaveComeIn = false;
    }
}
