using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharterRules.SpaceNow.GameStateModule;

namespace CharterRules.BallotModule
{
    public class BallotResultsContent : MonoBehaviour
    {
        public SpaceNowGameState gameState;

        public TMP_Text chosenText;
        public TMP_Text chosenLetter;
        public Image chosenImage;

        public Sprite optionASprite;
        public Sprite optionBSprite;

        public void SetContent(int selectedOption)
        {
            if (gameState.currentQuestion != null)
            {
                switch (selectedOption)
                {
                    case 0:
                        if (chosenText != null)
                            chosenText.text = gameState.currentQuestion.options.optionA.resolution;
                        if (chosenLetter != null)
                            chosenLetter.text = "A";
                        if (chosenImage != null)
                            chosenImage.sprite = optionASprite;
                        break;
                    case 1:
                        if (chosenText != null)
                            chosenText.text = gameState.currentQuestion.options.optionB.resolution;
                        if (chosenLetter != null)
                            chosenLetter.text = "B";
                        if (chosenImage != null)
                            chosenImage.sprite = optionBSprite;
                        break;
                }
            }
        }
    }
}


