using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float time;
    public float duration;
    public float elapsed;

    public bool isCounting;

    public UnityEvent timeOutEvent;

    public Timer LessThanOrEqualToThisTimer;

    [Serializable]
    public class TimeWarning
    {
        public bool isWarning;
        public UnityEvent warningEvent;

        [Tooltip("Time (in seconds) when time warnings are triggered.")]
        public List<int> times;
    }


    public List<TimeWarning> timeWarnings;

    // Start is called before the first frame update
    void Start()
    {
        isCounting = false;

        foreach (TimeWarning timeWarning in timeWarnings)
        {
            timeWarning.isWarning = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCounting &&
            (LessThanOrEqualToThisTimer == null || LessThanOrEqualToThisTimer.isCounting)
            )
        {
            time -= Time.deltaTime;
            elapsed += Time.deltaTime;
            if (time <= 0)
            {
                time = 0;
                isCounting = false;
                timeOutEvent.Invoke();
            }
            else if (LessThanOrEqualToThisTimer != null &&
                     time > LessThanOrEqualToThisTimer.time)
            {
                time = LessThanOrEqualToThisTimer.time;
            }
            RingWarnings();
        }
    }

    private void RingWarnings()
    {
        foreach (TimeWarning timeWarning in timeWarnings)
        {
            if (!timeWarning.isWarning && IsWarningTime(timeWarning))
            {
                timeWarning.warningEvent.Invoke();
                timeWarning.isWarning = true;
            }
            else if (timeWarning.isWarning && !IsWarningTime(timeWarning))
            {
                timeWarning.isWarning = false;
            }
        }
    }

    private bool IsWarningTime(TimeWarning timeWarning)
    {
        int currentTime = (int)Mathf.Ceil(time);
        foreach(int wTime in timeWarning.times)
        {
            if (wTime == currentTime)
            {
                return true;
            }
        }
        return false;
    }

    public void StartCounting()
    {
        isCounting = true;
    }

    public void PauseCounting()
    {
        isCounting = false;
    }

    public void SetToZero()
    {
        time = 0;
        elapsed = 0;
        isCounting = false;
    }

    public void Reset()
    {
        time = duration;
        elapsed = 0;
        isCounting = false;
    }

    public void SetAt(int i)
    {
        time = i;
        elapsed = 0;
        isCounting = false;
    }

}
