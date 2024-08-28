using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharterRules.SpaceNow.GameStateModule;

namespace CharterRules.BallotModule
{
    public class BallotNextButton : MonoBehaviour
    {
        public SpaceNowGameState gameState;

        public Canvas nextQuestionButton;
        public Canvas wrapUpButton;

        public void ShowNextButton()
        {
            if (gameState.nextQuestion != null)
            {
                nextQuestionButton.enabled = true;
                wrapUpButton.enabled = false;
            }
            else
            {
                nextQuestionButton.enabled = false;
                wrapUpButton.enabled = true;
            }

        }
    }
}

