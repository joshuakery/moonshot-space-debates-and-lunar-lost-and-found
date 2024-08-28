// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using CharterRules.GameState;

// public class SecondaryQuestionPanel : GenericWindow
// {
//     public TMP_Text number;
//     public TMP_Text title;
//     public TMP_Text situation;
//     public TMP_Text optionA;
//     public TMP_Text optionB;

//     public TMP_Text resolutionHeader;
//     public TMP_Text resolution;

//     public QuestionData currentQuestionData;

//     public void SetTexts()
//     {
//         currentQuestionData = gameState.GetCurrentQuestionData();

//         number.text = "Question " + (gameState.currentQuestionID + 1).ToString();
//         title.text = currentQuestionData.title;
//         situation.text = currentQuestionData.situation;
//         optionA.text = currentQuestionData.options.optionA.argument;
//         optionB.text = currentQuestionData.options.optionB.argument;

//     }

//     public void ShowWinner()
//     {

//         int[] winners = gameState.GetWinners(gameState.currentQuestionID);
//         if (winners.Length == 0)
//         {
//             resolutionHeader.text = "TOO BAD!";
//             resolution.text = "Your team didn't make a decision!";
//         }

//         int winner = winners[0];

//         if (winner == 0)
//         {
//             resolutionHeader.text = "YOUR TEAM CHOSE";
//             resolution.text = currentQuestionData.options.optionA.resolution;
//         }
//         else if (winner == 1)
//         {
//             resolutionHeader.text = "YOUR TEAM CHOSE";
//             resolution.text = currentQuestionData.options.optionB.resolution;
//         }
        
//     }
// }
