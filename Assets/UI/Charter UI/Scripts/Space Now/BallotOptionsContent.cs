using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharterRules.SpaceNow.GameStateModule;

namespace CharterRules.BallotModule
{
    public class BallotOptionsContent : MonoBehaviour
    {
        public SpaceNowGameState gameState;

        public TMP_Text onBallotSituation;
        public TMP_Text optionA;
        public TMP_Text optionB;

        private void OnEnable()
        {
            SetOptionsContent();
        }

        public void SetOptionsContent()
        {
            if (gameState.currentQuestion != null)
            {
                if (onBallotSituation != null)
                    onBallotSituation.text = gameState.currentQuestion.onBallot;

                if (optionA != null)
                    optionA.text = gameState.currentQuestion.options.optionA.resolution;

                if (optionB != null)
                    optionB.text = gameState.currentQuestion.options.optionB.resolution;
            }
        }

    }
}

