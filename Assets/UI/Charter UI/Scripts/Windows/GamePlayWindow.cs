using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharterRules.GameStateModule;
using CharterRules.QuestionsLoaderModule;
using CharterRules.Mission.WindowsModule;

namespace CharterRules.Mission.WindowsModule
{
    public class GamePlayWindow : GenericWindow
    {
        public GameEvent SetupNextQuestion;

        public TMP_Text number;
        public TMP_Text title;
        public TMP_Text situation;
        public RawImage optionAArguer;
        public TMP_Text optionA;
        public RawImage optionBArguer;
        public TMP_Text optionB;

        public TMP_Text resolutionHeader;
        public TMP_Text resolutionLetter;
        public Image resolutionLetterCircle;
        public Sprite optionALetterCircle;
        public Sprite optionBLetterCircle;

        public Sprite optionXLetterCircle;

        public TMP_Text resolution;

        public Timer mainTimer;

        [SerializeField]
        private Animator animationController;

        private void Awake()
        {
            if (mainTimer == null)
            {
                GameObject timerObj = GameObject.Find("Main Timer");
                if (timerObj != null)
                    mainTimer = timerObj.GetComponent<Timer>();
            }
                
        }

        private void OnEnable()
        {
            SetTexts();
        }

        public void SetTexts()
        {
            if (gameState.allQuestionData == null ||
                gameState.allQuestionData.Count == 0) return;

            string currentQuestionID = gameState.allQuestionDataOrder[gameState.currentQuestionIndex];
            QuestionData currentQuestionData = gameState.allQuestionData[currentQuestionID];

            if (number != null)
                number.text = "Question " + (gameState.currentQuestionIndex + 1).ToString();

            if (title != null)
                title.text = currentQuestionData.title;

            if (situation != null)
                situation.text = currentQuestionData.situation;

            if (optionAArguer != null)
                optionAArguer.texture = currentQuestionData.options.optionA.arguerImageTex;

            if (optionA != null)
                optionA.text = currentQuestionData.options.optionA.argument;

            if (optionBArguer != null)
                optionBArguer.texture = currentQuestionData.options.optionB.arguerImageTex;

            if (optionB != null)
                optionB.text = currentQuestionData.options.optionB.argument;

        }

        public void ShowWinner()
        {
            if (!resolutionHeader || !resolutionLetter || !resolution) return;
            if (gameState.allQuestionData == null ||
                gameState.allQuestionData.Count == 0) return;

            string currentQuestionID = gameState.allQuestionDataOrder[gameState.currentQuestionIndex];
            QuestionData currentQuestionData = gameState.allQuestionData[currentQuestionID];
            QuestionVotes currentQuestionVotes = gameState.allQuestionVotes[currentQuestionID];

            int[] winners = gameState.GetWinners(currentQuestionID);
            if (winners.Length == 0)
            {
                resolutionHeader.text = "NOBODY VOTED!";
                resolutionLetter.text = "?";
                resolutionLetterCircle.sprite = optionXLetterCircle;
                resolution.text = "Let’s move on.";
            }
            else if (winners.Length > 1)
            {
                resolutionLetter.text = "X";
                resolutionLetterCircle.sprite = optionXLetterCircle;

                if (currentQuestionVotes.tiesCount == 0)
                {
                    resolutionHeader.text = "IT’S A TIE!";

                    if (mainTimer != null && mainTimer.time == 0)
                    {
                        resolution.text = "Let’s wrap up.";
                    }
                    else
                    {
                        resolution.text = "Your team needs to <nobr>make a decision!</nobr>";
                    }
                }
                else
                {
                    resolutionHeader.text = "IT’S A TIE AGAIN!";

                    if (mainTimer != null && mainTimer.time == 0)
                    {
                        resolution.text = "Let's wrap up.";
                    }
                    else
                    {
                        resolution.text = "Let’s move on.";
                    }
                }
            }
            else
            {
                int winner = winners[0];

                resolutionHeader.text = "YOUR TEAM CHOSE";

                if (winner == 0)
                {
                    if (resolutionLetter != null)
                        resolutionLetter.text = "A";

                    if (resolutionLetterCircle != null)
                        resolutionLetterCircle.sprite = optionALetterCircle;

                    if (resolution != null)
                        resolution.text = currentQuestionData.options.optionA.resolution;
                }
                else if (winner == 1)
                {
                    if (resolutionLetter != null)
                        resolutionLetter.text = "B";

                    if (resolutionLetterCircle != null)
                        resolutionLetterCircle.sprite = optionBLetterCircle;

                    if (resolution != null)
                        resolution.text = currentQuestionData.options.optionB.resolution;
                }
            }
        }

        public void ShowQuestion()
        {
            animationController.SetBool("Show", true);
        }

    }
}


