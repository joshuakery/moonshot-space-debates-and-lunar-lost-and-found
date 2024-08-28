using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharterRules.SpaceNow.GameStateModule;

namespace CharterRules.BallotModule
{
    public class BallotResultsDataViz : MonoBehaviour
    {
        public SpaceNowGameState gameState;

        public TMP_Text percentageDisplay;
        public TMP_Text textDisplay;

        public Image aSlice;
        public Image bSlice;

        public void SetContent(int selectedOption)
        {

            if (gameState.currentResponses != null)
            {
                float unrounded = (float)gameState.currentResponses[selectedOption] / (float)gameState.totalCurrentResponses * 100;
                float percent = Mathf.Floor(unrounded);

                if (unrounded > 0 && unrounded < 1) //for very small numbers greater than 0
                {
                    percentageDisplay.text = "<1%";
                    textDisplay.text = "Less than 1% of visitors agreed with your choice.";
                }
                else
                {
                    percentageDisplay.text = System.String.Format("{0}%", percent);
                    textDisplay.text = System.String.Format("{0}% of visitors agreed with your choice.", percent);
                }

                SetPieChart(selectedOption, unrounded);

                //int totalResponses = gameState.currentResponses 
                //switch (selectedOption)
                //{
                //    case 0:
                //        float percent = Mathf.Round(gameState.currentResponses[selectedOption] / gameState.totalCurrentResponses);

                //        break;
                //    case 1:

                //        break;
                //}
            }
        }

        public void SetPieChart(int winner, float percent)
        {
            switch (winner)
            {
                case 0:
                    aSlice.transform.SetAsLastSibling();
                    aSlice.fillAmount = percent / 100;
                    bSlice.fillAmount = 1f;
                    break;
                case 1:
                    bSlice.transform.SetAsLastSibling();
                    bSlice.fillAmount = percent / 100;
                    aSlice.fillAmount = 1f;
                    break;
            }
        }
    }
}


