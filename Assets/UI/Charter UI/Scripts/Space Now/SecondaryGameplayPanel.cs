using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CharterRules.SpaceNow.GameStateModule;

public class SecondaryGameplayPanel : MonoBehaviour
{
    public SpaceNowGameState gameState;

    public TMP_Text headerText;

    public TMP_Text chosenText;

    public void SetHeader()
    {
        if (headerText != null)
        {
            headerText.text = System.String.Format("{0}: <mspace=0.75em>{1}/{2}</mspace>",
                gameState.currentQuestionSet.title,
                gameState.currentQuestionIndex + 1,
                gameState.currentQuestionSet.questions.Count
            );
        }

    }

    public void SetContent()
    {
        if (gameState.currentQuestion != null)
        {
            chosenText.text = gameState.currentQuestion.onBallot;
        }
    }

    //public void SetContent(int selectedOption)
    //{
    //    if (gameState.currentQuestion != null)
    //    {
    //        switch (selectedOption)
    //        {
    //            case 0:
    //                if (chosenText != null)
    //                    chosenText.text = gameState.currentQuestion.options.optionA.resolution;
    //                break;
    //            case 1:
    //                if (chosenText != null)
    //                    chosenText.text = gameState.currentQuestion.options.optionB.resolution;
    //                break;
    //        }
    //    }
    //}
}
