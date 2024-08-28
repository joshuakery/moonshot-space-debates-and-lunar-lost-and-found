using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using CharterRules.GameStateModule;
using CharterRules.BallotModule;
using rlmg.logging;
using MoonshotTimer;

namespace CharterRules.Mission
{
    public class UIManager : MonoBehaviour
    {
        public GameState gameState;
        public UISequenceManager sequenceManager;

        public int currentWindowIndex;
        public List<GameEvent> windowEvents;
        public GameEvent closeAllWindowsEvent;
        public GameEvent gameplayEvent;
        public GameEvent setupNextQuestionEvent;
        public GameEvent endVotingEvent;
        public GameEvent resetCurrentEvent;

        public GameEvent startEvent
        {
            get
            {
                if (windowEvents != null && windowEvents.Count > 0)
                    return windowEvents[0];
                else
                    return null;
            }
        }

        public GameEvent currentEvent
        {
            get
            {
                if (windowEvents != null &&
                    windowEvents.Count > currentWindowIndex &&
                    currentWindowIndex >= 0)
                    return windowEvents[currentWindowIndex];
                else
                    return null;
            }
        }

        public GameEvent nextEvent
        {
            get
            {
                if (windowEvents != null &&
                    currentWindowIndex + 1 >= 0 &&
                    currentWindowIndex + 1 < windowEvents.Count
                    )
                    return windowEvents[currentWindowIndex + 1];
                else
                    return null;
            }
        }

        public GameEvent prevEvent
        {
            get
            {
                if (windowEvents != null &&
                    currentWindowIndex - 1 >= 0 &&
                    currentWindowIndex - 1 < windowEvents.Count
                    )
                    return windowEvents[currentWindowIndex - 1];
                else
                    return null;
            }
        }

        public GameEvent endingEvent
        {
            get
            {
                if (windowEvents != null && windowEvents.Count > 0)
                    return windowEvents[windowEvents.Count - 1];
                else
                    return null;
            }
        }

        public Timer mainTimer;
        public Timer roundTimer;
        public Timer nextRoundTimer;
        public Timer bufferTimer;
        public TimerDisplay timerDisplay;

        public float timeNeededForAnotherRound = 20f;

        public bool tallyingVotes; //set to false in SetupNextQuestion listener

        //========Methods=========
        private void Start()
        {
            StartCoroutine(ResetGame(false));
        }

        public IEnumerator ResetGame()
        {
            yield return StartCoroutine(_ResetGame(true));
        }

        public IEnumerator ResetGame(bool doRestart)
        {
            yield return StartCoroutine(_ResetGame(doRestart));
        }

        private IEnumerator _ResetGame(bool doRestart)
        {
            gameState.Reset();

            tallyingVotes = false;

            roundTimer.Reset(); //ok to reset round timer because it's never set by server

            if (mainTimer.time > 0)
                timerDisplay.timer = mainTimer;
            else
                timerDisplay.timer = bufferTimer;

            if (doRestart) { yield return StartCoroutine(LateStart()); }
            else { yield return StartCoroutine(LateClose()); }
        }

        private IEnumerator LateStart()
        {
            yield return null; //just need to wait one frame

            closeAllWindowsEvent.Raise();
            startEvent.Raise();
        }

        private IEnumerator LateClose()
        {
            yield return null;
            closeAllWindowsEvent.Raise();
            currentWindowIndex = -1;
        }

