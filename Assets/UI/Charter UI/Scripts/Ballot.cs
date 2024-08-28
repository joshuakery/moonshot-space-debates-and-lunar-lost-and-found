using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;
using CharterRules.GameStateModule;
using CharterRules.QuestionsLoaderModule;

namespace CharterRules.BallotModule
{
    public class Ballot : MonoBehaviour
    {
        public GameState gameState;

        public TMP_Text number;
        public TMP_Text onBallotSituation;
        public TMP_Text optionA;
        public TMP_Text optionB;
        public Button voteButton;

        public GameObject timerDisplay;

        public int ballotID;

        public int selectedOption;

        public Sprite optionACircleUncast;
        public Sprite optionBCircleUncast;
        public Sprite optionACircle;
        public Sprite optionBCircle;
        public Image choiceDisplayCircle;
        public TMP_Text choiceDisplayLetter;
        public TMP_Text choiceDisplay;
        public TMP_Text confirmation;

        public GenericWindow1 bobAnimation;

        [SerializeField]
        private Animator animationController;

        private void Start()
        {
            ballotID = gameObject.GetInstanceID();
        }

        private void OnEnable()
        {
            SetTexts();
        }

        public void SetTexts()
        {
            if (gameState.allQuestionData != null &&
                gameState.allQuestionData.Count > 0)
            {
                string currentQuestionID = gameState.allQuestionDataOrder[gameState.currentQuestionIndex];
                QuestionData questionData = gameState.allQuestionData[currentQuestionID];

                if (number != null)
                    number.text = "QUESTION " + (gameState.currentQuestionIndex + 1).ToString();
                //number.text = "Your Ballot";

                if (onBallotSituation != null)
                    onBallotSituation.text = questionData.onBallot;

                if (optionA != null)
                    optionA.text = questionData.options.optionA.resolution;

                if (optionB != null)
                    optionB.text = questionData.options.optionB.resolution;
            }
        }

        public void SelectOption(int optionID)
        {
            selectedOption = optionID;

            if (gameState.allQuestionData != null &&
                gameState.allQuestionData.Count > 0)
            {
                string currentQuestionID = gameState.allQuestionDataOrder[gameState.currentQuestionIndex];
                QuestionData questionData = gameState.allQuestionData[currentQuestionID];

                string chosen = selectedOption == 0 ?
                    questionData.options.optionA.resolution :
                    questionData.options.optionB.resolution;

                if (choiceDisplay != null)
                    choiceDisplay.text = chosen;
            }
        }

        public void OnVote()
        {
            gameState.Vote(ballotID, selectedOption);

            // if (confirmation != null)
            //     confirmation.text = Convert.ToChar(65 + selectedOption).ToString();

            // ConfirmationSpriteCast();
        }

        public void ConfirmationSpriteCast()
        {
            if (choiceDisplayLetter != null)
                choiceDisplayLetter.text = Convert.ToChar(65 + selectedOption).ToString();

            if (choiceDisplayCircle != null)
            {
                if (selectedOption == 0)
                    choiceDisplayCircle.sprite = optionACircle;
                else if (selectedOption == 1)
                    choiceDisplayCircle.sprite = optionBCircle;
            }
        }

        public void ConfirmationSpriteUncast()
        {
            if (choiceDisplayLetter != null)
                choiceDisplayLetter.text = Convert.ToChar(65 + selectedOption).ToString();

            if (choiceDisplayCircle != null)
            {
                if (selectedOption == 0)
                    choiceDisplayCircle.sprite = optionACircleUncast;
                else if (selectedOption == 1)
                    choiceDisplayCircle.sprite = optionBCircleUncast;
            }
        }

        public void SetAnimatorBoolOn(string bName)
        {
            animationController.SetBool(bName, true);
        }

        public void SetAnimatorBoolOff(string bName)
        {
            animationController.SetBool(bName, false);
        }

        public void OnStart()
        {
            gameState.AddVoter(ballotID);
        }

    }
}


