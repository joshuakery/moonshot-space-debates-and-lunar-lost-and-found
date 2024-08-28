using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyIntEvent : UnityEvent<int>
{
}

public class GameEventListenerInt : MonoBehaviour
{
    public GameEventInt Event;
    public MyIntEvent Response;

    private void OnEnable()
    { Event.RegisterListener(this); }
 
    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(int n)
    { Response.Invoke(n); }
}
