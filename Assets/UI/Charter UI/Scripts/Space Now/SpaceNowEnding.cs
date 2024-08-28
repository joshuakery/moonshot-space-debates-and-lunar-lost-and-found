using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharterRules.SpaceNow.GameStateModule;

namespace CharterRules.SpaceNow.Windows
{
    public class SpaceNowEnding : MonoBehaviour
    {
        public SpaceNowGameState gameState;

        public TMP_Text subtitle;

        public RawImage icon;
        public RawImage illustration;
        public TMP_Text funFact;

        public void SetContent()
        {
            if (gameState.currentQuestion != null)
            {
                if (subtitle != null)
                    subtitle.text = gameState.currentQuestionSet.subtitle;

                if (icon != null)
                    icon.texture = gameState.currentQuestionSet.icon;

                if (illustration != null)
                    illustration.texture = gameState.currentQuestionSet.illustration;

                if (funFact != null)
                    funFact.text = gameState.currentQuestionSet.funFact;

            }
        }
    }
}