        private void OnEnable()
        {
            SetCurrentWindowListeners();
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

        public void GoToConclusion()
        {
            string finalResultsSceneName = "ResultsScene";
            SceneManager.LoadScene(finalResultsSceneName);  //alternative string-based approach
        }

        public void OnMainTimeout(float wait = 0f)
        {
            if (currentEvent == gameplayEvent)
            {
                if (!tallyingVotes)
                {
                    endVotingEvent.Raise();
                    tallyingVotes = true;
                }
            }
            else
            {
                StartCoroutine(WrapUp(wait));
            }

            if (bufferTimer != null)
            {
                bufferTimer.Reset();
                bufferTimer.StartCounting();
                //switch timer display to buffer timer on endingEvent
            }
        }

        private IEnumerator WrapUp(float wait)
        {
            mainTimer.PauseCounting();
            mainTimer.SetToZero();

            yield return new WaitForSeconds(wait);

            closeAllWindowsEvent.Raise();
            endingEvent.Raise();
        }

        public void OnRoundTimeout()
        {
            if (!tallyingVotes)
            {
                endVotingEvent.Raise();
                tallyingVotes = true;
            }
        }

        public void StartTallyingVotes()
        {
            tallyingVotes = true;
        }

        public void StopTallyingVotes()
        {
            tallyingVotes = false;
        }

        //========Next / Prev Question========
        public void NextQuestionRound()
        {
            string currentQuestionID = gameState.allQuestionDataOrder[gameState.currentQuestionIndex];
            int[] winners = gameState.GetWinners(currentQuestionID);


            if (  (
                    (Client.instance == null || Client.instance.missionState != MissionState.Paused)
                    &&
                    mainTimer.time == 0
                  ) //we're out of time!
                  || 
                  (
                     mainTimer.isCounting
                     &&
                     mainTimer.time <= timeNeededForAnotherRound
                   ) //we're out of time, so long as the timer is not paused!
            ) //so let's finish this round and end the game
            {
                StartCoroutine(WrapUp(0f));
            }
            else if (roundTimer.time > 0) //Debug Feature, just get to the end of the round timer
            {
                if (winners.Length > 0)
                {
                    endVotingEvent.Raise();
                }
                else //Debug Feature, if no votes, skip this question and move on
                {
                    if (gameState.allQuestionData != null &&
                        gameState.allQuestionData.Count > 0 &&
                        gameState.currentQuestionIndex + 1 >= gameState.allQuestionData.Count
                    ) //no more questions;
                    {
                        //closeAllWindowsEvent.Raise();
                        //endingEvent.Raise();
                        StartCoroutine(WrapUp(0f));
                    }
                    else //there is another question and it's okay to move on
                    {
                        gameState.currentQuestionIndex += 1;
                        setupNextQuestionEvent.Raise();
                    }
                }
            }
            else
            {
                QuestionVotes questionVotes = gameState.allQuestionVotes.ContainsKey(currentQuestionID) ?
                    gameState.allQuestionVotes[currentQuestionID] :
                    null;
                if (winners.Length > 1 && questionVotes != null && questionVotes.tiesCount < 1) //it's a tie and it's the first tie - redo the question
                {
                    questionVotes.tiesCount += 1;
                    resetCurrentEvent.Raise();

                }
                else
                {
                    if (
                        (gameState.allQuestionData != null &&
                        gameState.allQuestionData.Count > 0 &&
                        gameState.currentQuestionIndex + 1 >= gameState.allQuestionData.Count) //no more questions;
                        ||
                        (gameState.allQuestionVotes.Count >= gameState.targetNumQuestions) //or we've reach our goal
                    )
                    {
                        //closeAllWindowsEvent.Raise();
                        //endingEvent.Raise();
                        StartCoroutine(WrapUp(0f));
                    }
                    else //there is another question and it's okay to move on
                    {
                        gameState.currentQuestionIndex += 1;
                        setupNextQuestionEvent.Raise();
                    }
                }
            }
        }

        private void PrevQuestionRound()
        {
            if (gameState.allQuestionData != null &&
                gameState.allQuestionData.Count > 0)
            {
                if (gameState.currentQuestionIndex - 1 >= 0)
                {
                    endVotingEvent.Raise();
                    gameState.currentQuestionIndex -= 1;
                    setupNextQuestionEvent.Raise();
                }
                else
                {
                    //no more questions;
                    endVotingEvent.Raise();
                    closeAllWindowsEvent.Raise();
                    prevEvent.Raise();
                }
            }
        }

        private void Update()
        {
            // if (Input.touchCount > 0)
            // {
            //     RLMGLogger.Instance.Log("Touches detected: " + Input.touchCount, MESSAGETYPE.INFO);
            //     for ( var i = 0 ; i < Input.touchCount ; i++ ) {
            //         var touch = Input.GetTouch(i);
            //         RLMGLogger.Instance.Log(touch.ToString(), MESSAGETYPE.INFO);
            //     }
            // }

            //Events
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (currentEvent == gameplayEvent)
                {
                    sequenceManager.CompleteCurrentSequence();
                    NextQuestionRound();
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentEvent == gameplayEvent)
                {
                    sequenceManager.CompleteCurrentSequence();
                    PrevQuestionRound();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (nextEvent != null)
                {
                    sequenceManager.CompleteCurrentSequence();
                    if (nextEvent == startEvent)
                    {
                        closeAllWindowsEvent.Raise();
                        gameState.Reset();
                        nextEvent.Raise();
                    }
                    else
                    {
                        closeAllWindowsEvent.Raise();
                        nextEvent.Raise();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (prevEvent != null)
                {
                    sequenceManager.CompleteCurrentSequence();
                    if (prevEvent == startEvent)
                    {
                        closeAllWindowsEvent.Raise();
                        gameState.Reset();
                        prevEvent.Raise();
                    }
                    else
                    {
                        mainTimer.Reset();

                        closeAllWindowsEvent.Raise();
                        prevEvent.Raise();
                    }
                }
            }
            //Round Timer Controls
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                mainTimer.time = 2;
                mainTimer.StartCounting();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                roundTimer.time = 2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                mainTimer.time = 12;
                mainTimer.StartCounting();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                roundTimer.time = 12;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                nextRoundTimer.time = 0;
            }


            //Reset
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                closeAllWindowsEvent.Raise();
                StartCoroutine(ResetGame());
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                gameState.DeleteAllProgressFromFile();
                closeAllWindowsEvent.Raise();
                StartCoroutine(ResetGame());
            }
        }

        private void OnDestroy()
        {
            if (gameState.allQuestionVotes != null)
            {
                foreach (string questionID in gameState.allQuestionVotes.Keys)
                {
                    QuestionVotes questionVotes = gameState.allQuestionVotes[questionID];
                    if (questionVotes != null)
                    {
                        Debug.Log(
                            String.Format(
                                "Round {0}: Time to complete voting: {1}",
                                questionID,
                                questionVotes.time.ToString()
                            )
                        );
                    }
                }
            }
        }

    }

}

