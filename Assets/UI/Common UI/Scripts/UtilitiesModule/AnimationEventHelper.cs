using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{

    [Serializable]
    public class EventsToTrigger
    {
        public string trigger;
        public List<UnityEvent> mainEvents;
        public float delayBase;
        public float delayMultiplier;
        public UnityEvent callbackEvent;
    }

    [SerializeField]
    public List<EventsToTrigger> allEventsToTrigger;

    public Dictionary<string,EventsToTrigger> allEventsToTriggerDict;

    private void Start()
    {
        allEventsToTriggerDict = new Dictionary<string,EventsToTrigger>();
        foreach (EventsToTrigger eventsToTrigger in allEventsToTrigger)
        {
            allEventsToTriggerDict.Add(eventsToTrigger.trigger,eventsToTrigger);
        }
    }

    public void ResetAllTriggers()
    {
        
        Animator animator = gameObject.GetComponent<Animator>();
        animator.ResetAllTriggers();
    }

    public void TriggerAnimations(string trigger)
    {
        if (!allEventsToTriggerDict.ContainsKey(trigger)) return;

        EventsToTrigger eventsToTrigger = allEventsToTriggerDict[trigger];

        if (eventsToTrigger.mainEvents.Count > 0)
        {
            foreach(UnityEvent mainEvent in eventsToTrigger.mainEvents)
            {
                mainEvent.Invoke();
            }
        }

        StartCoroutine(DelayedTrigger(eventsToTrigger.delayBase, eventsToTrigger.callbackEvent));

    }

    private IEnumerator DelayedTrigger(float delay, UnityEvent delayedEvent)
    {
        yield return new WaitForSeconds(delay);
        delayedEvent.Invoke();
    }

    public void TriggerStaggeredAnimations(string trigger)
    {
        int i = 0;
        EventsToTrigger eventsToTrigger = allEventsToTriggerDict[trigger];
        foreach (UnityEvent mainEvent in eventsToTrigger.mainEvents)
        {
            float delay = (float)i * eventsToTrigger.delayMultiplier + eventsToTrigger.delayBase;
            StartCoroutine(DelayedTrigger(delay,mainEvent));
            i++;
        }
        float callbackDelay = (float)i*eventsToTrigger.delayMultiplier + eventsToTrigger.delayBase;
        StartCoroutine(DelayedTrigger(callbackDelay, eventsToTrigger.callbackEvent));
    }

    public void SetAnimatorBoolOn(string bName)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetBool(bName, true);
    }

    public void SetAnimatorBoolOff(string bName)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetBool(bName, false);
    }

    public void SetIntegerToNegativeOne(string iName)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetInteger(iName, -1);
    }

    public void SetRandomIntegerOfTwo(string iName)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetInteger(iName, UnityEngine.Random.Range(0,2));
    }

    public void SetRandomIntegerOfThree(string iName)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetInteger(iName, UnityEngine.Random.Range(0,3));
    }

    public void SetGameObjectInactive ()
    {
        gameObject.SetActive(false);
    }

    // public void AnimateUIToFullHeight(float duration)
    // {
    //     RectTransform rt = GetComponent<RectTransform>();
    //     float startPercent = 0f;
    //     float endPercent = 1f;
    //     EasingFunction.Ease ease = EasingFunction.Ease.EaseInOutSine;

    //     StartCoroutine( AnimateUIPercentHeight(rt,startPercent,endPercent,duration,ease) );
    // }

    // private IEnumerator AnimateUIPercentHeight(RectTransform rt, float startPercent, float endPercent, float duration, EasingFunction.Ease ease)
    // {
    //     EasingFunction.Function easingFunc = EasingFunction.GetEasingFunction(ease);
    //     float actualHeight = rt.sizeDelta[1];
    //     Debug.Log(actualHeight);

    //     float elapsed = 0.0f;
    //     while (elapsed < duration)
    //     {
    //         float interpolationPoint = elapsed/duration;

    //         float percentHeight = easingFunc(startPercent,endPercent,interpolationPoint);
    //         float height = (percentHeight * actualHeight);
    //         Debug.Log(height);
    //         rt.sizeDelta = new Vector2(rt.sizeDelta[0],height);

    //         elapsed += Time.deltaTime;
    //         yield return null;
    //     }
    //     rt.sizeDelta = new Vector2(rt.sizeDelta[0],actualHeight);
    // }

    // public void AnimateTemporaryPanelToMyHeight(GameObject panelPrefab)
    // {
    //     RectTransform rt = GetComponent<RectTransform>();

    //     GameObject panel = Instantiate(panelPrefab, transform.parent);
    //     LayoutElement le = panel.GetComponent<LayoutElement>();

    //     float start = 0f;
    //     float end = rt.sizeDelta[1] - 40f;

    //     float duration = 2f;
        
    //     EasingFunction.Ease ease = EasingFunction.Ease.Linear;

    //     StartCoroutine( WaitToDestroy(panel,AnimateLayoutElementMinHeight(le,start,end,duration,ease)) );

        
    // }

    // private IEnumerator WaitToDestroy(GameObject toDestroy, IEnumerator waitFunction)
    // {
    //     yield return StartCoroutine( waitFunction );
    //     GameObject.Destroy(toDestroy);
    // }

    // private IEnumerator AnimateLayoutElementMinHeight(LayoutElement le, float start, float end, float duration, EasingFunction.Ease ease)
    // {
    //     EasingFunction.Function easingFunc = EasingFunction.GetEasingFunction(ease);

    //     float elapsed = 0.0f;
    //     while (elapsed < duration)
    //     {
    //         float interpolationPoint = elapsed/duration;

    //         float minHeight = easingFunc(start,end,interpolationPoint);
    //         le.minHeight = minHeight;

    //         elapsed += Time.deltaTime;
    //         yield return null;
    //     }
    //     le.minHeight = end;
    // }

}
