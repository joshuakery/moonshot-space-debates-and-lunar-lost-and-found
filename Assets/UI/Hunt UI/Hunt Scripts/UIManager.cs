using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ScavengerHunt.GameStateModule;
using MoonshotTimer;

namespace ScavengerHunt.UIManagerModule
{
    public class UIManager : MonoBehaviour
    {
        public GameState gameState;
        public GameEvent CloseAllWindowsEvent;
        public GameEvent ModalClose;
        public GameEvent CloseAllWindowsStartOverEvent;
        public GameEvent ModalCloseStartOverEvent;
        public GameEvent HARDCloseAllEvent;
        public GameEvent StartEvent;
        public GameEvent EndingEvent
        {
            get
            {
                if (windowEvents != null && windowEvents.Count > 0)
                    return windowEvents[windowEvents.Count - 1];
                else
                    return null;
            }
        }
        public UISequenceManager uiSequenceManager;
        //debug
        public Timer mainTimer;
        public Timer bufferTimer;
        public TimerDisplay[] timerDisplays;
        public int currentWindowIndex;
        public List<GameEvent> windowEvents;

        public GameEvent currentEvent
        {
            get
            {
                if (windowEvents != null && windowEvents.Count > currentWindowIndex)
                    return windowEvents[currentWindowIndex];
                else
                    return null;
            }
        }

        //========Methods=========
        private void Start()
        {
            SetCurrentWindowListeners();
            StartCoroutine(ResetGame());
        }

        private void SetCurrentWindowListeners()
        {
            for (int i = 0; i < windowEvents.Count; i++)
            {
                GameObject empty = new GameObject("Current Window Listener");
                empty.SetActive(false);

                GameEventListener listener = empty.AddComponent<GameEventListener>();

                listener.Event = windowEvents[i];

                int k = i; //capture the value of i, not the variable
                listener.Response = new UnityEvent();
                listener.Response.AddListener(delegate {
                    currentWindowIndex = k;
                });

                empty.transform.parent = gameObject.transform;
                empty.SetActive(true);
            }
        }

        public IEnumerator ResetGame()
        {
            yield return StartCoroutine(_ResetGame(true));
        }

        public IEnumerator ResetGame(bool doRestart)
        {
            yield return StartCoroutine(_ResetGame(doRestart));
        }

        public IEnumerator _ResetGame(bool doRestart)
        {
            gameState.Reset();

            if (timerDisplays != null)
                foreach (TimerDisplay timerDisplay in timerDisplays)
                {
                    if (mainTimer.time > 0)
                        timerDisplay.timer = mainTimer;
                    else
                        timerDisplay.timer = bufferTimer;
                }

            if (doRestart)
                yield return StartCoroutine(LateStart());
            else
                yield return StartCoroutine(LateClose());
        }

        private IEnumerator LateStart()
        {
            yield return StartCoroutine(_LateStart());
        }

        private IEnumerator _LateStart()
        {
            yield return null; //just need to wait one frame minimum
            if (uiSequenceManager != null) { uiSequenceManager.CompleteCurrentSequence(); }

            HARDCloseAllEvent.Raise();
            yield return null;

            StartEvent.Raise();
        }

        private IEnumerator LateClose()
        {
            yield return null;
            HARDCloseAllEvent.Raise();
        }

        public void StartOver()
        {
            ModalCloseStartOverEvent.Raise();
            CloseAllWindowsStartOverEvent.Raise();

            StartCoroutine(ResetGame()); //wait for close animations
        }

        public void StartOver(bool doResetGame)
        {
            ModalCloseStartOverEvent.Raise();
            CloseAllWindowsStartOverEvent.Raise();

            if (doResetGame) { StartCoroutine(ResetGame()); } //wait for close animations
        }

        public void OnMainTimeout(float delay)
        {
            StartCoroutine(WrapUp(delay));

            if (bufferTimer != null)
            {
                if (timerDisplays != null)
                    foreach (TimerDisplay timerDisplay in timerDisplays)
                        timerDisplay.timer = bufferTimer;

                bufferTimer.Reset();
                bufferTimer.StartCounting();
            }
        }

        private IEnumerator WrapUp(float delay)
        {
            mainTimer.PauseCounting();
            mainTimer.SetToZero();

            yield return new WaitForSeconds(delay);

            CloseAllWindowsEvent.Raise();
            EndingEvent.Raise();
        }

        public void GoToConclusion()
        {
            string finalResultsSceneName = "ResultsScene";
            SceneManager.LoadScene(finalResultsSceneName);  //alternative string-based approach
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartOver();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                gameState.DeleteAllFoundFromFile();
            }
            //Main Timer
            else if (Input.GetKeyDown(KeyCode.W))
            {
                GoToConclusion();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                mainTimer.time = 2;
                mainTimer.StartCounting();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                mainTimer.time = 12;
                mainTimer.StartCounting();
            }
            //Windows Events
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (windowEvents.Count > 0 && currentWindowIndex != null)
                {
                    currentWindowIndex = (currentWindowIndex + 1) % windowEvents.Count;
                    CloseAllWindowsEvent.Raise();
                    if (mainTimer)
                        mainTimer.time = mainTimer.duration;
                    if (currentWindowIndex == 0)
                    {
                        gameState.Reset();
                    }
                    windowEvents[currentWindowIndex].Raise();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (windowEvents.Count > 0 && currentWindowIndex != null)
                {
                    currentWindowIndex -= 1;
                    if (currentWindowIndex < 0) currentWindowIndex = windowEvents.Count - 1;
                    CloseAllWindowsEvent.Raise();
                    if (mainTimer)
                        mainTimer.time = mainTimer.duration;
                    if (currentWindowIndex == 0)
                    {
                        gameState.Reset();
                    }
                    windowEvents[currentWindowIndex].Raise();
                }
            }

        }

    }
}


