using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using CharterRules.SpaceNow.GameStateModule;

namespace CharterRules.SpaceNow
{
	public class SpaceNowUIManager : MonoBehaviour
	{
		public SpaceNowGameState gameState;

		public int currentWindowIndex;
		public List<GameEvent> windowEvents;
		public GameEvent closeAllWindowsEvent;
        public GameEvent gameplayEvent;
		public GameEvent setupNextQuestionEvent;
		public GameEvent endVotingEvent;

		public Canvas primaryCanvas;
		private GenericWindowManager primaryWindowManager;
		public Canvas secondaryCanvas;
		private GenericWindowManager secondaryWindowManager;

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
				if (windowEvents != null && windowEvents.Count > currentWindowIndex)
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

        //========Methods=========
        private void Awake()
        {
			primaryWindowManager = primaryCanvas.gameObject.GetComponent<GenericWindowManager>();
			secondaryWindowManager = secondaryCanvas.gameObject.GetComponent<GenericWindowManager>();
		}

        private void Start()
		{
			ResetGame();
        }

		private void ResetGame()
		{
			gameState.Reset();
			StartCoroutine(LateStart());
		}

		private IEnumerator LateStart()
		{
			yield return null; //just need to wait one frame for config to load

			primaryWindowManager.ResetWindows();
			secondaryWindowManager.ResetWindows();
			primaryWindowManager.OpenAllWindowsAndCompleteAsync();
			secondaryWindowManager.OpenAllWindowsAndCompleteAsync();

			yield return null; //wait for DOTween to complete animations

			primaryCanvas.gameObject.SetActive(false);
			secondaryCanvas.gameObject.SetActive(false);

			yield return null; //wait for state change to be noticed

			primaryCanvas.gameObject.SetActive(true);
			secondaryCanvas.gameObject.SetActive(true);

			//not redundant - in case we have some extras in the hierarchy
			primaryWindowManager.CloseAllWindowsAndCompleteAsync();
			secondaryWindowManager.CloseAllWindowsAndCompleteAsync();

			//continue with standard reset
			closeAllWindowsEvent.Raise();
            startEvent.Raise();
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

        //========Next / Prev Question========
        public void NextQuestionRound()
        {
            if (gameState.nextQuestion == null || gameState.questionVotes.Count >= gameState.targetNumQuestions)
            {
				if (closeAllWindowsEvent != null)
					closeAllWindowsEvent.Raise();
				if (nextEvent != null)
					nextEvent.Raise();
				
            }
            else //there is another question and it's okay to move on
            {
                gameState.currentQuestionIndex += 1;

				if (setupNextQuestionEvent != null)
					setupNextQuestionEvent.Raise();
            }
        }

        private void PrevQuestionRound()
        {
			if (gameState.prevQuestion == null)
			{
				if (closeAllWindowsEvent != null)
					closeAllWindowsEvent.Raise();
				if (prevEvent != null)
					prevEvent.Raise();
			}
			else
            {
				endVotingEvent.Raise();

				gameState.currentQuestionIndex -= 1;

				setupNextQuestionEvent.Raise();
			}

        }

        private void Update()
        {
            //Events
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (nextEvent != null)
                {
                    if (nextEvent == startEvent)
                    {
                        closeAllWindowsEvent.Raise();
                        gameState.Reset();
                        nextEvent.Raise();
                    }
                    else if (nextEvent == gameplayEvent)
                    {
                        NextQuestionRound();
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
                    if (prevEvent == startEvent)
                    {
                        closeAllWindowsEvent.Raise();
                        gameState.Reset();
                        prevEvent.Raise();
                    }
                    else if (prevEvent == gameplayEvent)
                    {
                        PrevQuestionRound();
                    }
                    else
                    {
                        closeAllWindowsEvent.Raise();
                        prevEvent.Raise();
                    }
                }
            }
            ////Canvas Swap
            //else if (Input.GetKeyDown(KeyCode.S))
            //{
            //    SwapCanvasDisplays();
            //}
            ////Canvas Rotate
            //else if (Input.GetKeyDown(KeyCode.A))
            //{
            //    RotatePrimaryCanvasDisplay();
            //}

            //Reset
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                closeAllWindowsEvent.Raise();
                ResetGame();
            }

			else if (Input.GetKeyDown(KeyCode.R))
            {
				Canvas.ForceUpdateCanvases();
				Debug.Log("canvases updated");
            }
            //else if (Input.GetKeyDown(KeyCode.K))
            //{
            //    gameState.DeleteAllProgressFromFile();
            //    closeAllWindows.Raise();
            //    ResetGame();
            //}
        }

    }
}


