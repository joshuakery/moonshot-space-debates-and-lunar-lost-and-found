using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/GameEventInt"), System.Serializable]
public class GameEventInt : ScriptableObject
{
	private List<GameEventListenerInt> listeners = 
		new List<GameEventListenerInt>();

    public void Raise(int n)
    {
        for(int i = listeners.Count -1; i >= 0; i--) {
            listeners[i].OnEventRaised(n);
        }

    }

    public void RegisterListener(GameEventListenerInt listener)
    { listeners.Add(listener); }

    public void UnregisterListener(GameEventListenerInt listener)
    { listeners.Remove(listener); }
}