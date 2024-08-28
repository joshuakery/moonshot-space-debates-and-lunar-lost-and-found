using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[CreateAssetMenu(fileName = "UISequenceManager", menuName = "UI Sequence Manager")]
public class UISequenceManager : ScriptableObject
{
    public Sequence currentSequence;

    public void CompleteCurrentSequence()
    {
        if (currentSequence != null)
            currentSequence.Complete();

        currentSequence = null;
    }

    public void AppendInterval(float interval)
    {
        if (currentSequence == null)
            currentSequence = DOTween.Sequence();

        currentSequence.AppendInterval(interval);
        //Debug.Log("Appended interval, new duration: " + currentSequence.Duration().ToString());
    }

    public void AppendCallback(GameEvent gameEvent)
    {
        if (currentSequence == null)
            currentSequence = DOTween.Sequence();

        currentSequence.AppendCallback(() =>
           {
               gameEvent.Raise();
           }
        );
    }

    public void AppendCallback(TweenCallback callback)
    {
        if (currentSequence == null)
            currentSequence = DOTween.Sequence();

        currentSequence.AppendCallback(callback);
    }

    public void InsertCallback(float atPosition, TweenCallback callback)
    {
        if (currentSequence == null)
            currentSequence = DOTween.Sequence();

        currentSequence.InsertCallback(atPosition, callback);
    }

    public void CreateNewSequenceAfterCurrent()
    {
        if (currentSequence == null)
        {
            currentSequence = DOTween.Sequence();
        }
        else
        {
            float remaining = currentSequence.Duration() - currentSequence.Elapsed();
            currentSequence = DOTween.Sequence();
            currentSequence.AppendInterval(remaining);
        }
    }

    //Debug
    public void _LogCurrentDuration()
    {
        //if (currentSequence != null)
        //    Debug.Log(System.String.Format("Current sequence duration is {0}", currentSequence.Duration()));
        //else
        //    Debug.Log("Current sequence is null.");
    }
}
