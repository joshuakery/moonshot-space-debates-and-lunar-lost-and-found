using System;
using UnityEngine;
using TMPro;

namespace MoonshotTimer
{
    public class TimerDisplay : MonoBehaviour
    {
        public Timer timer;
        public TMP_Text timerDisplay;

        public bool displayAsMinutes;

        public string defaultTimerName;

        public float mspace = 0.7f;

        private void Start()
        {
            if (!timer && !String.IsNullOrEmpty(defaultTimerName))
                timer = GameObject.Find(defaultTimerName).GetComponent<Timer>();
        }

        private void Update()
        {
            float timeToDisplay = timer.time;

            string display;
            if (displayAsMinutes)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(Mathf.Ceil(timeToDisplay));
                display = String.Format(@"<mspace={1}em>{0:%m}</mspace>:<mspace={1}em>{0:ss}</mspace>", timeSpan, mspace);
            }
            else
            {
                display = Mathf.Round(timeToDisplay).ToString();
            }

            timerDisplay.text = display;
        }

        public void SwitchTimer(Timer t)
        {
            timer = t;
        }

    }
}


